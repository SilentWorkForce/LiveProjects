using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using ConstructionNew.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
namespace SignalRChat
{
    public class ChatHub : Hub //Hub base class provides methods that communicates with signalR connections
    {
        // Link to the website's DB, used for the ChatMessages table.
        ApplicationDbContext db = new ApplicationDbContext();


        // Submits a new chat message to the DB and updates all clients with it.
        public void Send(string name, string message) //This send method is called from Ajax
        {
            ChatMessage chat = new ChatMessage();

            // Fill in the chat model's basic details.
            chat.Message = message;
            chat.ChatMessageId = Guid.NewGuid();
            chat.Date = DateTime.Now;

            // Fill in the chat model's Sender
            string currentUserId = HttpContext.Current.User.Identity.GetUserId();              // Get ID of current user
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId); // Get name of current user using ID
            chat.Sender = currentUser.FName;                                                   // Assign 'Sender' (a string) the value of [FirstName] [LastInitial]
            chat.Sender += (currentUser.LName != null) ? (" " + currentUser.LName[0]) : "";    // And ignore that last name if the user does not have one.

            // Add the chat item to DB and save changes
            db.ChatMessages.Add(chat);
            db.SaveChanges();

            // Update all clients with the new chat
            PostNewMessageToClients(chat);

            // Force the calling client to look at the newest chats
            Clients.Caller.scrollToBottom();
        }

        // Grabs the entire chat discussion history from the DB and posts it to the user calling for it.
        // Does not affect any other ChatHub clients.
        public void GetMessages()
        {   
            // Clears the calling-client's chat of all content.
            Clients.Caller.refreshChat();

            // Gather and post messages to the chat dialog from the DB
            var messages = db.ChatMessages.ToList();  
            foreach (ChatMessage chatMessage in messages.OrderBy(d => d.Date))
            {
                PostNewMessageToCallingClient(chatMessage);
            }

            // Forces the calling-client to view the newest chats at the bottom of the chat-window element.
            Clients.Caller.scrollToBottom();

            // What to do with this... I feel like this should be somewhere else.
            // I think the server ought to have a periodic update() or clean() method, that's where this should go.
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => AutoDeleteChatMessagesAsync(cancellationToken));
        }

        // Edits a message in the DB, locating it by ID, then pushes this change to no clients.
        // Returns true when (if?) the change was successful.
        public void EditMessage(string messageId, string messageContent)
        {
            Guid guid;
            if (Guid.TryParse(messageId, out guid))
            {
                // Find the chat message and update its contents.
                var message = db.ChatMessages.Find(guid);
                message.Message = messageContent;

                // Inform the DB of changes and save them.
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
            }

            // Push this change to all clients
            //Clients.All.refreshChat();    // Happens too quickly, I think. Blanks the chat window instead of updating anything.
        }

        // Deletes a message from the DB by its ID, then pushes that update to nobody.
        // This method creates a one-item array and passes control over to the mains.
        public void DeleteMessage(string chatMessageStringId)
        {
            DeleteMessages(new string[] { chatMessageStringId });
        }

        // Deletes a list of messages from the DB by their IDs, then pushes that update to nobody.
        // The way it does this pushing is a little clunky, though, bear in mind.
        public void DeleteMessages(string[] chatMessageStringIds)
        {
            Guid guid = Guid.Empty;
            var chatMessageGuids = new List<Guid>();

            // Convert all string IDs passed to Guid's (keeps only the valid ones; invalids are ignored!)
            chatMessageGuids = (from strId in chatMessageStringIds
                                where Guid.TryParse(strId, out guid)
                                select guid).ToList();

            // Remove all identified chats from the DB
            foreach (Guid chatId in chatMessageGuids)
                db.ChatMessages.Remove(db.ChatMessages.Find(chatId));

            // Finalize changes
            db.SaveChanges();

            // Push to all clients
            //Clients.All.refreshChat();    // Happens too quickly, I think. Blanks the chat window instead of updating anything.
        }

        // This only needs to be called once a day. Currently, it's sitting in the GetMessages() method above. That means it's called a lot.
        // Cleans the DB of chat messages which are too old.
        private async Task AutoDeleteChatMessagesAsync(CancellationToken cancellationToken)
        {
            const int DaysToKeepChats = 7;

            await Task.Run(() => {
                // Build a list of chats that are too old to keep.
                List<ChatMessage> chatsToDelete = db.ChatMessages.Where(x => DbFunctions.DiffDays(x.Date, DateTime.Now) > DaysToKeepChats).ToList();

                // If this list isn't empty, delete each item from its corresponding row in the DB.
                if (chatsToDelete != null)
                {
                    foreach (var chat in chatsToDelete)
                    {
                        var chatMessage = db.ChatMessages.Find(chat.ChatMessageId);
                        db.ChatMessages.Remove(chatMessage);
                        db.SaveChanges();
                    }
                }
            }, cancellationToken);
        }

        // Posts a new chat message to all clients of the chat hub.
        private void PostNewMessageToClients(ChatMessage chatMessage)
        {
            string date, sender, message;
            FormatNewMessage(chatMessage, out date, out sender, out message);
            Clients.All.addNewMessageToPage(date, sender, message);
        }

        // Posts a new chat message to the client who invoked the server.
        // Currently, this is primarily used by the GetAllMessages() method.
        private void PostNewMessageToCallingClient(ChatMessage chatMessage)
        {
            string date, sender, message;
            FormatNewMessage(chatMessage, out date, out sender, out message);
            Clients.Caller.addNewMessageToPage(date, sender, message);
        }

        // Formats the input fields for a Javascript method call.
        // Currently, this only does anything to the date field.
        private void FormatNewMessage(ChatMessage chatMessage, out string date, out string sender, out string message)
        {
            date = $"{chatMessage.Date.ToString("h:mm tt")}";
            sender = $"{chatMessage.Sender}";
            message = $"{chatMessage.Message}";
        }
    }
}

