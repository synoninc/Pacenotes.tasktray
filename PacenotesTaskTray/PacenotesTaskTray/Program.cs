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
#if DEBUG
            conf.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, file));
#else
            conf.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, file));
#endif

            var eventlog = new EventLogTarget("eventlog");
            eventlog.Layout = "${message}${newline}${exception:format=ToString}";
            eventlog.Source = "NLogNoConfig";
            eventlog.Log = "Application";
            eventlog.EventId = "1001";
            conf.AddTarget(eventlog);
#if DEBUG
            conf.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, eventlog));
#else
            conf.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, eventlog));
#endif

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

        private int lastTargetCount = 0;
        private int lastTargetCount2 = 0;
        private int lastTargetCount3 = 0;

        private string login = "";
        private string username = "";
        private string password = "";
        private string notification = "";
        private string command = "";
        private string args = "";

        private System.Timers.Timer timer;
        private System.Timers.Timer timer2;
        private System.Timers.Timer timer3;

        private int intervalA = 60;
        private string targetA = "";
        private string target2A = "";
        private string target3A = "";
        private string cacheA = "";
        private string cache2A = "";
        private string cache3A = "";
        private string outputA = "";
        private string output2A = "";
        private string output3A = "";

        private string execA = "";
        private string exec2A = "";
        private string exec3A = "";

        private string lastA = "";
        private string last2A = "";
        private string last3A = "";

        private int lastTargetCountA = 0;
        private int lastTargetCount2A = 0;
        private int lastTargetCount3A = 0;

        private System.Timers.Timer timerA;
        private System.Timers.Timer timer2A;
        private System.Timers.Timer timer3A;

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

            this.timerA = new System.Timers.Timer(1000);
            this.timer2A = new System.Timers.Timer(1000);
            this.timer3A = new System.Timers.Timer(1000);

            this.readSetting();
            this.setTimer();
            this.setTimer2();
            this.setTimer3();

            this.setTimerA();
            this.setTimer2A();
            this.setTimer3A();

            Exe_logger.Warn("Execute Pacenotes.tasktary version 1.0 (C) Synon,Inc.");

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
                        try
                        {
                            // pdfファイルの取得
                            string[] files = Directory.GetFiles(target, "*.csv");

                            Exe_logger.Info("[1] execute timer (" + files.Length.ToString() + ")");

                            // ファイル数の変化なしの確認
                            targetCount = files.Length;
#if DEBUG
                            Console.WriteLine("[1] lastTargetCount = " + lastTargetCount);
                            Console.WriteLine("[1] targetCount = " + targetCount);
                            Console.WriteLine("[1] prevCount = " + prevCount);
#endif

                            Exe_logger.Info("[1] lastTargetCount = " + lastTargetCount);
                            Exe_logger.Info("[1] targetCount = " + targetCount);
                            Exe_logger.Info("[1] prevCount = " + prevCount);

                            if (prevCount != -1 && targetCount != 0 && prevCount == targetCount && targetCount != lastTargetCount)
                            {
#if DEBUG
#else
                                Exe_logger.Warn("[1] lastTargetCount = " + lastTargetCount);
                                Exe_logger.Warn("[1] targetCount = " + targetCount);
                                Exe_logger.Warn("[1] prevCount = " + prevCount);
#endif

                                timer.Stop();

                                // APIのログイン処理
                                Hashtable ht = new Hashtable();
                                ht["username"] = this.username;
                                ht["password"] = this.password;
                                String result = requestServer(this.login, ht);
                                JObject jResult = JObject.Parse(result);
#if DEBUG
                                Console.WriteLine(jResult["result"]);
#endif
                                if (jResult["result"].ToString() == "True")
                                {
                                    Exe_logger.Info("[1] login completed");
#if DEBUG
                                    Console.WriteLine("[1] login completed");
#endif

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

                                        Exe_logger.Warn("[1] execute completed (" + files.Length.ToString() + ") : " + param);
#if DEBUG
                                        Console.WriteLine("[1] execute completed (" + files.Length.ToString() + ") : " + param);
#endif

                                        lastTargetCount = targetCount;
                                        Exe_logger.Warn("[1] lastTargetCount = " + lastTargetCount);

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

                                Exe_logger.Info("[1] (3) targetCount = " + targetCount);
                                prevCount = targetCount;
                                Exe_logger.Info("[1] (3) prevCount = " + prevCount);

                                timer.Start();
                            }
                            if (prevCount != 0 && targetCount == 0)
                            {
#if DEBUG
                                Console.WriteLine("[1] break sub");
#endif

                                Exe_logger.Info("[1] (4) targetCount = " + targetCount);
                                prevCount = targetCount;
                                Exe_logger.Info("[1] (4) prevCount = " + prevCount);
                                lastTargetCount = targetCount;
                                Exe_logger.Info("[1] (4) lastTargetCount = " + lastTargetCount);

                            }
                            prevCount = targetCount;
                        }
                        catch (Exception ex)
                        {
                            Exe_logger.Error("[1] exception error : " + ex);

                            timer.Start();
                        }
                    }
                }
                else
                {
                    count++;
                }
            };
            timer.Start();
        }

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
                        try
                        {
                            // pdfファイルの取得
                            string[] files = Directory.GetFiles(target2, "*.csv");

                            Exe_logger.Info("[2] execute timer (" + files.Length.ToString() + ")");

                            // ファイル数の変化なしの確認
                            targetCount = files.Length;
#if DEBUG
                            Console.WriteLine("[2] lastTargetCount2 = " + lastTargetCount2);
                            Console.WriteLine("[2] targetCount = " + targetCount);
                            Console.WriteLine("[2] prevCount = " + prevCount);
#endif
                            Exe_logger.Info("[2] lastTargetCount2 = " + lastTargetCount2);
                            Exe_logger.Info("[2] targetCount = " + targetCount);
                            Exe_logger.Info("[2] prevCount = " + prevCount);

                            if (prevCount != -1 && targetCount != 0 && prevCount == targetCount && targetCount != lastTargetCount2)
                            {
#if DEBUG
#else
                                Exe_logger.Warn("[2] lastTargetCount2 = " + lastTargetCount2);
                                Exe_logger.Warn("[2] targetCount = " + targetCount);
                                Exe_logger.Warn("[2] prevCount = " + prevCount);
#endif
                                timer2.Stop();

                                // APIのログイン処理
                                Hashtable ht = new Hashtable();
                                ht["username"] = this.username;
                                ht["password"] = this.password;
                                String result = requestServer(this.login, ht);
                                JObject jResult = JObject.Parse(result);
#if DEBUG
                                Console.WriteLine(jResult["result"]);
#endif
                                if (jResult["result"].ToString() == "True")
                                {
                                    Exe_logger.Info("[2] login completed");
#if DEBUG
                                    Console.WriteLine("[2] login completed");
#endif

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

                                        Exe_logger.Warn("[2] execute completed (" + files.Length.ToString() + ") : " + param);
#if DEBUG
                                        Console.WriteLine("[2] execute completed (" + files.Length.ToString() + ") : " + param);
#endif

                                        lastTargetCount2 = targetCount;
                                        Exe_logger.Warn("[2] lastTargetCount2 = " + lastTargetCount2);

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

                                Exe_logger.Info("[2] (3) targetCount = " + targetCount);
                                prevCount = targetCount;
                                Exe_logger.Info("[2] (3) prevCount = " + prevCount);

                                timer2.Start();
                            }
                            if (prevCount != 0 && targetCount == 0)
                            {
#if DEBUG
                                Console.WriteLine("[2] break sub");
#endif

                                Exe_logger.Info("[2] (4) targetCount = " + targetCount);
                                prevCount = targetCount;
                                Exe_logger.Info("[2] (4) prevCount = " + prevCount);
                                lastTargetCount2 = targetCount;
                                Exe_logger.Info("[2] (3) lastTargetCount2 = " + lastTargetCount2);

                            }
                            prevCount = targetCount;
                        }
                        catch (Exception ex)
                        {
                            Exe_logger.Error("[2] exception error : " + ex);

                            timer2.Start();
                        }
                    }
                }
                else
                {
                    count++;
                }
            };
            timer2.Start();
        }

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
                        try
                        {
                            // pdfファイルの取得
                            string[] files = Directory.GetFiles(target3, "*.csv");

                            Exe_logger.Info("[3] execute timer (" + files.Length.ToString() + ")");

                            // ファイル数の変化なしの確認
                            targetCount = files.Length;
#if DEBUG
                            Console.WriteLine("[3] lastTargetCount3 = " + lastTargetCount3);
                            Console.WriteLine("[3] targetCount = " + targetCount);
                            Console.WriteLine("[3] prevCount = " + prevCount);
#endif
                            Exe_logger.Info("[3] lastTargetCount3 = " + lastTargetCount3);
                            Exe_logger.Info("[3] targetCount = " + targetCount);
                            Exe_logger.Info("[3] prevCount = " + prevCount);

                            if (prevCount != -1 && targetCount != 0 && prevCount == targetCount && targetCount != lastTargetCount3)
                            {
#if DEBUG
#else
                                Exe_logger.Warn("[3] lastTargetCount3 = " + lastTargetCount3);
                                Exe_logger.Warn("[3] targetCount = " + targetCount);
                                Exe_logger.Warn("[3] prevCount = " + prevCount);
#endif

                                timer3.Stop();

                                // APIのログイン処理
                                Hashtable ht = new Hashtable();
                                ht["username"] = this.username;
                                ht["password"] = this.password;
                                String result = requestServer(this.login, ht);
                                JObject jResult = JObject.Parse(result);
#if DEBUG
                                Console.WriteLine(jResult["result"]);
#endif
                                if (jResult["result"].ToString() == "True")
                                {
                                    Exe_logger.Info("[3] login completed");
#if DEBUG
                                    Console.WriteLine("[3] login completed");
#endif

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

                                        Exe_logger.Warn("[3] execute completed (" + files.Length.ToString() + ") : " + param);
#if DEBUG
                                        Console.WriteLine("[3] execute completed (" + files.Length.ToString() + ") : " + param);
#endif

                                        lastTargetCount3 = targetCount;
                                        Exe_logger.Warn("[3] lastTargetCount3 = " + lastTargetCount3);

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

                                Exe_logger.Info("[3] (3) targetCount = " + targetCount);
                                prevCount = targetCount;
                                Exe_logger.Info("[3] (3) prevCount = " + prevCount);

                                timer3.Start();
                            }
                            if (prevCount != 0 && targetCount == 0)
                            {
#if DEBUG
                                Console.WriteLine("[3] break sub");
#endif

                                Exe_logger.Info("[3] (4) targetCount = " + targetCount);
                                prevCount = targetCount;
                                Exe_logger.Info("[3] (4) prevCount = " + prevCount);
                                lastTargetCount3 = targetCount;
                                Exe_logger.Info("[3] (3) lastTargetCount3 = " + lastTargetCount3);

                            }
                            prevCount = targetCount;
                        }
                        catch (Exception ex)
                        {
                            Exe_logger.Error("[3] exception error : " + ex);

                            timer3.Start();
                        }
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
        /// 実行処理
        /// </summary>
        private String executeCommand(String command, String command2, String command3)
        {
            try
            {
                //Processオブジェクトを作成
                System.Diagnostics.Process p = new System.Diagnostics.Process();

                //入力できるようにする
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;

                //非同期で出力を読み取れるようにする
                p.StartInfo.RedirectStandardOutput = true;
                p.OutputDataReceived += p_OutputDataReceived;

                p.StartInfo.FileName =
                    System.Environment.GetEnvironmentVariable("ComSpec");
                p.StartInfo.CreateNoWindow = true;

                //起動
                p.Start();

                //非同期で出力の読み取りを開始
                p.BeginOutputReadLine();

                //入力のストリームを取得
                System.IO.StreamWriter sw = p.StandardInput;
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine(command);
                    sw.WriteLine(command2);
                    sw.WriteLine(command3);
                    //終了する
                    sw.WriteLine("exit");
                }
                sw.Close();

                p.WaitForExit();
                p.Close();

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //OutputDataReceivedイベントハンドラ
        //行が出力されるたびに呼び出される
        static void p_OutputDataReceived(object sender,
            System.Diagnostics.DataReceivedEventArgs e)
        {
            //出力された文字列を表示する
            Console.WriteLine(e.Data);
        }


        /// <summary>
        /// 監視タイマー処理
        /// </summary>
        private void setTimerA()
        {
            int count = 0;
            int prevCount = -1;
            int targetCount = 0;
            if (timerA.Enabled)
            {
                timerA.Stop();
            }
            timerA.Elapsed += (sender, e) =>
            {
                Exe_logger.Info("[1A] (0) prevCount = " + prevCount);

                // インターバルで実行
                if (count >= intervalA)
                {
                    count = 0;

                    Exe_logger.Info("[1A] execute timer");
                    if (targetA != "")
                    {
                        try
                        {
                            // pdfファイルの取得
                            string[] files = Directory.GetFiles(targetA, "*.pdf");

                            Exe_logger.Info("[1A] execute timer (" + files.Length.ToString() + ")");

                            // ファイル数の変化なしの確認
                            targetCount = files.Length;
#if DEBUG
                            Console.WriteLine("[1A] lastTargetCount = " + lastTargetCount);
                            Console.WriteLine("[1A] targetCount = " + targetCount);
                            Console.WriteLine("[1A] prevCount = " + prevCount);
#endif

                            Exe_logger.Info("[1A] lastTargetCount = " + lastTargetCount);
                            Exe_logger.Info("[1A] targetCount = " + targetCount);
                            Exe_logger.Info("[1A] prevCount = " + prevCount);

                            if (prevCount != -1 && targetCount != 0 && prevCount == targetCount && targetCount != lastTargetCount)
                            {
#if DEBUG
#else
                                Exe_logger.Warn("[1] lastTargetCount = " + lastTargetCount);
                                Exe_logger.Warn("[1] targetCount = " + targetCount);
                                Exe_logger.Warn("[1] prevCount = " + prevCount);
#endif

                                timerA.Stop();

                                // 実行処理
                                for (int i = 0; i < files.Length; i++)
                                {
                                    string filename = Path.GetFileNameWithoutExtension(files[i]);

                                    string command = this.execA.Replace("%target%", files[i]).Replace("%filename%", filename).Replace("%output%", this.outputA);
                                    string command2 = this.execA.Replace("%target%", files[i]).Replace("%filename%", filename).Replace("%output%", this.cacheA);
                                    string command3 = "del " + files[i];

                                    this.executeCommand(command, command2, command3);
                                }


                                Exe_logger.Info("[1A] (3) targetCount = " + targetCount);
                                prevCount = targetCount;
                                Exe_logger.Info("[1A] (3) prevCount = " + prevCount);

                                timerA.Start();
                            }
                            if (prevCount != 0 && targetCount == 0)
                            {
#if DEBUG
                                Console.WriteLine("[1A] break sub");
#endif

                                Exe_logger.Info("[1A] (4) targetCount = " + targetCount);
                                prevCount = targetCount;
                                Exe_logger.Info("[1A] (4) prevCount = " + prevCount);
                                lastTargetCount = targetCount;
                                Exe_logger.Info("[1A] (4) lastTargetCount = " + lastTargetCount);

                            }
                            prevCount = targetCount;
                        }
                        catch (Exception ex)
                        {
                            Exe_logger.Error("[1A] exception error : " + ex);

                            timerA.Start();
                        }
                    }
                }
                else
                {
                    count++;
                }
            };
            timerA.Start();
        }

        private void setTimer2A()
        {
            int count = 0;
            int prevCount = -1;
            int targetCount = 0;
            if (timer2A.Enabled)
            {
                timer2A.Stop();
            }
            timer2A.Elapsed += (sender, e) =>
            {
                Exe_logger.Info("[2A] (0) prevCount = " + prevCount);

                // インターバルで実行
                if (count >= intervalA)
                {
                    count = 0;

                    Exe_logger.Info("[2A] execute timer");
                    if (target2A != "")
                    {
                        try
                        {
                            // pdfファイルの取得
                            string[] files = Directory.GetFiles(target2A, "*.pdf");

                            Exe_logger.Info("[2A] execute timer (" + files.Length.ToString() + ")");

                            // ファイル数の変化なしの確認
                            targetCount = files.Length;
#if DEBUG
                            Console.WriteLine("[2A] lastTargetCount2 = " + lastTargetCount2);
                            Console.WriteLine("[2A] targetCount = " + targetCount);
                            Console.WriteLine("[2A] prevCount = " + prevCount);
#endif
                            Exe_logger.Info("[2A] lastTargetCount2 = " + lastTargetCount2);
                            Exe_logger.Info("[2A] targetCount = " + targetCount);
                            Exe_logger.Info("[2A] prevCount = " + prevCount);

                            if (prevCount != -1 && targetCount != 0 && prevCount == targetCount && targetCount != lastTargetCount2)
                            {
#if DEBUG
#else
                                Exe_logger.Warn("[2] lastTargetCount2 = " + lastTargetCount2);
                                Exe_logger.Warn("[2] targetCount = " + targetCount);
                                Exe_logger.Warn("[2] prevCount = " + prevCount);
#endif
                                timer2A.Stop();

                                // 実行処理
                                // 実行処理
                                for (int i = 0; i < files.Length; i++)
                                {
                                    string filename = Path.GetFileNameWithoutExtension(files[i]);

                                    string command = this.execA.Replace("%target%", files[i]).Replace("%filename%", filename).Replace("%output%", this.output2A);
                                    string command2 = this.execA.Replace("%target%", files[i]).Replace("%filename%", filename).Replace("%output%", this.cache2A);
                                    string command3 = "del " + files[i];

                                    this.executeCommand(command, command2, command3);
                                }

                                Exe_logger.Info("[2A] (3) targetCount = " + targetCount);
                                prevCount = targetCount;
                                Exe_logger.Info("[2A] (3) prevCount = " + prevCount);

                                timer2A.Start();
                            }
                            if (prevCount != 0 && targetCount == 0)
                            {
#if DEBUG
                                Console.WriteLine("[2A] break sub");
#endif

                                Exe_logger.Info("[2A] (4) targetCount = " + targetCount);
                                prevCount = targetCount;
                                Exe_logger.Info("[2A] (4) prevCount = " + prevCount);
                                lastTargetCount2 = targetCount;
                                Exe_logger.Info("[2A] (3) lastTargetCount2 = " + lastTargetCount2);

                            }
                            prevCount = targetCount;
                        }
                        catch (Exception ex)
                        {
                            Exe_logger.Error("[2A] exception error : " + ex);

                            timer2A.Start();
                        }
                    }
                }
                else
                {
                    count++;
                }
            };
            timer2A.Start();
        }

        private void setTimer3A()
        {
            int count = 0;
            int prevCount = -1;
            int targetCount = 0;
            if (timer3A.Enabled)
            {
                timer3A.Stop();
            }
            timer3A.Elapsed += (sender, e) =>
            {
                Exe_logger.Info("[3A] (0) prevCount = " + prevCount);

                // インターバルで実行
                if (count >= intervalA)
                {
                    count = 0;

                    Exe_logger.Info("[3A] execute timer");
                    if (target3A != "")
                    {
                        try
                        {
                            // pdfファイルの取得
                            string[] files = Directory.GetFiles(target3A, "*.pdf");

                            Exe_logger.Info("[3A] execute timer (" + files.Length.ToString() + ")");

                            // ファイル数の変化なしの確認
                            targetCount = files.Length;
#if DEBUG
                            Console.WriteLine("[3A] lastTargetCount3 = " + lastTargetCount3);
                            Console.WriteLine("[3A] targetCount = " + targetCount);
                            Console.WriteLine("[3A] prevCount = " + prevCount);
#endif
                            Exe_logger.Info("[3A] lastTargetCount3 = " + lastTargetCount3);
                            Exe_logger.Info("[3A] targetCount = " + targetCount);
                            Exe_logger.Info("[3A] prevCount = " + prevCount);

                            if (prevCount != -1 && targetCount != 0 && prevCount == targetCount && targetCount != lastTargetCount3)
                            {
#if DEBUG
#else
                                Exe_logger.Warn("[3] lastTargetCount3 = " + lastTargetCount3);
                                Exe_logger.Warn("[3] targetCount = " + targetCount);
                                Exe_logger.Warn("[3] prevCount = " + prevCount);
#endif

                                timer3A.Stop();

                                // 実行処理
                                // 実行処理
                                for (int i = 0; i < files.Length; i++)
                                {
                                    string filename = Path.GetFileNameWithoutExtension(files[i]);

                                    string command = this.execA.Replace("%target%", files[i]).Replace("%filename%", filename).Replace("%output%", this.output3A);
                                    string command2 = this.execA.Replace("%target%", files[i]).Replace("%filename%", filename).Replace("%output%", this.cache3A);
                                    string command3 = "del " + files[i];

                                    this.executeCommand(command, command2, command3);
                                }

                                Exe_logger.Info("[3A] (3) targetCount = " + targetCount);
                                prevCount = targetCount;
                                Exe_logger.Info("[3A] (3) prevCount = " + prevCount);

                                timer3A.Start();
                            }
                            if (prevCount != 0 && targetCount == 0)
                            {
#if DEBUG
                                Console.WriteLine("[3A] break sub");
#endif

                                Exe_logger.Info("[3A] (4) targetCount = " + targetCount);
                                prevCount = targetCount;
                                Exe_logger.Info("[3A] (4) prevCount = " + prevCount);
                                lastTargetCount3 = targetCount;
                                Exe_logger.Info("[3A] (3) lastTargetCount3 = " + lastTargetCount3);

                            }
                            prevCount = targetCount;
                        }
                        catch (Exception ex)
                        {
                            Exe_logger.Error("[3A] exception error : " + ex);

                            timer3A.Start();
                        }
                    }
                }
                else
                {
                    count++;
                }
            };
            timer3A.Start();
        }


        
        /// <summary>
        /// 監視タイマー処理
        /// </summary>
        private void setTimerOld()
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
                            Console.WriteLine("[1] target folder = " + subFolders[subFolders.Length - 1]);

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
                            }
                            else
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
        private void setTimer2Old()
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
        private void setTimer3Old()
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

                    this.intervalA = setting.IntervalA;
                    this.targetA = setting.TargetA;
                    this.target2A = setting.Target2A;
                    this.target3A = setting.Target3A;
                    this.cacheA = setting.CacheA;
                    this.cache2A = setting.Cache2A;
                    this.cache3A = setting.Cache3A;
                    this.outputA = setting.OutputA;
                    this.output2A = setting.Output2A;
                    this.output3A = setting.Output3A;
                    this.execA = setting.ExecA;
                    this.exec2A = setting.Exec2A;
                    this.exec3A = setting.Exec3A;
                }
            }
        }

        /// <summary>
        /// 保存処理
        /// </summary>
        private void Setting_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.ShowDialog();

            readSetting();
            this.setTimer();
            this.setTimer2();
            this.setTimer3();

            this.setTimerA();
            this.setTimer2A();
            this.setTimer3A();
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
