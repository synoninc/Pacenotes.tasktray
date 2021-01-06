using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PacenotesTaskTray
{
    [JsonObject("Setting")]
    class ClassSetting
    {
        [JsonProperty("Interval")]
        public int Interval { get; set; }

        [JsonProperty("Target Directory")]
        public string Target { get; set; }

        [JsonProperty("Login URL")]
        public string LoginUrl { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("Notification URL")]
        public string NotificationUrl { get; set; }
    }

}
