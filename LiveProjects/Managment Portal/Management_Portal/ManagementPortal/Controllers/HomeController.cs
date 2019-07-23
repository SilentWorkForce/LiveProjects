using ManagementPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagementPortal.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (User.Identity != null)
            {
                return View("Dashboard");
            }
            return View();
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SignIn()
        {
            return View();
        }

        public ActionResult Chat()
        {
            using (var context = new ApplicationDbContext())
            {
                var messages = from m in context.ChatMessages                                       //Linq statement to query all saved chat messages from
                               orderby m.Date descending                                            //ChatMessage table.
                               select m;
                return View(messages.ToList());             
            }
        }
        public ActionResult Dashboard()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            return View();
        }
    }
}