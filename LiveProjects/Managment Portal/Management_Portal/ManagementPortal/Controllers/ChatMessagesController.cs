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
    public class ChatMessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ChatMessages
        public ActionResult Index()
        {
            return View(db.ChatMessages.ToList());
        }

        // GET: ChatMessages/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatMessage chatMessage = db.ChatMessages.Find(id);
            if (chatMessage == null)
            {
                return HttpNotFound();
            }
            return View(chatMessage);
        }

        // GET: ChatMessages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChatMessages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChatMessageId,Sender,Date,Message")] ChatMessage chatMessage)
        {
            if (ModelState.IsValid)
            {
                chatMessage.ChatMessageId = Guid.NewGuid();
                db.ChatMessages.Add(chatMessage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chatMessage);
        }

        // GET: ChatMessages/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatMessage chatMessage = db.ChatMessages.Find(id);
            if (chatMessage == null)
            {
                return HttpNotFound();
            }
            return View(chatMessage);
        }

        // POST: ChatMessages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChatMessageId,Sender,Date,Message")] ChatMessage chatMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chatMessage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chatMessage);
        }

        // GET: ChatMessages/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatMessage chatMessage = db.ChatMessages.Find(id);
            if (chatMessage == null)
            {
                return HttpNotFound();
            }
            return View(chatMessage);
        }

        // POST: ChatMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ChatMessage chatMessage = db.ChatMessages.Find(id);
            db.ChatMessages.Remove(chatMessage);
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
