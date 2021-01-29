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

            string mutexName = "wsMain";
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, mutexName);

            bool hasHandle = false;
            try
            {
                try
                {
                    hasHandle = mutex.WaitOne(0, false);
                }
                catch (System.Threading.AbandonedMutexException)
                {
                    hasHandle = true;
                }
                if (hasHandle == false)
                {
                    MessageBox.Show("多重起動はできません。", "PacenotesTaskTray");
                    return;
                }
                /*
                            Application.EnableVisualStyles();
                            Application.SetCompatibleTextRenderingDefault(false);
                            Application.Run(new Form1());
                */
                // メニューの初期化
                ResidentMenu rm = new ResidentMenu();
                Application.Run();

            }
            finally
            {
                if (hasHandle)
                {
                    mutex.ReleaseMutex();
                }
                mutex.Close();
            }
        }

        /// <summary>
        /// NLogのの初期化処理です。
        /// </summary>
        private static void InitializeLogger()
        {
            var conf = new LoggingConfiguration();

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
        private string target2 = "";
        private string target3 = "";

        private string last = "";
        private string last2 = "";
        private string last3 = "";

        private string login = "";
        private string username = "";
        private string password = "";
        private string notification = "";
        private string command = "";
        private string args = "";

        private System.Timers.Timer timer;
        private System.Timers.Timer timer2;
        private System.Timers.Timer timer3;

        private Logger Exe_logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 初期化処理
        /// </summary>
        public ResidentMenu()
        {
            this.ShowInTaskbar = false;
            this.setMenu();

            this.timer = new System.Timers.Timer(1000);
            this.timer2 = new System.Timers.Timer(1000);
            this.timer3 = new System.Timers.Timer(1000);

            this.readSetting();
            this.setTimer();
            this.setTimer2();
            this.setTimer3();
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
            int count = 0;
            int prevCount = -1;
            int targetCount = 0;
            if (timer.Enabled)
            {
                timer.Stop();
            }
            timer.Elapsed += (sender, e) =>
            {
                Exe_logger.Info("[1] (0) prevCount = " + prevCount);

                // インターバルで実行
                if (count >= interval)
                {
                    count = 0;

                    Exe_logger.Info("[1] execute timer");
                    if (target != "")
                    {
                        // ターゲットフォルダーのフォルダー一覧の取得
                        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(target);
                        System.IO.DirectoryInfo[] subFolders =
                            di.GetDirectories("*", System.IO.SearchOption.AllDirectories);

                        // 新規フォルダーの確認
                        targetCount = subFolders.Length;
                        Console.WriteLine("[1] targetCount = " + targetCount);
                        Console.WriteLine("[1] prevCount = " + prevCount);
                        Exe_logger.Info("[1] targetCount = " + targetCount);
                        Exe_logger.Info("[1] prevCount = " + prevCount);
                        if (prevCount != -1 && prevCount + 1 <= targetCount)
                        {
                            Exe_logger.Info("[1] (1) targetCount = " + targetCount);
                            prevCount = targetCount;
                            Exe_logger.Info("[1] (1) prevCount = " + prevCount);

                            Exe_logger.Info("[1] target folder = " + subFolders[subFolders.Length - 1]);
                            Console.WriteLine("[1] target folder = " + subFolders[subFolders.Length-1]);

                            string tmp = subFolders[subFolders.Length - 1].ToString();
                            Console.WriteLine("[1] tmp = " + tmp);
                            Console.WriteLine("[1] last = " + this.last);
                            Exe_logger.Info("[1] tmp = " + tmp);
                            Exe_logger.Info("[1] last = " + this.last);

                            if (this.last != tmp)
                            {
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

                                            Exe_logger.Info("[1] execute sub timer (" + files.Length.ToString() + ")");

                                            // ファイル数の変化なしの確認
                                            targetCountSub = files.Length;
                                            Console.WriteLine("[1] targetCountSub = " + targetCountSub);
                                            Console.WriteLine("[1] prevCountSub = " + prevCountSub);
                                            Exe_logger.Info("[1] targetCountSub = " + targetCountSub);
                                            Exe_logger.Info("[1] prevCountSub = " + prevCountSub);

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
                                                    Exe_logger.Info("[1] login completed");
                                                    Console.WriteLine("[1] login completed");

                                                    // APIのコマンド実行処理
                                                    String argument = this.args;
                                                    Hashtable htExe = new Hashtable();
                                                    htExe["command"] = this.command;
                                                    htExe["args"] = argument.Replace("%target%", "1").Replace("%count%", files.Length.ToString());
                                                    String resultExe = requestServer(this.notification, htExe);
                                                    JObject jResultExe = JObject.Parse(resultExe);
                                                    if (jResultExe["result"].ToString() == "True")
                                                    {
                                                        string param = "";
                                                        foreach (string k in files)
                                                        {
                                                            param += String.Format("{0},", k);
                                                        }

                                                        Exe_logger.Info("[1] execute completed (" + files.Length.ToString() + ") : " + param);
                                                        Console.WriteLine("[1] execute completed (" + files.Length.ToString() + ") : " + param);
                                                    }
                                                    else
                                                    {
                                                        Exe_logger.Error("[1] execute erro : " + resultExe);
                                                    }
                                                }
                                                else
                                                {
                                                    Exe_logger.Error("[1] login error : " + result);
                                                }

                                                timerSub.Stop();

                                                Exe_logger.Info("[1] (3) targetCount = " + targetCount);
                                                prevCount = targetCount;
                                                Exe_logger.Info("[1] (3) prevCount = " + prevCount);
                                                this.last = subFolders[subFolders.Length - 1].ToString();

                                                timer.Start();
                                            }
                                            if (prevCountSub != 0 && targetCountSub == 0)
                                            {
                                                Console.WriteLine("[1] break sub");
                                                timerSub.Stop();

                                                Exe_logger.Info("[1] (4) targetCount = " + targetCount);
                                                prevCount = targetCount;
                                                Exe_logger.Info("[1] (4) prevCount = " + prevCount);
                                                this.last = "";

                                                timer.Start();
                                            }
                                            prevCountSub = targetCountSub;
                                        }
                                        catch (Exception ex)
                                        {
                                            Exe_logger.Error("[1] exception error : " + ex);

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
                            } else
                            {
                                Exe_logger.Info("[1] target folder cancel ");
                                Console.WriteLine("[1] target folder cancel");

                                prevCount = targetCount;
                            }
                        }
                        Exe_logger.Info("[1] (2) targetCount = " + targetCount);
                        prevCount = targetCount;
                        Exe_logger.Info("[1] (2) prevCount = " + prevCount);
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
        /// 監視タイマー処理
        /// </summary>
        private void setTimer2()
        {
            int count = 0;
            int prevCount = -1;
            int targetCount = 0;
            if (timer2.Enabled)
            {
                timer2.Stop();
            }
            timer2.Elapsed += (sender, e) =>
            {
                Exe_logger.Info("[2] (0) prevCount = " + prevCount);

                // インターバルで実行
                if (count >= interval)
                {
                    count = 0;

                    Exe_logger.Info("[2] execute timer");
                    if (target2 != "")
                    {
                        // ターゲットフォルダーのフォルダー一覧の取得
                        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(target2);
                        System.IO.DirectoryInfo[] subFolders =
                            di.GetDirectories("*", System.IO.SearchOption.AllDirectories);

                        // 新規フォルダーの確認
                        targetCount = subFolders.Length;
                        Console.WriteLine("[2] targetCount = " + targetCount);
                        Console.WriteLine("[2] prevCount = " + prevCount);
                        Exe_logger.Info("[2] targetCount = " + targetCount);
                        Exe_logger.Info("[2] prevCount = " + prevCount);
                        if (prevCount != -1 && prevCount + 1 <= targetCount)
                        {
                            Exe_logger.Info("[2] (1) targetCount = " + targetCount);
                            prevCount = targetCount;
                            Exe_logger.Info("[2] (1) prevCount = " + prevCount);

                            Exe_logger.Info("[2] target folder = " + subFolders[subFolders.Length - 1]);
                            Console.WriteLine("[2] target folder = " + subFolders[subFolders.Length - 1]);

                            string tmp = subFolders[subFolders.Length - 1].ToString();
                            Console.WriteLine("[2] tmp = " + tmp);
                            Console.WriteLine("[2] last2 = " + this.last2);
                            Exe_logger.Info("[2] tmp = " + tmp);
                            Exe_logger.Info("[2] last2 = " + this.last2);

                            if (this.last2 != tmp)
                            {
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
                                            string[] files = Directory.GetFiles(target2 + "\\" + subFolders[subFolders.Length - 1], "*.pdf");

                                            Exe_logger.Info("[2] execute sub timer (" + files.Length.ToString() + ")");

                                            // ファイル数の変化なしの確認
                                            targetCountSub = files.Length;
                                            Console.WriteLine("[2] targetCountSub = " + targetCountSub);
                                            Console.WriteLine("[2] prevCountSub = " + prevCountSub);
                                            Exe_logger.Info("[2] targetCountSub = " + targetCountSub);
                                            Exe_logger.Info("[2] prevCountSub = " + prevCountSub);
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
                                                    Exe_logger.Info("[2] login completed");
                                                    Console.WriteLine("[2] login completed");

                                                    // APIのコマンド実行処理
                                                    String argument = this.args;
                                                    Hashtable htExe = new Hashtable();
                                                    htExe["command"] = this.command;
                                                    htExe["args"] = argument.Replace("%target%", "2").Replace("%count%", files.Length.ToString());
                                                    String resultExe = requestServer(this.notification, htExe);
                                                    JObject jResultExe = JObject.Parse(resultExe);
                                                    if (jResultExe["result"].ToString() == "True")
                                                    {
                                                        string param = "";
                                                        foreach (string k in files)
                                                        {
                                                            param += String.Format("{0},", k);
                                                        }

                                                        Exe_logger.Info("[2] execute completed (" + files.Length.ToString() + ") : " + param);
                                                        Console.WriteLine("[2] execute completed (" + files.Length.ToString() + ") : " + param);
                                                    }
                                                    else
                                                    {
                                                        Exe_logger.Error("[2] execute erro : " + resultExe);
                                                    }
                                                }
                                                else
                                                {
                                                    Exe_logger.Error("[2] login error : " + result);
                                                }

                                                timerSub.Stop();

                                                Exe_logger.Info("[2] (3) targetCount = " + targetCount);
                                                prevCount = targetCount;
                                                Exe_logger.Info("[2] (3) prevCount = " + prevCount);
                                                this.last2 = subFolders[subFolders.Length - 1].ToString();

                                                timer2.Start();
                                            }
                                            if (prevCountSub != 0 && targetCountSub == 0)
                                            {
                                                Console.WriteLine("[2] break sub");
                                                timerSub.Stop();

                                                Exe_logger.Info("[2] (4) targetCount = " + targetCount);
                                                prevCount = targetCount;
                                                Exe_logger.Info("[2] (4) prevCount = " + prevCount);
                                                this.last2 = "";

                                                timer.Start();
                                            }
                                            prevCountSub = targetCountSub;
                                        }
                                        catch (Exception ex)
                                        {
                                            Exe_logger.Error("[2] exception error : " + ex);

                                            subFolders = di.GetDirectories("*", System.IO.SearchOption.AllDirectories);
                                        }
                                    }
                                    else
                                    {
                                        countSub++;
                                    }
                                };
                                timerSub.Start();
                                timer2.Stop();
                            }
                            else
                            {
                                Exe_logger.Info("[2] target folder cancel ");
                                Console.WriteLine("[2] target folder cancel");

                                prevCount = targetCount;
                            }

                        }
                        Exe_logger.Info("[2] (2) targetCount = " + targetCount);
                        prevCount = targetCount;
                        Exe_logger.Info("[2] (2) prevCount = " + prevCount);
                    }
                }
                else
                {
                    count++;
                }
            };
            timer2.Start();
        }

        /// <summary>
        /// 監視タイマー処理
        /// </summary>
        private void setTimer3()
        {
            int count = 0;
            int prevCount = -1;
            int targetCount = 0;
            if (timer3.Enabled)
            {
                timer3.Stop();
            }
            timer3.Elapsed += (sender, e) =>
            {
                Exe_logger.Info("[3] (0) prevCount = " + prevCount);

                // インターバルで実行
                if (count >= interval)
                {
                    count = 0;

                    Exe_logger.Info("[3] execute timer");
                    if (target3 != "")
                    {
                        // ターゲットフォルダーのフォルダー一覧の取得
                        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(target3);
                        System.IO.DirectoryInfo[] subFolders =
                            di.GetDirectories("*", System.IO.SearchOption.AllDirectories);

                        // 新規フォルダーの確認
                        targetCount = subFolders.Length;
                        Console.WriteLine("[3] targetCount = " + targetCount);
                        Console.WriteLine("[3] prevCount = " + prevCount);
                        Exe_logger.Info("[3] targetCount = " + targetCount);
                        Exe_logger.Info("[3] prevCount = " + prevCount);
                        if (prevCount != -1 && prevCount + 1 <= targetCount)
                        {
                            Exe_logger.Info("[3] (1) targetCount = " + targetCount);
                            prevCount = targetCount;
                            Exe_logger.Info("[3] (1) prevCount = " + prevCount);

                            Exe_logger.Info("[3] target folder = " + subFolders[subFolders.Length - 1]);
                            Console.WriteLine("[3] target folder = " + subFolders[subFolders.Length - 1]);

                            string tmp = subFolders[subFolders.Length - 1].ToString();
                            Console.WriteLine("[3] tmp = " + tmp);
                            Console.WriteLine("[3] last3 = " + this.last3);
                            Exe_logger.Info("[3] tmp = " + tmp);
                            Exe_logger.Info("[3] last3 = " + this.last3);

                            if (this.last3 != tmp)
                            {
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
                                            string[] files = Directory.GetFiles(target3 + "\\" + subFolders[subFolders.Length - 1], "*.pdf");

                                            Exe_logger.Info("[3] execute sub timer (" + files.Length.ToString() + ")");

                                            // ファイル数の変化なしの確認
                                            targetCountSub = files.Length;
                                            Console.WriteLine("[3] targetCountSub = " + targetCountSub);
                                            Console.WriteLine("[3] prevCountSub = " + prevCountSub);
                                            Exe_logger.Info("[3] targetCountSub = " + targetCountSub);
                                            Exe_logger.Info("[3] prevCountSub = " + prevCountSub);
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
                                                    Exe_logger.Info("[3] login completed");
                                                    Console.WriteLine("[3] login completed");

                                                    // APIのコマンド実行処理
                                                    String argument = this.args;
                                                    Hashtable htExe = new Hashtable();
                                                    htExe["command"] = this.command;
                                                    htExe["args"] = argument.Replace("%target%", "3").Replace("%count%", files.Length.ToString());
                                                    String resultExe = requestServer(this.notification, htExe);
                                                    JObject jResultExe = JObject.Parse(resultExe);
                                                    if (jResultExe["result"].ToString() == "True")
                                                    {
                                                        string param = "";
                                                        foreach (string k in files)
                                                        {
                                                            param += String.Format("{0},", k);
                                                        }

                                                        Exe_logger.Info("[3] execute completed (" + files.Length.ToString() + ") : " + param);
                                                        Console.WriteLine("[3] execute completed (" + files.Length.ToString() + ") : " + param);
                                                    }
                                                    else
                                                    {
                                                        Exe_logger.Error("[3] execute erro : " + resultExe);
                                                    }
                                                }
                                                else
                                                {
                                                    Exe_logger.Error("[3] login error : " + result);
                                                }

                                                timerSub.Stop();

                                                Exe_logger.Info("[3] (3) targetCount = " + targetCount);
                                                prevCount = targetCount;
                                                Exe_logger.Info("[3] (3) prevCount = " + prevCount);
                                                this.last3 = subFolders[subFolders.Length - 1].ToString();

                                                timer3.Start();
                                            }
                                            if (prevCountSub != 0 && targetCountSub == 0)
                                            {
                                                Console.WriteLine("[3] break sub");
                                                timerSub.Stop();

                                                Exe_logger.Info("[3] (4) targetCount = " + targetCount);
                                                prevCount = targetCount;
                                                Exe_logger.Info("[3] (4) prevCount = " + prevCount);
                                                this.last3 = "";

                                                timer3.Start();
                                            }
                                            prevCountSub = targetCountSub;
                                        }
                                        catch (Exception ex)
                                        {
                                            Exe_logger.Error("[3] exception error : " + ex);

                                            subFolders = di.GetDirectories("*", System.IO.SearchOption.AllDirectories);
                                        }
                                    }
                                    else
                                    {
                                        countSub++;
                                    }
                                };
                                timerSub.Start();
                                timer3.Stop();
                            }
                            else
                            {
                                Exe_logger.Info("[3] target folder cancel ");
                                Console.WriteLine("[3] target folder cancel");

                                prevCount = targetCount;
                            }

                        }
                        Exe_logger.Info("[3] (2) targetCount = " + targetCount);
                        prevCount = targetCount;
                        Exe_logger.Info("[3] (2) prevCount = " + prevCount);
                    }
                }
                else
                {
                    count++;
                }
            };
            timer3.Start();
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
                    this.target = setting.Target;
                    this.target2 = setting.Target2;
                    this.target3 = setting.Target3;
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
            this.setTimer2();
            this.setTimer3();
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
