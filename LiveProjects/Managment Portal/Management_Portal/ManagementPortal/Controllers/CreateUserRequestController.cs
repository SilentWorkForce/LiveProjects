using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ManagementPortal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ManagementPortal.Controllers
{
    public class CreateUserRequestController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CreateUserRequest
        public ActionResult Index()
        {
            return View(db.CreateUserRequests.ToList());
        }

        // GET: CreateUserRequest/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CreateUserRequest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserCreationRequestId,UserName,ConfirmationCode")] CreateUserRequest createUserRequest)
        {
            if (ModelState.IsValid)
            {
                createUserRequest.UserCreationRequestId = Guid.NewGuid();
                createUserRequest.ConfirmationCode = Random(100000,1000000);
                db.CreateUserRequests.Add(createUserRequest);
                db.SaveChanges();
                return RedirectToAction("ConfirmationCode", new { id = createUserRequest.UserCreationRequestId });
            }
            return View(createUserRequest);
        }

        // Creates a ramdomly generated code for the confirmation code
        private int Random(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // GET: CreateUserRequest/ConfirmationCode
        public ActionResult ConfirmationCode(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreateUserRequest createUserRequest = db.CreateUserRequests.Find(id);
            if (createUserRequest == null)
            {
                return HttpNotFound();
            }
            return View(createUserRequest);
        }

        // GET: CreateUserRequest/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreateUserRequest createUserRequest = db.CreateUserRequests.Find(id);
            if (createUserRequest == null)
            {
                return HttpNotFound();
            }
            return View(createUserRequest);
        }

        // POST: CreateUserRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CreateUserRequest createUserRequest = db.CreateUserRequests.Find(id);
            db.CreateUserRequests.Remove(createUserRequest);
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
