using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Mime;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using ConstructionNew.Models;
using Microsoft.Ajax.Utilities;

namespace ConstructionNew.Controllers
{
    public class DashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Admin()
           
        {
       
            return View();
        }

        public ActionResult Index()
        {
            //checks if the user is on a mobile device and if so, redirects to "ActionResult" "MobileDashboard" to render the mobile dashboard.
            if (Request.Browser.IsMobileDevice)
            {
                return RedirectToAction("MobileDashboard");
            }
            ViewBag.Message = "Admin dashboard";

            var userList = new List<SelectListItem> { new SelectListItem { Selected = true, Text = @"All", Value = "", } };
            using (db)
            {
                db.Users.ForEach(u => userList.Add(new SelectListItem
                {
                    Text = String.Format("{0}, {1}", u.LName, u.FName),
                    Value = u.Id
                }));
            }


            ViewBag.UserList = userList;
            return View();
        }

        //"Authorize" checks if the user is registered and if so, will allow the ActionResult to return the view "MobileDashboard".
        [Authorize]
        public ActionResult MobileDashboard()
        {
            return View();
        }

        public ActionResult ChatBox()
        {
            return View();
        }

        public ActionResult Chat()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult MasterSchedulePartial()
        {
            // This List should be "Filter List to groups based on active Jobs for the week" -- from the schedule display flow chart
            // Not sure by what criteria this should be determined.
            // Perhaps jobs could query for Jobs that have Schedules that have StartDates that are within the next 7 days, something like that.
            // For now, This list is only filtering for Jobs that have Schedules.
            // No Ordering is applied either.
            // This avoids displaying the default jobs that are currently empty.
            // -Jeremy Stewart 5-10-19

            var jobs = db.Jobs.Where(j => j.Schedules.Count > 0).ToList();
            return PartialView("_MasterSchedulePartial", jobs);
        }

        [ChildActionOnly]
        public ActionResult EmployeeSchedulePartial()
        {
            var jobs = db.Jobs.Where(j => j.Schedules.Count > 0).ToList();
            return PartialView("_EmployeeSchedulePartial", jobs);
        }

        // controller to call the partial view and populate the modal
        public ActionResult CompanyNewsModal()
        {
            
            return PartialView("_CreateCompanyNewsPartial");
        }
    }
}
    