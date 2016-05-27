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
using System.Net.Http.Headers;

namespace Tempe
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private static string VISION_CLIENTID = ConfigurationManager.AppSettings["OxfordClient"];
        private static string IMAGE_ENDPOINT = ConfigurationManager.AppSettings["ImageEndpoint"];
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                var latestImage = GetLatestImageUrl();
                if (string.IsNullOrEmpty(latestImage))
                {
                    return message.CreateReplyMessage("Not really sure to be completely honest...");
                }
                else
                {
                    VisionServiceClient client = new VisionServiceClient(VISION_CLIENTID);
                    var r = await client.DescribeAsync(latestImage);
                    var msg = string.Join(", ", r.Description.Captions.Select(c => c.Text));

                    // return our reply to the user
                    var reply = message.CreateReplyMessage($"Thanks for asking, I think I see the following: {msg}.");
                    reply.Attachments = new List<Attachment>();
                    reply.Attachments.Add(new Attachment()
                    {
                        ContentUrl = latestImage,
                        ContentType = "image/jpg"
                    });

                    return reply;
                }
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private string GetLatestImageUrl()
        {
            var url = "http://iluvesports.com/wp-content/uploads/2014/09/pro-skateboarders-of-all-time1.jpg";
            return url;
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