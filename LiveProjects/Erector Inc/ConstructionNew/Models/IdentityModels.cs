using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using ConstructionNew.Enums;
using ConstructionNew.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ConstructionNew.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [Display(Name = "First Name")]
        public string FName { get; set; }
        [Display(Name = "Last Name")]
        public string LName { get; set; }
        [Display(Name = "Work Category")]
        public WorkType WorkType { get; set; }
        [Display(Name = "User Role")]
        public string UserRole { get; internal set; }
        [Display(Name = "Suspended")]
        public bool Suspended { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Job> Jobs { get; set; }
        //A DBset property (representing table in DB) will be created for this entity set
        //With removal of JobOther & JobSite classes, I have commented these out of the DbContext - Trey Hamel
        //public DbSet<JobOther> JobOthers { get; set; }
        //public DbSet<JobSite> JobSites { get; set; }
        public DbSet<CreateUserRequest> CreateUserRequests { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<CompanyNews> CompanyNews { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ShiftTime> ShiftTimes { get; set; }
        public object AspNetUsers { get; internal set; }




        // This broke my database for some reason?
        // public System.Data.Entity.DbSet<ConstructionNew.Models.ApplicationUser> ApplicationUsers { get; set; }
    }

}