using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManagementPortal.Models
{
    public class CreateUserRequest
    {
        [Key]
        public Guid UserCreationRequestId { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Confirmation Code")]
        public int ConfirmationCode { get; set; }
    }
}