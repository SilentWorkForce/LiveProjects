using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementPortal.Models;

namespace ManagementPortal.Controllers
{
    public class JobsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Jobs
        public ActionResult Index()
        {
            return View(db.Jobs.ToList());
        }

        // GET: Jobs/Details/5
        public ActionResult Details(string id)
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

        // GET: Jobs/Create
        public ActionResult Create()
        {
            var states = GetAllStates();
            var model = new Job();
            model.States = GetSelectListItems(states);
            return View(model);
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,PasswordChar,FirstName,LastName,WorkType,PhoneNumber,UserRole,State,County,ZipCode,Suspended")] Job job)
        {
            var states = GetAllStates();
            job.States = GetSelectListItems(states);

            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(job);
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            var states = GetAllStates();
            job.States = GetSelectListItems(states);
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
        public ActionResult Edit([Bind(Include = "Id,Email,PasswordHash,FirstName,LastName,WorkType,PhoneNumber,UserRole,State,County,ZipCode,Suspended")] Job job)
        {
            var states = GetAllStates();
            job.States = GetSelectListItems(states);
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(job);
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(string id)
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
        public ActionResult DeleteConfirmed(string id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // List of all the states 
        private IEnumerable<string> GetAllStates()
        {
            return new List<string>
            {
                {"Alaska"},
                {"Alabama"},
                {"Arkansas"},
                {"Arizona"},
                {"California"},
                {"Colorado"},
                {"Connecticut"},
                {"District Of Columbia"},
                {"Delaware"},
                {"Florida"},
                {"Georgia"},
                {"Hawaii"},
                {"Iowa"},
                {"Idaho"},
                {"Illinois"},
                {"Indiana"},
                {"Kansas"},
                {"Kentucky"},
                {"Louisiana"},
                {"Massachusetts"},
                {"Maryland"},
                {"Maine"},
                {"Michigan"},
                {"Minnesota"},
                {"Missouri"},
                {"Mississippi"},
                {"Montana"},
                {"North Carolina"},
                {"North Dakota"},
                {"Nebraska"},
                {"New Hampshire"},
                {"New Jersey"},
                {"New Mexico"},
                {"Nevada"},
                {"New York"},
                {"Ohio"},
                {"Oklahoma"},
                {"Oregon"},
                {"Pennsylvania"},
                {"Rhode Island"},
                {"South Carolina"},
                {"South Dakota"},
                {"Tennessee"},
                {"Texas"},
                {"Utah"},
                {"Virginia"},
                {"Vermont"},
                {"Washington"},
                {"Wisconsin"},
                {"West Virginia"},
                {"Wyoming"}
            };
        }

        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            var selectList = new List<SelectListItem>();
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem { Value = element, Text = element });
            }
            return selectList;
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
