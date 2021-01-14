using System;
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
    /// <summary>
    /// 環境ファイル項目の設定フォーム
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ClassSetting setting = new ClassSetting();

            var filePath = @"setting.json";

            // ファイルの存在なしでデフォルト作成
            if (!File.Exists(filePath))
            {
                setting.Interval = 60;
                setting.Target = "";
                setting.Target2 = "";
                setting.Target3 = "";
                setting.LoginUrl = "http://api.pacenotes.io/api/login";
                setting.Username = "admin";
                setting.Password = "pacenotes";
                setting.NotificationUrl = "http://api.pacenotes.io/api/command";
                setting.Command = "";
                setting.Args = "";

                var jsonData = JsonConvert.SerializeObject(setting);

                using (var sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                    sw.Write(jsonData);
                }
            }

            // ファイルの読み込みで画面作成
            using (var sr = new StreamReader(filePath, System.Text.Encoding.UTF8))
            {
                var jsonData = sr.ReadToEnd();

                setting = JsonConvert.DeserializeObject<ClassSetting>(jsonData);
                this.textInterval.Text = setting.Interval.ToString();
                this.textTarget.Text = setting.Target;
                this.textTarget2.Text = setting.Target2;
                this.textTarget3.Text = setting.Target3;
                this.textLogin.Text = setting.LoginUrl;
                this.textUsername.Text = setting.Username;
                this.textPassword.Text = setting.Password;
                this.textNotification.Text = setting.NotificationUrl;
                this.textCommand.Text = setting.Command;
                this.textArgs.Text = setting.Args;
            }
        }

        /// <summary>
        /// 閉じる処理
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 保存処理
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            ClassSetting setting = new ClassSetting();

            var filePath = @"setting.json";

            // 画面からファイルの書き出し
            int interval;
            if (int.TryParse(this.textInterval.Text, out interval))
            {
                setting.Interval = interval;
                setting.Target = this.textTarget.Text;
                setting.Target2 = this.textTarget2.Text;
                setting.Target3 = this.textTarget3.Text;
                setting.LoginUrl = this.textLogin.Text;
                setting.Username = this.textUsername.Text;
                setting.Password = this.textPassword.Text;
                setting.NotificationUrl = this.textNotification.Text;
                setting.Command = this.textCommand.Text;
                setting.Args = this.textArgs.Text;

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

        /// <summary>
        /// フォルダー選択処理
        /// </summary>
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
