using System;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using theorbo.Services;

namespace theorbo.Config
{
    [DataContract]
    public class BotConfiguration
    {
        private static readonly string ConfigFilename;

        static BotConfiguration()
        {
            ConfigFilename = Path.Combine(PathService.AppData, "service-config.json");
        }

        [JsonProperty("telegram_bot_chat_timeout")]
        public TimeSpan TelegramBotChatTimeout { get; set; } = TimeSpan.FromMinutes(1);

        //Set cleartext data with cleartext: prefix, they would be replaced with protected text on load
        [ProtectedString]
        [JsonProperty("telegram_botkey")]
        public string TelegramAnnouncerBotKey { get; set; } = "cleartext:dummy_botkey";

        [JsonProperty("telegram_max_messages_per_sec")]
        public int TelegramMaxMessagesPerSecond { get; set; } = 15;

        public static BotConfiguration LoadOrCreate(bool saveIfNew = false)
        {
            if (!File.Exists(ConfigFilename))
            {
                var result = new BotConfiguration();

                if (saveIfNew)
                    result.Save();

                return result;
            }

            var existing = JsonConvert.DeserializeObject<BotConfiguration>(File.ReadAllText(ConfigFilename),
                JsonFormatters.IndentedAutotype);

            if (CurrentUserProtectedString.GenerateProtectedPropertiesFromCleartext(existing))
                existing.Save();

            return existing;
        }

        public void Save()
        {
            File.WriteAllText(ConfigFilename, JsonConvert.SerializeObject(this, JsonFormatters.IndentedAutotype));
        }
    }
}