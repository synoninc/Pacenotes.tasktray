using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PacenotesTaskTray
{
    /// <summary>
    /// 環境ファイル項目の定義
    /// </summary>
    [JsonObject("Setting")]
    class ClassSetting
    {
        [JsonProperty("Interval")]
        public int Interval { get; set; }

        [JsonProperty("Target Directory")]
        public string Target { get; set; }

        [JsonProperty("Target Directory2")]
        public string Target2 { get; set; }

        [JsonProperty("Target Directory3")]
        public string Target3 { get; set; }

        [JsonProperty("Login URL")]
        public string LoginUrl { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("Notification URL")]
        public string NotificationUrl { get; set; }

        [JsonProperty("Command")]
        public string Command { get; set; }

        [JsonProperty("Args")]
        public string Args { get; set; }


        [JsonProperty("IntervalA")]
        public int IntervalA { get; set; }

        [JsonProperty("TargetA Directory")]
        public string TargetA { get; set; }

        [JsonProperty("TargetA Directory2")]
        public string Target2A { get; set; }

        [JsonProperty("TargetA Directory3")]
        public string Target3A { get; set; }

        [JsonProperty("CacheA Directory")]
        public string CacheA { get; set; }

        [JsonProperty("CacheA Directory2")]
        public string Cache2A { get; set; }

        [JsonProperty("CacheA Directory3")]
        public string Cache3A { get; set; }

        [JsonProperty("OutputA Directory")]
        public string OutputA { get; set; }

        [JsonProperty("OutputA Directory2")]
        public string Output2A { get; set; }

        [JsonProperty("OutputA Directory3")]
        public string Output3A { get; set; }

        [JsonProperty("ExecA")]
        public string ExecA { get; set; }

        [JsonProperty("Exec2A")]
        public string Exec2A { get; set; }

        [JsonProperty("Exec3A")]
        public string Exec3A { get; set; }
    }

}
