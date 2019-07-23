using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConstructionNew.Models;
using Microsoft.Ajax.Utilities;

namespace ConstructionNew.Controllers
{
    public class CreateUserRequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CreateUserRequests
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.CreateUserRequests.ToList());
        }

        // GET: CreateUserRequests/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(Guid? id)
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

        // GET: CreateUserRequests/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            // Send a list of user roles in a GET request
            ViewBag.Name = new SelectList(db.Roles.ToList(), "Name", "Name", "Employee");
            return View();
        }

        // POST: CreateUserRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "UserCreationRequestId,UserName,ConfirmationCode,UserRoles")] CreateUserRequest createUserRequest)
        {
            if (ModelState.IsValid)
            {

                // Check db for chosen username and return an error if it is already used.
                if (db.Users.Any(u => u.UserName == createUserRequest.UserName) || db.CreateUserRequests.Any(x => x.UserName == createUserRequest.UserName) )
                {
                    string usernameError = ("Username \"" + createUserRequest.UserName + "\" has already been used.");
                    ModelState.AddModelError("UserName", usernameError);               
                }
                // Add chosen username to createUserRequest if it is available
                else
                {
                    Random confirmation = new Random();
                    int confirmationNum = confirmation.Next(10000, 99999);

                    createUserRequest.UserCreationRequestId = Guid.NewGuid();
                    createUserRequest.ConfirmationCode = confirmationNum;
                    db.CreateUserRequests.Add(createUserRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
               
            }
            ViewBag.Name = new SelectList(db.Roles
                //.Where(u => !u.Name.Contains("Admin"))
                .ToList(), "Name", "Name");
            return View(createUserRequest);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AllUsers()
        {
            ViewBag.Message = "";

            var registeredUserList = new List<SelectListItem> { new SelectListItem { Selected = true, Text = @"All", Value = "", } };
            using (db)
            {
                db.Users.ForEach(u => registeredUserList.Add(new SelectListItem
                {
                    Text = String.Format("{0}, {1}", u.LName, u.FName),
                    Value = u.Id
                }));

                ViewBag.RegisteredUserList = registeredUserList;
                return View(db.CreateUserRequests.ToList());
            }
        }

        // GET: CreateUserRequests/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Guid? id)
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

        // POST: CreateUserRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "UserCreationRequestId,UserName,ConfirmationCode")] CreateUserRequest createUserRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(createUserRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(createUserRequest);
        }

        // GET: CreateUserRequests/Delete/5
        [Authorize(Roles = "Admin")]
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

        // POST: CreateUserRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
