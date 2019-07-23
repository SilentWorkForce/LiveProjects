using ManagementPortal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System;
using System.Data.Entity.Migrations;


[assembly: OwinStartupAttribute(typeof(ManagementPortal.Startup))]
namespace ManagementPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            createRolesandUsers();
            addDataToTables();

            app.MapSignalR();                //Used to map signalR to the project. Must be after configuration files

        }

        // In this method we will create default User roles and Admin user for login   
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "Admin1";
                user.Email = "admin@gmail.com";

                string userPWD = "1234@!AZ";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);

            }
            context.SaveChanges();
        }

        private void addDataToTables()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var NewsItems = new List<CompanyNews>
                {
                    new CompanyNews
                    {
                        NewsId = 1,
                        DateStamp = System.Convert.ToString(new DateTime(2019, 8, 13, 8, 30, 00).Ticks),
                        //DateStamp = "5/7/2019",
                        Title = "Policy Change",
                        NewsItem = "Due to recent events, fireworks are no longer allowed on the premises. Similarly, we wish Tom a speedy recovery!",
                        ExpirationDate = new DateTime(2019, 10, 25, 10, 30, 00)
                    },
                    new CompanyNews
                    {
                        NewsId = 2,
                        DateStamp = Convert.ToString(new DateTime(2019, 8, 12, 8, 30, 00).Ticks),
                        //DateStamp = "5/7/2019",
                        Title = "Company Party",
                        NewsItem = "Our annual company party is going to be held on 8/12/2019. There will be fireworks in the break room.",
                        ExpirationDate = new DateTime(2019, 10, 25, 10, 30, 00)
                    },
                    new CompanyNews
                    {
                        NewsId = 3,
                        DateStamp = Convert.ToString(new System.DateTime(2019, 8, 5, 8, 30, 00).Ticks),
                        Title = "New Hire",
                        NewsItem = "We've hired on a new employee, please welcome Eric to the team. He will be helping us out with restructuring, so please be nice to Eric.",
                        ExpirationDate = new DateTime(2019, 9, 2, 9, 45, 33)
                    },
                    new CompanyNews
                    {
                        NewsId = 4,
                        DateStamp = Convert.ToString(new DateTime(2019, 7, 2, 8, 30, 00).Ticks),
                        Title = "Happy Retirement, John!",
                        NewsItem = "Today, John Frankenheimer is moving on to enjoy retirement. Way to go, John! We have decided to give John a trampoline as a parting gift. Here's to a long and healthy retirement.",
                        ExpirationDate = new DateTime(2019, 7, 12, 10, 30, 00)
                    },
                    new CompanyNews
                    {
                        NewsId = 5,
                        DateStamp = Convert.ToString(new DateTime(2019, 3, 1, 8, 30, 00).Ticks),
                        Title = "Website update",
                        NewsItem = "The website has been updated. If you are seeing this in the future, you won't, actually...",
                        ExpirationDate = new DateTime(2018, 2, 1, 10, 30, 00)
                    },
                    new CompanyNews
                    {
                        NewsId = 6,
                        DateStamp = Convert.ToString(new DateTime(1901, 3, 1, 8, 30, 00).Ticks),
                        Title = "Happy Mistake Appreciation Day",
                        NewsItem = "You are seeing this because someone screwed up the expiration function on News features. Today is a day we appreciate their mistake and learn from it. " +
                            "There is a chance you will be haunted by the ghosts of mistakes' past, One-eyed Willy, Daisy Whoopsie, and Three-Fingered Bruce. Have a nice day and see you tomorrow for Unit Test Appreciation day.",
                        ExpirationDate = new DateTime(1901, 2, 1, 10, 30, 00)
                    }

                };
            NewsItems.ForEach(x => context.CompanyNews.AddOrUpdate(n => n.NewsId, x));
            context.SaveChanges();

        }


    }

}


