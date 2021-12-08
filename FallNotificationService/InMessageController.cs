using EmbedIO.Routing;
using EmbedIO.WebApi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallNotificationService
{
    public class InMessageController : WebApiController
    {
     
        private static Dictionary<int, ChatSession> sessions = new Dictionary<int, ChatSession>();

        public InMessageController() : base()
        {

        }

        [Route(EmbedIO.HttpVerbs.Post, "/message")]
        public async Task<string> ProcessMesage([FormData] NameValueCollection content)
        {
            try
            {

                //return "ok";
                var postcontent = content.AllKeys;
                JObject obj = JObject.Parse(postcontent[0]);

                var chat_id = obj["message"]["chat"]["id"].ToObject<int>();
                var text = obj["message"]["text"].ToObject<string>();
                var userName = obj["message"]["from"]["username"].ToObject<string>();

                ChatSession cur_session = null;

                if (!sessions.TryGetValue(chat_id, out cur_session))
                {
                    sessions.Add(chat_id, new ChatSession(chat_id, userName));
                    cur_session = sessions[chat_id];
                }

                cur_session.AddMessage(text);

                await ChatSessionHandler.HandleSession(cur_session);

                return "ok";
            }
            catch (Exception)
            {

                return "ok";
            }
        }

        public static async Task SendToAll(string message)
        {
            foreach( var session in sessions.Where(w => w.Value.Status == ChatStatus.USER_EXISTS))
            {
                await TelegramBotManager.SendMessage(session.Key, message);
            }
        }

    }
}
