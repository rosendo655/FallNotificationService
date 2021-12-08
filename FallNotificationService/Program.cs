using EmbedIO;
using EmbedIO.WebApi;
using Swan.Logging;
using System;
using System.Threading.Tasks;

namespace FallNotificationService
{
    class Program
    {
        static async Task Main(string[] args)
        {

            CreateServer();

            await TelegramBotManager.UpdateWebHook($"{ConfigProvider.PublicUrl}/api/message");

            

            Console.ReadKey();
        }

        private static async void DataReceiverWebsocket_OnStringReceived(object sender, string e)
        {
            string message = $"Una caida ha ocurrido";
            bool fall = message.Contains("any");
            if (fall)
            {
                await InMessageController.SendToAll(message);
            }
        }

        static async void CreateServer()
        {

            // Our web server is disposable.
            using (var server = CreateWebServer())
            {
                // Once we've registered our modules and configured them, we call the RunAsync() method.
                await server.RunAsync();

                var browser = new System.Diagnostics.Process()
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo() { UseShellExecute = true }
                };
                //browser.Start();
                // Wait for any key to be pressed before disposing of our web server.
                // In a service, we'd manage the lifecycle of our web server using
                // something like a BackgroundWorker or a ManualResetEvent.
                while (true) ;
            }
        }

        private static WebServer CreateWebServer()
        {
            string[] prefixes = ConfigProvider.ServerUrls;
            var server = new WebServer(o => o
                    .WithUrlPrefixes(prefixes)
                    .WithMode(HttpListenerMode.EmbedIO))
                    .WithWebApi("/api", m => m
                        .WithController<InMessageController>()
                    )
                    .WithModule(DataReceiverWebsocket.SenderInstance("/ws2"))
                    .WithModule(DataReceiverWebsocket.ReceiverInstance("/ws")
                    
                    );
            // Listen for state changes.
            server.StateChanged += (s, e) => $"WebServer New State - {e.NewState}".Info();
            
            return server;
        }
    }
}
