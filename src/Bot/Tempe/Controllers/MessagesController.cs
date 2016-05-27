using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.ProjectOxford.Vision;
using System.Configuration;

namespace Tempe
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private static string VISION_CLIENTID = ConfigurationManager.AppSettings["OxfordClient"];
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                Trace.TraceInformation($"Message: {message.SourceText}");
                // calculate something for us to return
                int length = (message.Text ?? string.Empty).Length;

                //VisionServiceClient client = new VisionServiceClient(VISION_CLIENTID);
                //var r = await client.DescribeAsync("http://sites.psu.edu/siowfa15/wp-content/uploads/sites/29639/2015/10/cat.jpg");
                //var msg = string.Join(", ", r.Description.Captions.Select(c => c.Text));
                var msg = "this thing and that";

                // return our reply to the user
                var reply = message.CreateReplyMessage($"Thanks for asking, I think I see {msg}.");
                reply.Attachments = new List<Attachment>();
                reply.Attachments.Add(new Attachment()
                {
                    ContentUrl = "http://sites.psu.edu/siowfa15/wp-content/uploads/sites/29639/2015/10/cat.jpg",
                    ContentType = "image/jpg"
                });

                return reply;
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == "BotAddedToConversation")
            {
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
            }
            else if (message.Type == "UserAddedToConversation")
            {
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }
    }
}