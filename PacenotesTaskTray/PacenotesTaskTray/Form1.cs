﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;


namespace PacenotesTaskTray
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ClassSetting setting = new ClassSetting();

            var filePath = @"setting.json";

            if (!File.Exists(filePath))
            {
                // デフォルト作成
                setting.Interval = 60;
                setting.Target = "";
                setting.LoginUrl = "http://api.pacenotes.io/api/login";
                setting.Username = "admin";
                setting.Password = "pacenotes";
                setting.NotificationUrl = "http://api.pacenotes.io/api/command";

                var jsonData = JsonConvert.SerializeObject(setting);

                using (var sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                    sw.Write(jsonData);
                }
            }

            using (var sr = new StreamReader(filePath, System.Text.Encoding.UTF8))
            {
                var jsonData = sr.ReadToEnd();

                setting = JsonConvert.DeserializeObject<ClassSetting>(jsonData);
                this.textInterval.Text = setting.Interval.ToString();
                this.textTarget.Text = setting.Target;
                this.textLogin.Text = setting.LoginUrl;
                this.textUsername.Text = setting.Username;
                this.textPassword.Text = setting.Password;
                this.textNotification.Text = setting.NotificationUrl;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClassSetting setting = new ClassSetting();

            var filePath = @"setting.json";

            int interval;
            if (int.TryParse(this.textInterval.Text, out interval))
            {
                setting.Interval = interval;
                setting.Target = this.textTarget.Text;
                setting.LoginUrl = this.textLogin.Text;
                setting.Username = this.textUsername.Text;
                setting.Password = this.textPassword.Text;
                setting.NotificationUrl = this.textNotification.Text;

                var jsonData = JsonConvert.SerializeObject(setting);

                using (var sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                    sw.Write(jsonData);
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("インターバルは数字を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
        }

        private void buttonDialog_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "監視するフォルダを指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = @"C:\\";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                this.textTarget.Text = fbd.SelectedPath;
            }
        }
    }
}
