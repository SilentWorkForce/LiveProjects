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
    public class CompanyNewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CompanyNews
        public ActionResult Index()
        {
            return View(db.CompanyNews.ToList());
        }

        // GET: CompanyNews/Details/5
        public ActionResult Details(int id)
        {
            if (id < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyNews companyNews = db.CompanyNews.Find(id);
            if (companyNews == null)
            {
                return HttpNotFound();
            }
            return View(companyNews);
        }

        // GET: CompanyNews/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompanyNews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NewsId,DateStamp,Title,NewsItem,ExpirationDate")] CompanyNews companyNews)
        {  // Made a message popup for when a user tries to input an invalid expiration date
            if (companyNews.ExpirationDate <= Convert.ToDateTime(DateTime.Now))
            {
                Response.Write(@"<SCRIPT LANGUAGE=""JavaScript"">alert('" + "Please enter a valid expiration date." + "')</SCRIPT>");
                return View(companyNews);   
            }
           

            if (ModelState.IsValid)
            {
                db.CompanyNews.Add(companyNews);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(companyNews);
        }

        // GET: CompanyNews/Edit/5
        public ActionResult Edit(int id)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyNews companyNews = db.CompanyNews.Find(id);
            if (companyNews == null)
            {
                return HttpNotFound();
            }
            return View(companyNews);
        }

        // POST: CompanyNews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NewsId,DateStamp,Title,NewsItem,ExpirationDate")] CompanyNews companyNews)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyNews).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(companyNews);
        }

        // GET: CompanyNews/Delete/5
        public ActionResult Delete(int id)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyNews companyNews = db.CompanyNews.Find(id);
            if (companyNews == null)
            {
                return HttpNotFound();
            }
            return View(companyNews);
        }

        // POST: CompanyNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompanyNews companyNews = db.CompanyNews.Find(id);
            db.CompanyNews.Remove(companyNews);
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
