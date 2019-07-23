using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConstructionNew.Models;

namespace ConstructionNew.Controllers
{
    public class SchedulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public string Person { get; private set; }

        // GET: Schedules
        [Authorize] //used to restrict view of unregistered users
        public ActionResult Index()
        {
            // Added ordering by Startdate to make index view a little easier to read for testing
            // Doesn't need to stay this way. JS 5/5/19
            //return View(db.Schedules.OrderBy(s => s.StartDate).ToList());
            //return View(db.Schedules.OrderBy(s => s.Job.JobNumber).ToList());
            //var jobsAssigned = db.Jobs.OrderBy(s => s.JobNumber).ToList();

            //In order to allow for the handshake between Job library and Schedule library jobs had to be created within the controller
            //Looking for an option to allow for both this and a way to sort by Job number  -- EB 5/24/19

            var jobs = db.Jobs.Where(j => j.Schedules.Count > 0).ToList();
            ViewBag.person = GetUsers();
            return View(jobs);

        }

        [Authorize]
        public ActionResult MySchedulePartial()
        {
            var jobs = db.Jobs.Where(j => j.Schedules.Count > 0).ToList();

            return View(jobs);
        }

        // GET: Schedules/Details/5
        [Authorize] //used to restrict view of unregistered users
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: Schedules/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            // Obtain data for drowpdown lists
            ViewBag.jobsite = GetJobSites();
            ViewBag.person = GetUsers();
            
