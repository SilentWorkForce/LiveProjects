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
    public class ShiftTimesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ShiftTimes
        public ActionResult Index()
        {
            return View(db.ShiftTime.ToList());
        }

        // GET: ShiftTimes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShiftTime shiftTime = db.ShiftTime.Find(id);
            if (shiftTime == null)
            {
                return HttpNotFound();
            }
            return View(shiftTime);
        }

        // GET: ShiftTimes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShiftTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShiftTimeId,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday,Default")] ShiftTime shiftTime)
        {
            if (ModelState.IsValid)
            {
                shiftTime.ShiftTimeId = Guid.NewGuid();
                db.ShiftTime.Add(shiftTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shiftTime);
        }

        // GET: ShiftTimes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShiftTime shiftTime = db.ShiftTime.Find(id);
            if (shiftTime == null)
            {
                return HttpNotFound();
            }
            return View(shiftTime);
        }

        // POST: ShiftTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShiftTimeId,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday,Default")] ShiftTime shiftTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shiftTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shiftTime);
        }

        // GET: ShiftTimes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShiftTime shiftTime = db.ShiftTime.Find(id);
            if (shiftTime == null)
            {
                return HttpNotFound();
            }
            return View(shiftTime);
        }

        // POST: ShiftTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ShiftTime shiftTime = db.ShiftTime.Find(id);
            db.ShiftTime.Remove(shiftTime);
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
