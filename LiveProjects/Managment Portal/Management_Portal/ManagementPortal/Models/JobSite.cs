using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ManagementPortal.Models
{
    public class Jobsite
    {
        [DisplayName("Job Site ID")]
        public int JobSiteID { get; set; }
        [DisplayName("Address")]
        public string Address { get; set; }
        [DisplayName("Town")]
        public string Town { get; set; }
        [DisplayName("State")]
        public string State { get; set; }
        [DisplayName("Zip")]
        public string Zip { get; set; }
    }
}