            return View();
        }


        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(string Person, Guid JobSite, DateTime StartDate, DateTime? EndDate)
        {
            //Prevents Admin from adding suspended users to a schedule 
            ApplicationUser _person = db.Users.Where(u => u.Id == Person).First();
            if (_person.Suspended)
            {
                ModelState.AddModelError("Person", "Suspended users cannot be added to the schedule");
            }
            // EndDate should be after StartDate
            if (EndDate.HasValue && EndDate < StartDate)
            {
                ModelState.AddModelError("EndDate", "You must choose an End Date that is after the Start Date.");
            }            
            else if (ModelState.IsValid)
            {
                // Post Method receives simple parameters from the view
                // Person and JobSite are used to fetch a Job and User from the db
                // which model binding doesn't automatically do.
                Job job = db.Jobs.Where(j => j.JobId == JobSite).First();
                ApplicationUser applicationUser = db.Users.Where(u => u.Id == Person).First();
                // Those can then be passed to the constructor along with the dates to build a schedule.
                Schedule schedule = new Schedule(applicationUser, job, StartDate, EndDate);
                db.Schedules.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // If Schedule creation failed, we need to get users and jobs to pass to the view again...
            ViewBag.jobsite = GetJobSites();
            ViewBag.person = GetUsers();
            // ... and display an error message.
            ModelState.AddModelError(string.Empty, "There was an error and your schedule was not created.");
            return View();
        }

        // GET: Schedules/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.jobsite = GetJobSites(schedule.Job.JobId.ToString()); // Pass in Job Id as string to GetJobsites method to retrieve selected object to be used as default selection. 
            ViewBag.person = GetUsers(schedule.Person.Id); // Pass in Person Id to GetUsers method to retrieve selected object to be used as default selection. 
            
            return View(schedule);
        }
        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ScheduleId,StartDate,EndDate")] Schedule schedule, string Person, Guid JobSite)
        {


            ApplicationUser _person = db.Users.Where(u => u.Id == Person).First();
            Job _job = db.Jobs.Where(j => j.JobId == JobSite).First();

            Schedule Schedule = db.Schedules.Where(s => s.ScheduleId == schedule.ScheduleId).First();
            Schedule.Job = _job;
            Schedule.Person = _person;
            Schedule.EndDate = schedule.EndDate;
            Schedule.StartDate = schedule.StartDate;
            //Prevents Admin from reassigning scedule to a suspended user 
            if (_person.Suspended)
            {
                ModelState.AddModelError("Person", "Suspended users cannot be added to the schedule");           
            }
            if (schedule.EndDate.HasValue && schedule.EndDate < schedule.StartDate)
            {
                ModelState.AddModelError("EndDate", "You must choose an End Date that is after the Start Date.");
            }

            else if (ModelState.IsValid)
            {
                db.Entry(Schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // If Schedule creation failed, we need to get users and jobs to pass to the view again...
            ViewBag.jobsite = GetJobSites();
            ViewBag.person = GetUsers();
            // ... and display an error message.
            ModelState.AddModelError(string.Empty, "There was an error and your schedule was not created.");
            return View(schedule);


        }

        // GET: Schedules/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]


        public ActionResult DeleteConfirmed(Guid id)
        {
            Schedule schedule = db.Schedules.Find(id);
            db.Schedules.Remove(schedule);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public IEnumerable<SelectListItem> GetJobSites(string selectedJobId = null)  // Added selectedJobId that is set to null to be passed in when a default field value is needed
        {
            // Create a list of JobTitle/JobId pairs to pass to the view with viewbag.jobsite
            // The DropDownList will display the JobTitle list and
            // The POST data receives the corresponding JobId

            return db.Jobs.OrderBy(o => o.JobNumber).Select(m => new SelectListItem()
            {
                Value = m.JobId.ToString(),
                Text = m.JobTitle,
                Selected = selectedJobId == m.JobId.ToString() // Add Select variable that holds the Id of the current selected item that gets passed when the corresponding edit button is clicked
            });
        }

        public IEnumerable<SelectListItem> GetUsers(string selectedUserId = null) // Added selectedUserId that is set to null to be passed in when a default field value is needed
        {
            // Create a list of UserName/Id pairs to pass to the view with viewbag.person
            // The DropDownList will display the UserName list and
            // The POST data receives the corresponding Id

            return db.Users.Where(x => x.UserName != "SiteAdmin").OrderBy(o => o.UserName).Select(m => new SelectListItem()
            {
                Value = m.Id,
                Text = (m.FName + " " + m.LName),
                Selected = selectedUserId == m.Id // Add Select variable that holds the Id of the current selected item that gets passed when the corresponding edit button is clicked
            });
        }

        //Creates a list of weeks for a year
        public List<string> FetchWeeks(int year)
        {
            List<string> weeks = new List<string>();
            DateTime startDate = new DateTime(year, 1, 1);
            startDate = startDate.AddDays(1 - (int)startDate.DayOfWeek);
            DateTime endDate = startDate.AddDays(6);
            while (startDate.Year < 1 + year)
            {
                weeks.Add(string.Format("{0:MM/dd/yyyy} to {1:MM/dd/yyyy}", startDate, endDate));
                startDate = startDate.AddDays(7);
                endDate = endDate.AddDays(7);
            }
            return weeks;
        }

        //Convert a list of weeks into IEnumerable<SelectListItem> in order to call DropDownList method in Schedules Index view   
        public IEnumerable<SelectListItem> ListWeeks()
        {
            IEnumerable<SelectListItem> week = FetchWeeks(2019).ConvertAll(w =>
            
                new SelectListItem()
                {
                    Text = w.ToString(),
                    Value = w
                    
                });
            return week;
        }

        [HttpPost]
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

        public List<ApplicationUser> GetEmployees()
        {
            List<ApplicationUser> employees = new List<ApplicationUser>();
            employees = db.Users.ToList();
            return employees;
        }

        public List<Job> GetJobs()
        {
            List<Job> jobs = new List<Job>();
            jobs = db.Jobs.ToList();
            return jobs;
        }

        public ActionResult MasterScheduleEdit()
        {
            dynamic myModel = new ExpandoObject();
            myModel.Employees = GetEmployees();
            myModel.Jobs = GetJobs();
            var weeks = FetchWeeks(2019);
            ViewBag.Weeks = new SelectList(weeks);
            return View(myModel);
        }

        public ActionResult AssignForeman()
        {
            dynamic myModel = new ExpandoObject();
            myModel.Employees = GetEmployees();
            myModel.Jobs = GetJobs();
            return View(myModel);
        }
    }
}