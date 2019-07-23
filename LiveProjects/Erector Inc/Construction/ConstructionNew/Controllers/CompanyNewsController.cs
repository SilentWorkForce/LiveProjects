using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConstructionNew.Models;
using System.Web.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace ConstructionNew.Controllers
{

    public class CompanyNewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CompanyNews
        [Authorize] //used to restrict view of unregistered users
        public ActionResult Index()
        {
            return View(db.CompanyNews.ToList());
        }

        // Testing for work item 4341: Display Company News items with 
        // Expiration dates that have not yet occurred, or are NULL.
        public ActionResult UnexpiredNews()
        {
            var data = db.CompanyNews
                .Where(x => !x.ExpirationDate.HasValue
                || DbFunctions.DiffHours(x.ExpirationDate, DateTime.Now) <= 0);

            return View(data.ToList());
        }

        // GET: CompanyNews/Details/5
        [Authorize] //used to restrict view of unregistered users
        public ActionResult Details(string id)
        {
            if (id == null)
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
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => AutoDeleteNewsAsync(cancellationToken));
            return View();
        }

        // POST: CompanyNews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Title,NewsItem,ExpirationDate")] CompanyNews companyNews)
        {
            if (ModelState.IsValid)
            {
                companyNews.DateStamp = DateTime.Now.Ticks.ToString();
                db.CompanyNews.Add(companyNews);
                db.SaveChanges();
                return RedirectToAction("Index", "Dashboard");
            }

            return View(companyNews);
        }

        // GET: CompanyNews/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "DateStamp,Title,NewsItem,ExpirationDate")] CompanyNews companyNews)
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
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
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
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            CompanyNews companyNews = db.CompanyNews.Find(id);
            db.CompanyNews.Remove(companyNews);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult _CompanyNews()
        {
            return PartialView(db.CompanyNews.AsEnumerable());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task AutoDeleteNewsAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => {
                List<CompanyNews> newsToDelete = db.CompanyNews.Where(x => DbFunctions.DiffDays(x.ExpirationDate, DateTime.Now) > 90).ToList();
                //calculate the difference between the expiration date of the news and the current date, if it is more than 90 days or 3 months, it gets deleted.
                if (newsToDelete != null)
                {
                    foreach (var news in newsToDelete)
                    {
                        var companyNews = db.CompanyNews.Find(news.DateStamp);
                        db.CompanyNews.Remove(companyNews);
                        db.SaveChanges();
                    }
                }
            }, cancellationToken);
        }
    }
}
