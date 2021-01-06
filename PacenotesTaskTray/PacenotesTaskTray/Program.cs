using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Timers;


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

            ResidentMenu rm = new ResidentMenu();
            Application.Run();

            /*
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form1());
            */
        }
    }

    class ResidentMenu : Form
    {

        private int interval = 60;
        private string target = "";
        private string login = "";
        private string username = "";
        private string password = "";
        private string notification = "";

        public ResidentMenu()
        {
            this.ShowInTaskbar = false;
            this.setMenu();

            this.readSetting();
            this.setTimer();
        }

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
                if (count >= interval)
                {
                    Console.WriteLine("execute timer");
                    if (target != "")
                    {
                        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(target);
                        System.IO.DirectoryInfo[] subFolders =
                            di.GetDirectories("*", System.IO.SearchOption.AllDirectories);

                        targetCount = subFolders.Length;
                        Console.WriteLine("targetCount = " + targetCount);
                        Console.WriteLine("prevCount = " + prevCount);
                        if (prevCount != -1 && prevCount + 1 <= targetCount)
                        {
                            Console.WriteLine("last = " + subFolders[subFolders.Length-1]);

                            System.Timers.Timer timerSub = new System.Timers.Timer(1000);

                            int countSub = 0;
                            int prevCountSub = -1;
                            int targetCountSub = 0;
                            timerSub.Elapsed += (senderSub, eSub) =>
                            {
                                if (countSub >= interval)
                                {
                                    Console.WriteLine("execute sub timer");

                                    string[] files = Directory.GetFiles(target + "\\" + subFolders[subFolders.Length - 1], "*.pdf");

                                    targetCountSub = files.Length;
                                    Console.WriteLine("targetCountSub = " + targetCountSub);
                                    Console.WriteLine("prevCountSub = " + prevCountSub);
                                    if (prevCountSub != -1 && targetCountSub != 0 && prevCountSub == targetCountSub)
                                    {
                                        Console.WriteLine("last sub = " + files[files.Length - 1]);
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

                                    countSub = 0;
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
                    count = 0;
                }
                else
                {
                    count++;
                }
            };
            timer.Start();
        }

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
                }
            }
        }

        private void Setting_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();

            readSetting();
            this.setTimer();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

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
