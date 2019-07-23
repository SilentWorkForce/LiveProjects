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
    public class ChatMessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ChatMessages
        [Authorize] //used to restrict view of unregistered users
        public ActionResult Index()
        {
            // Messages should appear in most-recent-first order
            List<ChatMessage> messages;
            messages = db.ChatMessages.OrderByDescending(msg => msg.Date).ToList();

            return View(messages);
        }

        // GET: ChatMessages/Details/5
        [Authorize] //used to restrict view of unregistered users
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

        // [TODO] It is probably safe to delete this method.
        // GET: ChatMessages/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // [TODO] Commenting this out seems to affect nothing. Is this even used?
        // We create the chat messages within the ChatHub, so this is not used
        // POST: ChatMessages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ChatMessageId,Date,Message")] ChatMessage chatMessage)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        chatMessage.ChatMessageId = Guid.NewGuid();
        //        db.ChatMessages.Add(chatMessage);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(chatMessage);
        //}

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
        public ActionResult Edit([Bind(Include = "ChatMessageId,Date,Message,Sender")] ChatMessage chatMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chatMessage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit");
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

        public ActionResult MobileChat()
        {
            return View();
        }

    }
}
