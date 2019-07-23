using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConstructionNew.Models;

namespace ConstructionNew.Controllers
{
    public class JobsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        

        // GET: Jobs
        [Authorize] //used to restrict view of unregistered users
        public ActionResult Index()
        {

            return View(db.Jobs.OrderBy(i => i.JobNumber).ToList());
        }

        // GET: Jobs/Details/5
        [Authorize] //used to restrict view of unregistered users
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }

            var jobDetailsVM = new JobDetailsViewModel
            {
                JobId = job.JobId,
                JobTitle = job.JobTitle,
                JobNumber = job.JobNumber,
                StreetAddress = job.StreetAddress,
                City = job.City,
                State = job.State,
                Zipcode = job.Zipcode,
                Note = job.Note,
                ShiftTimes = Convert.ToString(job.ShiftTimes),
                ForemanName = "None Assigned",
                Phone = "",

            };
            //Get foreman data
            var query = (from a in db.Users
                         join b in db.Schedules on a.Id equals b.Person.Id
                         where a.WorkType == Enums.WorkType.Foreman && b.Job.JobNumber == job.JobNumber
                         select new
                         {
                             a.FName,
                             a.LName,
                             a.PhoneNumber
                         }).FirstOrDefault();
            //Add foreman data to VM if a result was found
            if (query != null)
            {
                jobDetailsVM.ForemanName = query.FName + " " + query.LName;
                jobDetailsVM.Phone = query.PhoneNumber;
            }

            return View(jobDetailsVM);
        }



        // GET: Jobs/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            // Obtain data for drowpdown lists            
            ViewBag.foremen = GetForemen();
            return View();
        }


        //Convert a list of week days into IEnumerable<SelectListItem> in order to call DropDownList method in Job Create view   
        public IEnumerable<SelectListItem> WeekDays()
        {
            List<string> weekDays = new List<string> {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            IEnumerable<SelectListItem> wDays = weekDays.ConvertAll(d =>

                new SelectListItem()
                {
                    Text = d.ToString(),
                    Value = d

                });
            return wDays;
        }
        //Selects a correct collumn in ShiftTimes Table to record a shifttime, called in the ActionResult Create (below) for each extra ShiftTime
        

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "JobId,JobTitle,JobNumber,ShiftTimes,StreetAddress,City,State,Zipcode,Note")] Job job, 
                                   [Bind(Include = "Person, StartDate,EndDate")] Schedule schedule, String Foremen,
                                   [Bind(Include = "ShiftTimeId, Default, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday")] ShiftTime shiftTime, string ShiftTimeDefault,
                                   string SelectWeekDay1, string SelectWeekDay2, string SelectWeekDay3, string SelectWeekDay4, string SelectWeekDay5, 
                                   string SelectShiftTime1, string SelectShiftTime2, string SelectShiftTime3, string SelectShiftTime4, string SelectShiftTime5)
        {

            if (ModelState.IsValid)
            {                      
                if (db.Jobs.Any(j => j.JobTitle.Equals(job.JobTitle)))
                {
                    ModelState.AddModelError("JobTitle", "This job title is currently being used, please change the title," +
                                             " or delete/modify the job titled " + "\"" + job.JobTitle + "\".");
                    return View(job);
                }

                if (db.Jobs.Any(j => j.JobNumber.Equals(job.JobNumber)))
                {
                    ModelState.AddModelError("JobNumber", "This job number is currently being used, please change the number," +
                                             " or delete/modify the job numbered " + "\"" + job.JobNumber + "\".");
                    return View(job);
                }

                //Checking if the weekdays repeat
                HashSet<string> selectWD = new HashSet<string>();
                if (!string.IsNullOrEmpty(SelectWeekDay1) && !selectWD.Add(SelectWeekDay1)
                    || !string.IsNullOrEmpty(SelectWeekDay2) && !selectWD.Add(SelectWeekDay2)
                    || !string.IsNullOrEmpty(SelectWeekDay3) && !selectWD.Add(SelectWeekDay3)
                    || !string.IsNullOrEmpty(SelectWeekDay4) && !selectWD.Add(SelectWeekDay4)
                    || !string.IsNullOrEmpty(SelectWeekDay5) && !selectWD.Add(SelectWeekDay5)
                    )
                {
                    ViewBag.ErrorMessage = "Please select any Weekday only once";
                    return View(job);
                }

                else
                {
                    
                    shiftTime.ShiftTimeId = Guid.NewGuid();//Creates ShiftTimeID for a job
                    shiftTime.Default = ShiftTimeDefault;
                 
                    RecordShiftTime(SelectWeekDay1, SelectShiftTime1);
                    RecordShiftTime(SelectWeekDay2, SelectShiftTime2);
                    RecordShiftTime(SelectWeekDay3, SelectShiftTime3);
                    RecordShiftTime(SelectWeekDay4, SelectShiftTime4);
                    RecordShiftTime(SelectWeekDay5, SelectShiftTime5);

                    db.ShiftTimes.Add(shiftTime);
                    db.SaveChanges();
                    job.JobId = Guid.NewGuid();
                    job.ShiftTimes = db.ShiftTimes.Where(j => j.ShiftTimeId == shiftTime.ShiftTimeId).First(); 
                    db.Jobs.Add(job);
                    db.SaveChanges();

                    //Selects a correct collumn in ShiftTimes Table to record a shifttime
                    ShiftTime RecordShiftTime(string day, string time)
                    {
                       if (day != null)
                        {
                            if (day == "Monday") { shiftTime.Monday = time; }
                            if (day == "Tuesday") { shiftTime.Tuesday = time; }
                            if (day == "Wednesday") { shiftTime.Wednesday = time; }
                            if (day == "Thursday") { shiftTime.Thursday = time; }
                            if (day == "Friday") { shiftTime.Friday = time; }
                            if (day == "Saturday") { shiftTime.Saturday = time; }
                            if (day == "Sunday") { shiftTime.Sunday = time; }
                       }
                        return shiftTime;
                    }

                    if (schedule != null)
                    {
                        ApplicationUser foreman = db.Users.Where(f => f.Id == Foremen).FirstOrDefault();
                        schedule.Job = db.Jobs.Where(j => j.JobId == job.JobId).FirstOrDefault();
                        schedule.Person = foreman;
                        schedule.ScheduleId = Guid.NewGuid();
                        db.Schedules.Add(schedule);
                      //db.SaveChanges(); Commented as had an error occurs  during job creation due to shedule always exists even if the form Foreman is not populated.-NZ
                    }

                    return RedirectToAction("Index");
                }
            }
            return View(job);
        }




        // GET: Jobs/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }

            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "JobId,JobTitle,JobNumber,ShiftTimes,StreetAddress,City,State,Zipcode,Note")] Job job,
                                 [Bind(Include = "ShiftTimeId, Default, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday")] ShiftTime shiftTime)
                             
        {
            if (ModelState.IsValid)
            {   
                //Checking if more than 5 days have been selected
                List<string> weekDays = new List<string> { job.ShiftTimes.Monday, job.ShiftTimes.Tuesday, job.ShiftTimes.Wednesday,
                                        job.ShiftTimes.Thursday, job.ShiftTimes.Friday, job.ShiftTimes.Saturday, job.ShiftTimes.Sunday};
                List<string> countDays = new List<string>();
                foreach (string d in weekDays)
                {
                    if (d != null)
                    {
                        countDays.Add(d);
                    }
                }
                if (countDays.Count() > 5)
                {
                    ViewBag.ErrorMessage = "Please select no more than 5 Weekdays ";
                    return View(job);
                }
                //Update the DB
                db.Entry(job).State = EntityState.Modified;
                db.Entry(job.ShiftTimes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(job);
        }

        // GET: Jobs/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                Job job = db.Jobs.Find(id);
                db.Jobs.Remove(job);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["shortMessage"] = "This job has people scheduled on it and therefore cannot be deleted.";
                return RedirectToAction("Index", "Jobs");
            }
        }


        private IEnumerable<SelectListItem> GetForemen(string selectedForemanId = null)  // Added selectedForemenId that is set to null to be passed in when a default field value is needed
        {
            // Create a list of Foreman to pass to the viewbag foreman
            // The DropDownList will display the Foremen list and
            // The POST data receives the corresponding ForemenId

            return db.Users.Where(f => f.WorkType == Enums.WorkType.Foreman).OrderBy(g => g.UserName).Select(h => new SelectListItem()
            {
                Value = h.Id,
                Text = h.FName + " " + h.LName,
                Selected = selectedForemanId == h.Id // Add Select variable that holds the Id of the current selected item that gets passed when the corresponding edit button is clicked
            });
        }
        
        public ActionResult CreateJobsPartialView()
        {
            ViewBag.foremen = GetForemen();

            return PartialView("CreateJobsPartialView");
        }
        

        //Method to check if Job contains any non-defult shiftime
        [HttpPost]
        public bool HasNonDefaultShiftTime()
        {
            //Note: Uncomment the JQuery Ajax caller at bottom of site.js to test

            //Test 1: This job should return true and modify defaults
            var testJob = (from j in db.Jobs where j.JobNumber == "375" select j).FirstOrDefault();

            //Test 2: This job should return false  
            //var testJob = (from j in db.Jobs where j.JobNumber == "425" select j).FirstOrDefault();

            var shiftTime = (from s in db.ShiftTimes where s.ShiftTimeId == testJob.ShiftTimes.ShiftTimeId select s).FirstOrDefault();
            bool hasNonDefault = false;           
            //set all default shiftTimes to null
            if (shiftTime.Monday == shiftTime.Default)
            {
                shiftTime.Monday = null;
            }
            if (shiftTime.Tuesday == shiftTime.Default)
            {
                shiftTime.Tuesday = null;
            }
            if (shiftTime.Wednesday == shiftTime.Default)
            {
                shiftTime.Wednesday = null;
            }
            if (shiftTime.Thursday == shiftTime.Default)
            {
                shiftTime.Thursday = null;
            }
            if (shiftTime.Friday == shiftTime.Default)
            {
                shiftTime.Friday = null;
            }
            if (shiftTime.Saturday == shiftTime.Default)
            {
                shiftTime.Saturday = null;
            }
            if (shiftTime.Sunday == shiftTime.Default)
            {
                shiftTime.Sunday = null;
            }
            db.SaveChanges();
            
            //Check to see if schedule has any non-default shift times
            if (shiftTime.Monday != null || shiftTime.Tuesday != null || shiftTime.Wednesday != null || shiftTime.Thursday != null ||
               shiftTime.Friday != null || shiftTime.Saturday != null || shiftTime.Sunday != null)
            {
                hasNonDefault = true;
            }

            return hasNonDefault;
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



    }
}
