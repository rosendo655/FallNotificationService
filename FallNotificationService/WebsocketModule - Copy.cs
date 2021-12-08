using EmbedIO.WebSockets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FallNotificationService
{
    public class DataReceiverWebsocket2 : WebSocketModule
    {
        public static event EventHandler<string> OnStringReceived;

        private static IWebSocketContext clientContext;


        public DataReceiverWebsocket2(string urlPath)
            : base(urlPath, true)
        {
            // placeholder
        }

        public static void SendAll(string text)
        {
            
        }

        /// <inheritdoc />
        protected override async Task OnMessageReceivedAsync(
            IWebSocketContext context,
            byte[] rxBuffer,
            IWebSocketReceiveResult rxResult)
            => OnStringReceived?.Invoke(this, Encoding.GetString(rxBuffer));

        /// <inheritdoc />
        protected override async Task OnClientConnectedAsync(IWebSocketContext context)
        {
            clientContext = context;
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
