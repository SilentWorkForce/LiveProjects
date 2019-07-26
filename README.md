# Live Project

## Introduction
For the last 4-6 weeks of my time at the tech academy, I worked with my peers in a team developing full functioning websites in both MVC/MVVM Web Application in C# and Python using django. I worked on 3 different websites each on a different stage of developement, the first one on a pre exsiting codebase was a great learning oppertunity for fixing bugs, cleaning up code, and adding requested features.
the second project was mid way through which I found to be a bit more friendly and I saw how a good developer works with what they have to make a quality product. The third Project was brand new and was easliy my favorite to work on as I could build and work with code that has nothing to conflict with, Of course occasionally some conflicts happened when several new projects uploaded at the same time and they didnt agree with eachother. I worked on several back end and front end stories that I am very proud of. Because much of the first site had already been built, there were also a good deal of front end stories and UX improvements that needed to be completed, all of varying degrees of difficulty. Everyone on the team had a chance to work on front end and back end stories. Over several weeks through the sprint I also had the opportunity to work on some other project management and team programming skills that I'm confident I will use again and again on future projects.

Below are descriptions of the stories I worked on, along with code snippets.

## Back End Stories
* DropDownList
* Filtering DropDownList
* Login Page
* Create User Profile
## Front End Stories
* Made UI ViewButtons
* Back to home, Delete, Edit, Create.
* Functional Paths
* Front login page
* PasswordHash Invisiblity


### DropDownFilterList C sharp .Net Framework MVC (Pull from exsiting database)
This was easily the most challenging within C sharp I had to create a drop down menu,
Then that drop down would need to pull from the exsiting SQL database of users and admins,
And finally show the names on the drop down with the ability to select and view details.

  > [HttpPost]
        public ViewResult Index(string SearchWeek, string Person)
        {   // Filter jobs results by week  
            if (SearchWeek != "")
            {
                string[] weekParts = SearchWeek.Split(' ');
                DateTime weekStart = Convert.ToDateTime(weekParts[0]);
                DateTime weekEnd = Convert.ToDateTime(weekParts[2]).AddDays(1);               
                var jobSearch = (from x in db.Jobs join y in db.Schedules on x.JobId equals y.Job.JobId
                                 where (y.StartDate >= weekStart) && (y.StartDate <= weekEnd)
                                 || (y.EndDate <= weekEnd) && (y.EndDate >= weekStart)
                                 || (y.StartDate <= weekStart) && (y.EndDate >= weekEnd)
                                 || (y.StartDate <= weekStart) && (y.EndDate == null)
                                 select x).Distinct().ToList();                
                return View(jobSearch);          
            }
            // Filter jobs assigned to the selected user/employee
            else if (Person != "")
            {
                var jobSearch = (from x in db.Jobs
                                 join y in db.Schedules on x.JobId equals y.Job.JobId
                                 where y.Person.Id == Person && x.Schedules.Count > 0
                                 select x).Distinct().ToList();
                return View(jobSearch);
            }
            // Show all scheduled jobs
            else
            {
                var jobs = db.Jobs.Where(j => j.Schedules.Count > 0).ToList();
                return View(jobs);
            }                                 
        }

 
 ### Login Page Python Django (Create User page)
I was tasked with creating a front login page and user creation form, which includes many pages, Including:
* Password Reset, confirm, complete, done, email.
* Login page
* Registration/Signup
* Urls and Views
* Templates

Below is just some of the code I used for the login/user creation, I have all the code on github.

urlpatterns = [
    path('', views.SignUp),
    path('login/', views.login),
]

class SignUp(generic.CreateView):
    form_class = UserCreationForm
    success_url = reverse_lazy('login')
    template_name = 'signup.html'
    

def SignUp(request):
    if request.method == 'POST':
        form = UserCreationForm(request.POST)
        if form.is_valid():
            form.save()
            username = form.cleaned_data.get('username')
            raw_password = form.cleaned_data.get('password1')
            user = authenticate(username=username, password=raw_password)
            login(request, user)
            return redirect('home')
    else:
        form = UserCreationForm()
    return render(request, 'signup.html', {'form': form})

### Front End C sharp (Viewbuttons with Submit, Back to list, Delete, Details, Edit)
I had to make buttons with functionality on a new project for a management portal website for admins and team leads.

<p>
<a type="button" class="btn btn-sm btn-primary" href="@Url.Action("Create")">
<i class="fa fa-plus-square"></i> Create New
</a>   
</p>
<table class="table table-striped table-bg table-hover">
<tr>
<th scope="col">
@Html.DisplayNameFor(model => model.StartDate)
</th>
<th scope="col">
@Html.DisplayNameFor(model => model.EndDate)
</th>
<th scope="col"></th>
</tr>
@foreach (var item in Model) {
<tr scope="row">
<td>
@Html.DisplayFor(modelItem => item.StartDate)
</td>
<td>
@Html.DisplayFor(modelItem => item.EndDate)
</td>
<td class="text-right">
<a type="button" class="btn btn-sm btn-primary" href="@Url.Action("Edit", new { id = item.ScheduleId })">
<span>
<i class="fa fa-pencil"></i> Edit
</span>
</a>
<a type="button" class="btn btn-sm btn-primary" href="@Url.Action("Details", new { id = item.ScheduleId })">
<span>
<i class="fa fa-list"></i> Details
</span>
</a>
<a type="button" class="btn btn-sm btn-primary" href="@Url.Action("Delete", new { id = item.ScheduleId })">
<span>
<i class="fa fa-trash"></i> Delete
</span>
</a>
</td>
</tr>
}

</table>



### Front End C sharp (Remove an exsiting nav bar link & Make Password Text field show as invisible or astriks)

 public string PasswordHash { get; set; }
 [Display(Name = "First Name")]

                    <!--Admin-->
                    <li class="has-sub">
                        <a class="nav-title" href="#"><i class="fa fa-clipboard icon-padding" onclick="openNav()"></i>Admin</a>
                        <ul>
                            <li>@Html.ActionLink("Some Link", "Index", new { controller = "Home" }, new { @class = "dropdown-item" })</li>
                            <li>@Html.ActionLink("Another Link", "Index", new { controller = "Home" }, new { @class = "dropdown-item" })</li>

                        </ul>
                    </li>

*Jump to: [Back End Stories](#back-end-stories), [Other Skills](#other-skills-learned), [Page Top](#live-project)*



### Extra fixes
Found typos on the websites and random little tweeks here and there to clean up the code or make a button fit better.



*Jump to: [Back End Stories](#back-end-stories), [Other Skills](#other-skills-learned), [Page Top](#live-project)*

## Other Skills Learned
* Working with a team
* Devops
* Slack
* Visual Studio troubleshooting
* Studio Code Troubleshooting
* Confidence
   
  
  
*Jump to: [Back End Stories](#back-end-stories), [Other Skills](#other-skills-learned), [Page Top](#live-project)*
