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

                setting.IntervalA = 60;
                setting.TargetA = "";
                setting.Target2A = "";
                setting.Target3A = "";
                setting.CacheA = "";
                setting.Cache2A = "";
                setting.Cache3A = "";
                setting.OutputA = "";
                setting.Output2A = "";
                setting.Output3A = "";
                setting.ExecA = "";
                setting.Exec2A = "";
                setting.Exec3A = "";

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

                this.textIntervalA.Text = setting.IntervalA.ToString();
                this.textTargetA.Text = setting.TargetA;
                this.textTarget2A.Text = setting.Target2A;
                this.textTarget3A.Text = setting.Target3A;
                this.textCacheA.Text = setting.CacheA;
                this.textCache2A.Text = setting.Cache2A;
                this.textCache3A.Text = setting.Cache3A;
                this.textOutputA.Text = setting.OutputA;
                this.textOutput2A.Text = setting.Output2A;
                this.textOutput3A.Text = setting.Output3A;
                this.textExecuteA.Text = setting.ExecA;
                this.textExecute2A.Text = setting.Exec2A;
                this.textExecute3A.Text = setting.Exec3A;
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
            int intervalA;
            if (int.TryParse(this.textInterval.Text, out interval) && int.TryParse(this.textIntervalA.Text, out intervalA))
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

                setting.IntervalA = intervalA;
                setting.TargetA = this.textTargetA.Text;
                setting.Target2A = this.textTarget2A.Text;
                setting.Target3A = this.textTarget3A.Text;
                setting.CacheA = this.textCacheA.Text;
                setting.Cache2A = this.textCache2A.Text;
                setting.Cache3A = this.textCache3A.Text;
                setting.OutputA = this.textOutputA.Text;
                setting.Output2A = this.textOutput2A.Text;
                setting.Output3A = this.textOutput3A.Text;
                setting.ExecA = this.textExecuteA.Text;
                setting.Exec2A = this.textExecute2A.Text;
                setting.Exec3A = this.textExecute3A.Text;

                var jsonData = JsonConvert.SerializeObject(setting);

                using (var sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                    sw.Write(jsonData);
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("インターバル/インターバルAは数字を入力してください。",
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

        private void buttonDialog2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "監視するフォルダ2を指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = @"C:\\";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                this.textTarget2.Text = fbd.SelectedPath;
            }
        }

        private void buttonDialog3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "監視するフォルダ3を指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = @"C:\\";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                this.textTarget3.Text = fbd.SelectedPath;
            }
        }

        private void buttonDialogA_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "PDFを監視するフォルダを指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = @"C:\\";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                this.textTargetA.Text = fbd.SelectedPath;
            }
        }

        private void buttonDialog2A_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "PDFを監視するフォルダ2を指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = @"C:\\";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                this.textTarget2A.Text = fbd.SelectedPath;
            }
        }

        private void buttonDialog3A_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "PDFを監視するフォルダ3を指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = @"C:\\";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                this.textTarget3A.Text = fbd.SelectedPath;
            }
        }

        private void buttonCacheA_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "PDFをキャッシュするフォルダを指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = @"C:\\";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                this.textCacheA.Text = fbd.SelectedPath;
            }
        }

        private void buttonCache2A_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "PDFをキャッシュするフォルダ2を指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = @"C:\\";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                this.textCache2A.Text = fbd.SelectedPath;
            }
        }

        private void buttonCache3A_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "PDFをキャッシュするフォルダ3を指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = @"C:\\";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                this.textCache3A.Text = fbd.SelectedPath;
            }
        }

        private void buttonOutputA_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "変換後のPDFを保存するフォルダを指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = @"C:\\";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                this.textOutputA.Text = fbd.SelectedPath;
            }
        }

        private void buttonOutput2A_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "変換後のPDFを保存するフォルダ2を指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = @"C:\\";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                this.textOutput2A.Text = fbd.SelectedPath;
            }
        }

        private void buttonOutput3A_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "変換後のPDFを保存するフォルダ3を指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = @"C:\\";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                this.textOutput3A.Text = fbd.SelectedPath;
            }
        }
    }
}
