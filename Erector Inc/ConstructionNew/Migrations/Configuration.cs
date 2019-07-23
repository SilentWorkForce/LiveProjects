namespace ConstructionNew.Migrations
{
    using ConstructionNew.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ConstructionNew.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ConstructionNew.Models.ApplicationDbContext context)
        {

            if (!context.Users.Any())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var roleA = roleManager.FindByName("Admin");
                if (roleA == null)
                {
                    roleA = new IdentityRole("Admin");
                    roleManager.Create(roleA);
                }

                var roleM = roleManager.FindByName("Manager");
                if (roleM == null)
                {
                    roleM = new IdentityRole("Manager");
                    roleManager.Create(roleM);
                }

                var roleE = roleManager.FindByName("Employee");
                if (roleE == null)
                {
                    roleE = new IdentityRole("Employee");
                    roleManager.Create(roleE);
                }

                var newAdmin = new ApplicationUser()
                {
                    Id = "5ebdde05-45e8-479b-a371-948c80331ab0",
                    UserName = "Jack",
                    Email = "hello@gmail.com",
                    PhoneNumber = "555-555-1234",
                    WorkType = Enums.WorkType.Foreman,
                    UserRole = "Admin"
                };
                userManager.Create(newAdmin, "123ABCdef!");
                userManager.AddToRole(newAdmin.Id, newAdmin.UserRole);

                var newManager = new ApplicationUser()
                {
                    Id = "e8130867-6398-4b57-ba38-99799a292a8b",
                    UserName = "Jill",
                    Email = "test@123.com",
                    PhoneNumber = "555-909-0987",
                    WorkType = Enums.WorkType.LeadMan,
                    UserRole = "Manager"
                };
                userManager.Create(newManager, "123!@#ABCabc");
                userManager.AddToRole(newManager.Id, newManager.UserRole);

                var newEmployee = new ApplicationUser()
                {
                    Id = "7874562f-5bb5-4adf-9c1f-3454fc15fb8d",
                    UserName = "Joe",
                    Email = "joe@doe.com",
                    PhoneNumber = "555-555-4567",
                    WorkType = Enums.WorkType.NewMBA,
                    UserRole = "Employee"
                };
                userManager.Create(newEmployee, "789asdASD!");
                userManager.AddToRole(newEmployee.Id, newEmployee.UserRole);

            }

            List<ApplicationUser> applicationUsers = context.Users.ToList();
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            // ----------------------------------------------------------------------------------------------------
            // The seed data for Users and Jobs have been modified to have static guids.
            // This allows the seed data to remain the same each time you update or recreate the database, and also
            // the same guids will appear in every local copy of the db. 
            // This is neccessary so that the Schedule seed data can refer to existing jobs and users.
            // -Jeremy Stewart 4-28-2019---------------------------------------------------------------------------

            //PasswordHasher hasher = new PasswordHasher();
            //var applicationUsers = new List<ApplicationUser>
            //{
            //    new ApplicationUser
            //    {
            //        Id ="5ebdde05-45e8-479b-a371-948c80331ab0",Email ="hello@gmail.com",
            //        PasswordHash =hasher.HashPassword("123ABCdef!"),
            //        UserName ="Jack",SecurityStamp = Guid.NewGuid().ToString(),
            //        WorkType = 0,UserRole = "Admin"},

            //    new ApplicationUser()
            //    {
            //        Id ="e8130867-6398-4b57-ba38-99799a292a8b",
            //        Email ="test@123.com",
            //        PasswordHash =hasher.HashPassword("123!@#ABCabc"),
            //        UserName ="Jill",
            //        SecurityStamp = Guid.NewGuid().ToString(),
            //        WorkType = 0,
            //        UserRole = "Manager"
            //    },

            //    new ApplicationUser()
            //    {
            //        Id ="7874562f-5bb5-4adf-9c1f-3454fc15fb8d",
            //        Email ="joe@doe.com",
            //        PasswordHash =hasher.HashPassword("789asdASD#!"),
            //        UserName ="Joe",
            //        SecurityStamp = Guid.NewGuid().ToString(),
            //        WorkType = 0,
            //        UserRole = "Employee"
            //    },
            //};
            //applicationUsers.ForEach(x => context.Users.AddOrUpdate(u => u.UserName, x));
            //context.SaveChanges();

            //The table ShiftTimes filled out after the ModifyJobModel migration on 31/05/2019
            var ShiftTimes = new List<ShiftTime>
            {
                new ShiftTime{ShiftTimeId=Guid.Parse("69121893-3afc-4f92-85f3-40bb5e7c7e29"), Default = "08:00", Monday = "08:00", Tuesday = "09:00", Wednesday = "08:00", Thursday = "07:00", Friday = "08:00"},
                new ShiftTime{ShiftTimeId=Guid.Parse("345df545-678d-cce6-56ab-345df87db234"), Default = "07:30"},
                new ShiftTime{ShiftTimeId=Guid.Parse("2dc45f56-ef98-5634-d743-345abc45dc82"), Default = "09:15"},
            };
            ShiftTimes.ForEach(x => context.ShiftTimes.AddOrUpdate(j => j.ShiftTimeId, x));
            context.SaveChanges();


            var Jobs = new List<Job>
            {
                new Job{JobId=Guid.Parse("36c3d745-2db4-4e41-8a39-5cf3ecc84c32"), JobTitle="Hillsboro Aviation", JobNumber="375",
                    StreetAddress ="3845 NE 30th Ave", City = "Hillsboro", State=Enums.State.OR, Zipcode=97124,Note="Bring hearing protection",
                    ShiftTimes = ShiftTimes.Where(x => x.ShiftTimeId == Guid.Parse("69121893-3afc-4f92-85f3-40bb5e7c7e29")).First()},
   
                new Job{JobId=Guid.Parse("36a8eaa1-7d91-4902-90f4-215827050817"), JobTitle="U-Haul of South Salem", JobNumber="425",
                    StreetAddress ="1395 12th St SE", City="Salem", State=Enums.State.OR, Zipcode=97302, Note="",
                    ShiftTimes = ShiftTimes.Where(x => x.ShiftTimeId == Guid.Parse("345df545-678d-cce6-56ab-345df87db234")).First()},
              
                new Job{JobId=Guid.Parse("e7e7a3db-62b9-4eb0-867f-3410c8d28c13"), JobTitle="Olympia Regional Airport", JobNumber="475",
                    StreetAddress="7643 Old Hwy 99 SE", City="Olympia", State=Enums.State.WA, Zipcode=98501, Note="Bring eye protection",
                    ShiftTimes = ShiftTimes.Where(x => x.ShiftTimeId == Guid.Parse("2dc45f56-ef98-5634-d743-345abc45dc82")).First()},
            };
            Jobs.ForEach(x => context.Jobs.AddOrUpdate(j => j.JobId, x));
            context.SaveChanges();
         
            var DefaultJobs = new List<Job>
            {
                new Job{ JobId=Guid.Parse("32526e98-ae65-4382-bbdd-8259434263d3"), JobTitle="Injured", JobNumber=null,
                    StreetAddress ="", City = "", State=Enums.State.OR, Note=" " },
                new Job{ JobId=Guid.Parse("ae3eb11a-dc06-467e-8a51-9d398e933887"), JobTitle="PTO", JobNumber=null,
                    StreetAddress ="", City="", State=Enums.State.OR, Note="" },
                new Job { JobId = Guid.Parse("4469ee0a-2293-4384-bfb8-e0ce48997f06"), JobTitle = "Laid Off", JobNumber = null,
                    StreetAddress = "", City = "", State = Enums.State.OR, Note = "" },
                new Job{ JobId=Guid.Parse("d3ec401c-ac94-495b-9dee-c1f62d0711d0"), JobTitle="Other", JobNumber=null,
                    StreetAddress="", City="", State=Enums.State.OR, Note=""}
            };
            DefaultJobs.ForEach(x => context.Jobs.AddOrUpdate(j => j.JobId, x));
            context.SaveChanges();

            // Model "Schedule" class conflicts with Migrations "Schedule" class in this namespace.
            // Using "Models.Schedule" in this seed method out of neccessity to avoid naming conflict.
            //
            // Utilizing List<Job> and List<ApplicationUser> previously created from above seed data.
            // -Jeremy Stewart 4-28-19
          
            var Schedules = new List<Models.Schedule>
            {
                new Models.Schedule
                {
                    ScheduleId=Guid.Parse("1283c061-9666-4b22-95fe-5c1f8cb045d1"),
                    Job=Jobs.Where(x => x.JobId == Guid.Parse("36c3d745-2db4-4e41-8a39-5cf3ecc84c32")).First(),
                    Person=applicationUsers.Where(u => u.UserName == "Jack").First(),
                    StartDate=DateTime.Parse("5/1/2019 08:00:00"),

                    EndDate=DateTime.Parse("5/1/2019 08:00:00")
                },
                new Models.Schedule
                {
                    ScheduleId=Guid.Parse("a05f2335-d7af-4b09-b636-26bf39f84829"),
                    Job=Jobs.Where(x => x.JobId == Guid.Parse("36a8eaa1-7d91-4902-90f4-215827050817")).First(),
                    Person=applicationUsers.Where(u => u.UserName == "Jill").First(),
                    StartDate=DateTime.Parse("5/1/2019 06:00:00"),
                    EndDate=DateTime.Parse("5/6/2019 16:30:00")
                },
                new Models.Schedule
                {
                    ScheduleId=Guid.Parse("32822356-24ad-4e8e-9415-c3ac5e3ca4b9"),
                    Job=Jobs.Where(x => x.JobId == Guid.Parse("e7e7a3db-62b9-4eb0-867f-3410c8d28c13")).First(),
                    Person=applicationUsers.Where(u => u.UserName == "Joe").First(),
                    StartDate=DateTime.Parse("5/11/2019 08:00:00"),
                    EndDate=DateTime.Parse("5/17/2019 16:30:00")
                },
                new Models.Schedule
                {
                    ScheduleId=Guid.Parse("94624e75-13d7-425f-bbb1-d4ea1dcfec20"),
                    Job=Jobs.Where(x => x.JobId == Guid.Parse("e7e7a3db-62b9-4eb0-867f-3410c8d28c13")).First(),
                    Person=applicationUsers.Where(u => u.UserName == "Joe").First(),
                    StartDate=DateTime.Parse("5/20/2019 08:00:00"),
                    EndDate=null
                }
            };
            Schedules.ForEach(x => context.Schedules.AddOrUpdate(s => s.ScheduleId, x));            
            context.SaveChanges();

            // Dummy ChatMessages to be seeded
            var ChatMessages = new List<ChatMessage>
            {
                new ChatMessage
                {
                    ChatMessageId=Guid.Parse("4605fdb7-8c40-40c4-a561-ca417f18d86d"),
                    Sender= "Jill L",
                    Date = DateTime.Now,
                    Message = "I have made a news post about the upcoming party."
                },
                new ChatMessage
                {
                    ChatMessageId=Guid.Parse("b2425f93-30fa-440e-b5bf-6893cc487d8d"),
                    Sender="Jack W",
                    Date = DateTime.Now.AddSeconds(1),
                    Message = "Thank you, Jill can you make a news post for our new hire next?"
                },
                new ChatMessage
                {
                    ChatMessageId=Guid.Parse("7bee5676-5f90-4091-8dd9-c2773169a201"),
                    Sender= "Jill L",
                    Date = DateTime.Now.AddSeconds(2),
                    Message = "I will do that now."
                },
                new ChatMessage
                {
                    ChatMessageId=Guid.Parse("9a132c64-8aa5-48a7-b04e-9eef9fb4c8a5"),
                    Sender= "Jack W",
                    Date = DateTime.Now.AddSeconds(3),
                    Message = "Thanks again."
                }
            };
            ChatMessages.ForEach(x => context.ChatMessages.AddOrUpdate(c => c.ChatMessageId, x));
            context.SaveChanges();

            // Dummy NewsItems to be seeded.
            var NewsItems = new List<CompanyNews>
            {
                new CompanyNews
                {
                    DateStamp = Convert.ToString(new DateTime(2019, 5, 7, 8, 30, 00).Ticks),
                    //DateStamp = "5/7/2019",
                    Title = "Company Party",
                    NewsItem = "Our annual company party is going to be held on 6/20/2019",
                    ExpirationDate = new DateTime(2019, 6, 25, 10, 30, 00)
                },
                new CompanyNews
                {
                    DateStamp = Convert.ToString(new DateTime(2019, 5, 5, 8, 30, 00).Ticks),
                    Title = "New Hire",
                    NewsItem = "We've hired on a new employee, please welcome Sam to the team.",
                    ExpirationDate = new DateTime(2019, 7, 15, 9, 45, 33)
                },
                new CompanyNews
                {
                    DateStamp = Convert.ToString(new DateTime(2019, 5, 2, 8, 30, 00).Ticks),
                    Title = "Happy Birthday Joe!!",
                    NewsItem = "Today is Joe's birthday. There will be cake in the break room.",
                    ExpirationDate = new DateTime(2019, 7, 12, 10, 30, 00)
                },
                new CompanyNews
                {
                    DateStamp = Convert.ToString(new DateTime(2019, 3, 1, 8, 30, 00).Ticks),
                    Title = "Website update",
                    NewsItem = "We are having updates made on our website, there may be a bit of down time.",
                    ExpirationDate = new DateTime(2019, 4, 1, 10, 30, 00)
                }

            };
            NewsItems.ForEach(x => context.CompanyNews.AddOrUpdate(n => n.DateStamp, x));
            context.SaveChanges();
        }
    }
}
