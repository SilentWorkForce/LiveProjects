using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ManagementPortal.Areas.Employee.Models
{
    public class EmployeeInfo
    {
        [DisplayName("ID")]
        public int ID { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Phone Number")]
        [Phone]
        public string Phone { get; set; }

        [DisplayName("Email")]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }
    }
}