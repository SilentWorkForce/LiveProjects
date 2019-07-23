namespace ManagementPortal.Migrations
{
    using ManagementPortal.Models;
    using ManagementPortal.Enums;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Data.Entity.Validation;

    internal sealed class Configuration : DbMigrationsConfiguration<ManagementPortal.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "ManagementPortal.Models.ApplicationDbContext";
        }
        protected override void Seed(ManagementPortal.Models.ApplicationDbContext context)
        {
            
        }
    }
}

//Change made to upload to server
