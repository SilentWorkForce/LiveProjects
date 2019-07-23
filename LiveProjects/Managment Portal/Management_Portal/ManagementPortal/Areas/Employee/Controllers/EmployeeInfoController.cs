using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementPortal.Areas.Employee.Models;
using ManagementPortal.Models;

namespace ManagementPortal.Areas.Employee.Controllers
{
    public class EmployeeInfoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employee/EmployeeInfoes
        public ActionResult Index()
        {
            return View(db.Employees.ToList());
        }

        // GET: Employee/EmployeeInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeInfo employeeInfo = db.Employees.Find(id);
            if (employeeInfo == null)
            {
                return HttpNotFound();
            }
            return View(employeeInfo);
        }

        // GET: Employee/EmployeeInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/EmployeeInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Phone,Email,Address")] EmployeeInfo employeeInfo)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employeeInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employeeInfo);
        }

        // GET: Employee/EmployeeInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeInfo employeeInfo = db.Employees.Find(id);
            if (employeeInfo == null)
            {
                return HttpNotFound();
            }
            return View(employeeInfo);
        }

        // POST: Employee/EmployeeInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Phone,Email,Address")] EmployeeInfo employeeInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employeeInfo);
        }

        // GET: Employee/EmployeeInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeInfo employeeInfo = db.Employees.Find(id);
            if (employeeInfo == null)
            {
                return HttpNotFound();
            }
            return View(employeeInfo);
        }

        // POST: Employee/EmployeeInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeInfo employeeInfo = db.Employees.Find(id);
            db.Employees.Remove(employeeInfo);
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
