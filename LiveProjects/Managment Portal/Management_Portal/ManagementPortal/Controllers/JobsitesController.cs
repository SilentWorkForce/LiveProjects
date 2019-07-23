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
    public class JobsitesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Jobsites
        public ActionResult Index()
        {
            return View(db.Jobsites.ToList());
        }

        // GET: Jobsites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jobsite jobsite = db.Jobsites.Find(id);
            if (jobsite == null)
            {
                return HttpNotFound();
            }
            return View(jobsite);
        }

        // GET: Jobsites/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Jobsites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobSiteID,Address,Town,State,Zip")] Jobsite jobsite)
        {
            if (ModelState.IsValid)
            {
                db.Jobsites.Add(jobsite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobsite);
        }

        // GET: Jobsites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jobsite jobsite = db.Jobsites.Find(id);
            if (jobsite == null)
            {
                return HttpNotFound();
            }
            return View(jobsite);
        }

        // POST: Jobsites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobSiteID,Address,Town,State,Zip")] Jobsite jobsite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobsite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobsite);
        }

        // GET: Jobsites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jobsite jobsite = db.Jobsites.Find(id);
            if (jobsite == null)
            {
                return HttpNotFound();
            }
            return View(jobsite);
        }

        // POST: Jobsites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Jobsite jobsite = db.Jobsites.Find(id);
            db.Jobsites.Remove(jobsite);
            db.SaveChanges();
            return RedirectToAction("Index");
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
