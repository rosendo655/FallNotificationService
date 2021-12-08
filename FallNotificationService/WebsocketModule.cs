using EmbedIO.WebSockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FallNotificationService
{
    public class DataReceiverWebsocket : WebSocketModule
    {
        private static DateTime lastAlert = DateTime.Now;

        private Func<WebSocketModule, IWebSocketContext, string, Task> DoOnReceived;

        private static DataReceiverWebsocket senderInstance;
        public static DataReceiverWebsocket SenderInstance(string url) =>
            senderInstance = senderInstance ?? new DataReceiverWebsocket(url,
                async (ws,cntx,payload) =>
                {

                }
                );

        private static DataReceiverWebsocket receiverInstance;
        public static DataReceiverWebsocket ReceiverInstance(string url) =>
            receiverInstance = receiverInstance ?? new DataReceiverWebsocket(url,
                async (ws, cntx, payload) =>
                {
                    var received = JsonConvert.DeserializeObject<JObject>(payload);
                    var fall = received.ContainsKey("fall");
                    DateTime curTime = DateTime.Now;
                    if(fall && (curTime - lastAlert > TimeSpan.FromSeconds(15)))
                    {
                        await InMessageController.SendToAll("Ha ocurrido una caida");
                        lastAlert = DateTime.Now;
                    }

                    await senderInstance.SendAllAsync(payload);
                }
                );


        public DataReceiverWebsocket(string urlPath)
            : base(urlPath, true)
        {
            // placeholder
        }

        public DataReceiverWebsocket(string url, Func<WebSocketModule,IWebSocketContext, string, Task> onStringReceived):this(url)
        {
            DoOnReceived = onStringReceived;
        }

        public  Task SendAllAsync(string str)
        {
            return this.BroadcastAsync(str);
        }

        /// <inheritdoc />
        protected override async Task OnMessageReceivedAsync(
            IWebSocketContext context,
            byte[] rxBuffer,
            IWebSocketReceiveResult rxResult)
            => await DoOnReceived(this,context,Encoding.GetString(rxBuffer));

        /// <inheritdoc />
        protected override async Task OnClientConnectedAsync(IWebSocketContext context)
        {

        }

        /// <inheritdoc />
        protected override async Task OnClientDisconnectedAsync(IWebSocketContext context)
        {

        }

        private async Task SendToOthersAsync(IWebSocketContext context, string payload)
        {

        }
    }
}
