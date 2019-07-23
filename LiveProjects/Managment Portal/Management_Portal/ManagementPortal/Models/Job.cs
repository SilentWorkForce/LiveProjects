using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ManagementPortal.Enums;
using System.Web.Mvc;

namespace ManagementPortal.Models
{
    public class Job
    {
        [Key]
        [Required(ErrorMessage="Required Field. Please enter an ID #: "),Display(Name = "User Id")]
        public string Id { get; set; }
        [Required(ErrorMessage ="Required Field. Please enter a valid Email: "),Display(Name = "Email"),DataType(DataType.EmailAddress)] 
        public string Email { get; set; }
        [Required(ErrorMessage ="Required Field. Please enter a Password: "),Display(Name = "PasswordChar")]
        public char PasswordChar { get; set; }
        [Required(ErrorMessage ="Required Field. Please enter a First Name:"),Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage ="Required Field. Please enter a Last Name: "),Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage ="Required Field. Please enter a Work Type >> Leadman,Foreman,ExpMBA, or NewMBA: "),Display(Name = "Work Type")]
        public WorkType WorkType { get; set; }
        [Display(Name = "Phone Number"), DataType(DataType.PhoneNumber),RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone Number")]
        [Required(ErrorMessage = "Required Field. Please enter a valid Phone Number: ")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Required Field. Please enter a User Role: "),Display(Name = "User Role")]
        public string UserRole { get; set; }
        [Required(ErrorMessage ="Required Field. Please enter a State: "),Display(Name = "State")]
        public string State { get; set; }
        public IEnumerable<SelectListItem> States { get; set; } 
        [Required(ErrorMessage ="Required Field. Please enter a County: "),Display(Name = "County")]
        public string County { get; set; }
        [Required(ErrorMessage ="Required Field.  Please enter a Zip Code: "),Display(Name = "Zip Code")]
        public string ZipCode { get; set; } 
        [Display(Name = "Suspended")]
        public bool Suspended { get; set; }
    }
}