using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Timers;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace PacenotesTaskTray
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {

            // NLogの初期化
            InitializeLogger();

            // メニューの初期化
            ResidentMenu rm = new ResidentMenu();
            Application.Run();

            /*
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form1());
            */
        }

        /// <summary>
        /// NLogのの初期化処理です。
        /// </summary>
        private static void InitializeLogger()
        {
            var conf = new LoggingConfiguration();

            // ファイル形式、shift-jisコードで/log/execute_yyyyMMdd.log型式で7つ(一週間)保存する
            var file = new FileTarget("file");
            file.Encoding = System.Text.Encoding.GetEncoding("shift-jis");
            file.Layout = "${longdate} [${threadid:padding=2}] [${uppercase:${level:padding=-5}}] ${callsite}() - ${message}${exception:format=ToString}";
            file.FileName = "${basedir}/logs/execute_${date:format=yyyyMMdd}.log";
            file.ArchiveNumbering = ArchiveNumberingMode.Date;
            file.ArchiveFileName = "${basedir}/logs/execute.log.{#}";
            file.ArchiveEvery = FileArchivePeriod.None;
            file.MaxArchiveFiles = 7;
            conf.AddTarget(file);
            conf.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, file));

            var eventlog = new EventLogTarget("eventlog");
            eventlog.Layout = "${message}${newline}${exception:format=ToString}";
            eventlog.Source = "NLogNoConfig";
            eventlog.Log = "Application";
            eventlog.EventId = "1001";
            conf.AddTarget(eventlog);
            conf.LoggingRules.Add(new LoggingRule("*", LogLevel.Error, eventlog));

            LogManager.Configuration = conf;
        }

    }

    /// <summary>
    /// メニューの処理です。
    /// </summary>
    class ResidentMenu : Form
    {

        private int interval = 60;
        private string target = "";
        private string login = "";
        private string username = "";
        private string password = "";
        private string notification = "";
        private string command = "";
        private string args = "";

        private Logger Exe_logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 初期化処理
        /// </summary>
        public ResidentMenu()
        {
            this.ShowInTaskbar = false;
            this.setMenu();

            this.readSetting();
            this.setTimer();
        }

        /// <summary>
        /// cookieの保存
        /// </summary>
        private static System.Net.CookieContainer cContainer =
            new System.Net.CookieContainer();

        private static Boolean RemoteCertificateValidationCallback(Object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }
            else
            {
                return true;
/*
                //SslPolicyErrors列挙体には、Flags属性があるので、
                //エラーの原因が複数含まれているかもしれない。
                //そのため、&演算子で１つ１つエラーの原因を検出する。
                if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) ==
                    SslPolicyErrors.RemoteCertificateChainErrors)
                {
                    Console.WriteLine("ChainStatusが、空でない配列を返しました");
                }

                if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) ==
                    SslPolicyErrors.RemoteCertificateNameMismatch)
                {
                    Console.WriteLine("証明書名が不一致です");
                }

                if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNotAvailable) ==
                    SslPolicyErrors.RemoteCertificateNotAvailable)
                {
                    Console.WriteLine("証明書が利用できません");
                }

                //検証失敗とする
                return false;
*/
            }
        }

        /// <summary>
        /// API呼び出し処理
        /// </summary>
        private String requestServer(String url, Hashtable ht)
        {
            try
            {
                string json = JsonConvert.SerializeObject(ht);
                byte[] data = Encoding.ASCII.GetBytes(json);

                if (System.Net.ServicePointManager.ServerCertificateValidationCallback == null)
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
                }

                // リクエストの作成
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/json";
                req.ContentLength = data.Length;
                req.Timeout = 10000;

                req.CookieContainer = new System.Net.CookieContainer();
                req.CookieContainer.Add(cContainer.GetCookies(req.RequestUri));

                // リクエストの送信
                Stream reqStream = req.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                WebResponse res = req.GetResponse();

                // cookie保存
                System.Net.CookieCollection cookies =
                    req.CookieContainer.GetCookies(req.RequestUri);
                cContainer.Add(cookies);

                // レスポンスの受信
                Stream resStream = res.GetResponseStream();
                StreamReader sr = new StreamReader(resStream, Encoding.UTF8);
                string result = sr.ReadToEnd();
                sr.Close();
                resStream.Close();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 監視タイマー処理
        /// </summary>
        private void setTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer(1000);

            int count = 0;
            int prevCount = -1;
            int targetCount = 0;
            if (timer.Enabled)
            {
                timer.Stop();
            }
            timer.Elapsed += (sender, e) =>
            {
                // インターバルで実行
                if (count >= interval)
                {
                    count = 0;

                    Exe_logger.Info("execute timer");
                    if (target != "")
                    {
                        // ターゲットフォルダーのフォルダー一覧の取得
                        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(target);
                        System.IO.DirectoryInfo[] subFolders =
                            di.GetDirectories("*", System.IO.SearchOption.AllDirectories);

                        // 新規フォルダーの確認
                        targetCount = subFolders.Length;
                        Console.WriteLine("targetCount = " + targetCount);
                        Console.WriteLine("prevCount = " + prevCount);
                        if (prevCount != -1 && prevCount + 1 <= targetCount)
                        {
                            Exe_logger.Info("target folder = " + subFolders[subFolders.Length - 1]);
                            Console.WriteLine("target folder = " + subFolders[subFolders.Length-1]);

                            // タイマーの起動
                            System.Timers.Timer timerSub = new System.Timers.Timer(1000);

                            int countSub = 0;
                            int prevCountSub = -1;
                            int targetCountSub = 0;
                            timerSub.Elapsed += (senderSub, eSub) =>
                            {
                                // インターバルで実行
                                if (countSub >= interval)
                                {
                                    countSub = 0;

                                    try
                                    {
                                        // pdfファイルの取得
                                        string[] files = Directory.GetFiles(target + "\\" + subFolders[subFolders.Length - 1], "*.pdf");

                                        Exe_logger.Info("execute sub timer (" + files.Length.ToString() + ")");

                                        // ファイル数の変化なしの確認
                                        targetCountSub = files.Length;
                                        Console.WriteLine("targetCountSub = " + targetCountSub);
                                        Console.WriteLine("prevCountSub = " + prevCountSub);
                                        if (prevCountSub != -1 && targetCountSub != 0 && prevCountSub == targetCountSub)
                                        {
                                            // APIのログイン処理
                                            Hashtable ht = new Hashtable();
                                            ht["username"] = this.username;
                                            ht["password"] = this.password;
                                            String result = requestServer(this.login, ht);
                                            JObject jResult = JObject.Parse(result);
                                            Console.WriteLine(jResult["result"]);
                                            if (jResult["result"].ToString() == "True")
                                            {
                                                Exe_logger.Info("login completed");
                                                Console.WriteLine("login completed");

                                                // APIのコマンド実行処理
                                                String argument = this.args;
                                                Hashtable htExe = new Hashtable();
                                                htExe["command"] = this.command;
                                                htExe["args"] = argument.Replace("%count%", files.Length.ToString());
                                                String resultExe = requestServer(this.notification, htExe);
                                                JObject jResultExe = JObject.Parse(resultExe);
                                                if (jResultExe["result"].ToString() == "True")
                                                {
                                                    string param = "";
                                                    foreach (string k in files)
                                                    {
                                                        param += String.Format("{0},", k);
                                                    }

                                                    Exe_logger.Info("execute completed (" + files.Length.ToString() + ") : " + param);
                                                    Console.WriteLine("execute completed (" + files.Length.ToString() + ") : " + param);
                                                }
                                                else
                                                {
                                                    Exe_logger.Error("execute erro : " + resultExe);
                                                }
                                            }
                                            else
                                            {
                                                Exe_logger.Error("login error : " + result);
                                            }

                                            timerSub.Stop();
                                            timer.Start();
                                        }
                                        if (prevCountSub != 0 && targetCountSub == 0)
                                        {
                                            Console.WriteLine("break sub");
                                            timerSub.Stop();
                                            timer.Start();
                                        }
                                        prevCountSub = targetCountSub;
                                    }
                                    catch (Exception ex)
                                    {
                                        Exe_logger.Error("exception error : " + ex);

                                        subFolders = di.GetDirectories("*", System.IO.SearchOption.AllDirectories);
                                    }
                                }
                                else
                                {
                                    countSub++;
                                }
                            };
                            timerSub.Start();
                            timer.Stop();
                        }
                        prevCount = targetCount;
                    }
                }
                else
                {
                    count++;
                }
            };
            timer.Start();
        }

        /// <summary>
        /// 環境ファイル読み込み処理
        /// </summary>
        private void readSetting()
        {
            ClassSetting setting = new ClassSetting();

            var filePath = @"setting.json";

            if (File.Exists(filePath))
            {
                using (var sr = new StreamReader(filePath, System.Text.Encoding.UTF8))
                {
                    var jsonData = sr.ReadToEnd();

                    setting = JsonConvert.DeserializeObject<ClassSetting>(jsonData);
                    this.interval = setting.Interval;
                    this.target= setting.Target;
                    this.login = setting.LoginUrl;
                    this.username = setting.Username;
                    this.password = setting.Password;
                    this.notification = setting.NotificationUrl;
                    this.command = setting.Command;
                    this.args = setting.Args;
                }
            }
        }

        /// <summary>
        /// 保存処理
        /// </summary>
        private void Setting_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();

            readSetting();
            this.setTimer();
        }

        /// <summary>
        /// 閉じる処理
        /// </summary>
        private void Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// メニュー作成処理
        /// </summary>
        private void setMenu()
        {
            NotifyIcon icon = new NotifyIcon();
            icon.Icon = new Icon("app.ico");
            icon.Visible = true;
            icon.Text = "Pacenotes";
            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem menuItem1 = new ToolStripMenuItem();
            menuItem1.Text = "&設定";
            menuItem1.Click += new EventHandler(Setting_Click);
            menu.Items.Add(menuItem1);

            ToolStripMenuItem menuItem2 = new ToolStripMenuItem();
            menuItem2.Text = "&終了";
            menuItem2.Click += new EventHandler(Close_Click);
            menu.Items.Add(menuItem2);

            icon.ContextMenuStrip = menu;
        }
    }
}
