using ConstructionNew.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConstructionNew.Models
{
    public class ChatMessage
    {
        [Display(Name = "ID")]
        public Guid ChatMessageId { get; set; }
        [Display(Name = "Sender")]
        public string Sender { get; set; }
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}