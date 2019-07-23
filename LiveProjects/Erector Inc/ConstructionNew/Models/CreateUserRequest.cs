using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConstructionNew.Models
{
    public class CreateUserRequest
    {
        [Key]
        public Guid UserCreationRequestId { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }
        //hidden
        public int ConfirmationCode { get; set; }

        [Display(Name = "User Role")]
        public string UserRoles { get; set; }
    }
}