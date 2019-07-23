using System;                                                                   //This class represents the server side functions for SignalR (instant messaging)
using System.Collections.Generic;                                               //NOTE: Functions are named in this c# class in normal PascalCase naming convention
using System.Linq;                                                              //And are called using camelCase from javascript on the Views pages. This is a 
using System.Web;                                                               //functionality of SignalR. -PC/Note
using Microsoft.AspNet.SignalR;

namespace ManagementPortal.Models
{                                                                               //installed Install-Package Microsoft.AspNet.SignalR -Version 2.4.1
    public class ChatHub : Hub
    {

        public void SendMessage(string name, string message)
        {
            using (var context = new ApplicationDbContext())                    //Opens connection to database
            {
                var chatMessage = new ChatMessage();                            //Chat message object to be populated and passed to table
                chatMessage.Date = DateTime.Now;
                chatMessage.ChatMessageId = Guid.NewGuid();         
                chatMessage.Sender = name;
                chatMessage.Message = message;

                context.ChatMessages.Add(chatMessage);                          //Adding chat message to the chat message table, then closes connection
                context.SaveChanges();
            }
            DateTime dt = DateTime.Now;
            string timeString = dt.DayOfWeek + " " +                            //String for javascript side display.
                dt.ToShortTimeString() + " " + dt.ToShortDateString();

            Clients.All.addNewMessageToPage(name, message, timeString);         //This calls a message on all clients javascript function called addNewMessage.
        }
    }
}