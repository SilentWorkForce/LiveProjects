using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ManagementPortal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; internal set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        //add the user identity here for the aspnetuser table.

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        //A DBset property (representing table in DB) will be created for this entity set
        //add DBsets here:
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<CompanyNews> CompanyNews { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<EventModel> Events { get; set; }
        public DbSet<Jobsite> Jobsites { get; set; }
        public DbSet<CreateUserRequest> CreateUserRequests { get; set; }


        public DbSet<ShiftTime> ShiftTime { get; set; }
        //end dbsets
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        //Scafolding databases
        //public System.Data.Entity.DbSet<ManagementPortal.Models.Jobsite> Jobsites { get; set; }
        //Area specific scaffolding databases
        public System.Data.Entity.DbSet<ManagementPortal.Areas.Employee.Models.EmployeeInfo> Employees { get; set; }
    }
}