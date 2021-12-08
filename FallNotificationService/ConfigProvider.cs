using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;

namespace FallNotificationService
{
    public class ConfigProvider
    {
        private static JObject config;
        private static JObject Config => config = config ?? JsonConvert.DeserializeObject<JObject>(ConfigString);
        private static string ConfigString =>File.ReadAllText("./config.json");



        public static string PublicUrl => Config["public_url"].ToObject<string>();
        public static string TelegramApiToken => Config["telegram_token"].ToObject<string>();
        public static string[] ServerUrls => Config["server_urls"].ToObject<string[]>();


    }
}
