using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Model;
using ConstructionNew.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Linq;
using System.Web.Mvc.Html;

[assembly: OwinStartupAttribute(typeof(ConstructionNew.Startup))]
namespace ConstructionNew
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();

            // run our create roles method
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // This and the associated if statement are meant to be temporary.
                // Remove when the admin binding bug is fixed.
                var siteAdminExists = (from u in context.Users
                                       where u.UserName == "SiteAdmin"
                                       select u).Any();

                //create admin role
                // We create an admin in Configuration.cs so do we need this still?
                if (!roleManager.RoleExists("Admin"))
                {
                    roleManager.Create(new IdentityRole("Admin"));
                }
                    if (!siteAdminExists)
                {
                    PasswordHasher hasher = new PasswordHasher();
                    var admin = new ApplicationUser();
                    admin.FName = "Site";
                    admin.LName = "Admin";
                    admin.UserName = "SiteAdmin";
                    admin.Email = "test@gmail.com";
                    admin.UserRole = "Admin";
                    var checkUserResult = userManager.Create(admin, "@Pass1234");
                    if (checkUserResult.Succeeded)
                    {
                        var AdminCreated = userManager.AddToRole(admin.Id, "Admin");
                    }
                }

                if (!roleManager.RoleExists("Manager"))
                {
                    roleManager.Create(new IdentityRole("Manager"));

                }
                if (!roleManager.RoleExists("Employee"))
                {
                    roleManager.Create(new IdentityRole("Employee"));

                }

                // Add FName and LName to default users.
                var joeUpdated = (from user in context.Users
                    where user.UserName == "Joe"
                    select user).FirstOrDefault();
                if (joeUpdated != null)
                {
                    joeUpdated.FName = "Joe";
                    joeUpdated.LName = "Johnson";
                }

                var jillUpdated = (from user in context.Users
                    where user.UserName == "Jill"
                    select user).FirstOrDefault();
                if (jillUpdated != null)
                {
                    jillUpdated.FName = "Jill";
                    jillUpdated.LName = "Lee";
                }

                var jackUpdated = (from user in context.Users
                    where user.UserName == "Jack"
                    select user).FirstOrDefault();
                if (jackUpdated != null)
                {
                    jackUpdated.FName = "Jack";
                    jackUpdated.LName = "Wagner";
                }

                // Save changes
                context.SaveChanges();
            }
        }
    }
}
