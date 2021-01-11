namespace PacenotesTaskTray
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textInterval = new System.Windows.Forms.TextBox();
            this.textTarget = new System.Windows.Forms.TextBox();
            this.textLogin = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textUsername = new System.Windows.Forms.TextBox();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.textNotification = new System.Windows.Forms.TextBox();
            this.buttonDialog = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.textArgs = new System.Windows.Forms.TextBox();
            this.textCommand = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "インターバル";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "監視先";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "ログインURL";
            // 
            // textInterval
            // 
            this.textInterval.Location = new System.Drawing.Point(189, 45);
            this.textInterval.Name = "textInterval";
            this.textInterval.Size = new System.Drawing.Size(100, 31);
            this.textInterval.TabIndex = 3;
            // 
            // textTarget
            // 
            this.textTarget.Location = new System.Drawing.Point(189, 112);
            this.textTarget.Name = "textTarget";
            this.textTarget.Size = new System.Drawing.Size(544, 31);
            this.textTarget.TabIndex = 4;
            // 
            // textLogin
            // 
            this.textLogin.Location = new System.Drawing.Point(189, 189);
            this.textLogin.Name = "textLogin";
            this.textLogin.Size = new System.Drawing.Size(544, 31);
            this.textLogin.TabIndex = 5;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(187, 620);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(119, 51);
            this.buttonSave.TabIndex = 6;
            this.buttonSave.Text = "保存";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(464, 620);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(119, 51);
            this.buttonClose.TabIndex = 7;
            this.buttonClose.Text = "閉じる";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 256);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 24);
            this.label4.TabIndex = 8;
            this.label4.Text = "ユーザー名";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 318);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 24);
            this.label5.TabIndex = 9;
            this.label5.Text = "パスワード";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 388);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 24);
            this.label6.TabIndex = 10;
            this.label6.Text = "通知URL";
            // 
            // textUsername
            // 
            this.textUsername.Location = new System.Drawing.Point(189, 253);
            this.textUsername.Name = "textUsername";
            this.textUsername.Size = new System.Drawing.Size(544, 31);
            this.textUsername.TabIndex = 11;
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(189, 315);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(544, 31);
            this.textPassword.TabIndex = 12;
            // 
            // textNotification
            // 
            this.textNotification.Location = new System.Drawing.Point(189, 385);
            this.textNotification.Name = "textNotification";
            this.textNotification.Size = new System.Drawing.Size(544, 31);
            this.textNotification.TabIndex = 13;
            // 
            // buttonDialog
            // 
            this.buttonDialog.Location = new System.Drawing.Point(749, 116);
            this.buttonDialog.Name = "buttonDialog";
            this.buttonDialog.Size = new System.Drawing.Size(33, 23);
            this.buttonDialog.TabIndex = 14;
            this.buttonDialog.Text = "...";
            this.buttonDialog.UseVisualStyleBackColor = true;
            this.buttonDialog.Click += new System.EventHandler(this.buttonDialog_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(310, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 24);
            this.label7.TabIndex = 15;
            this.label7.Text = "秒";
            // 
            // textArgs
            // 
            this.textArgs.Location = new System.Drawing.Point(189, 520);
            this.textArgs.Name = "textArgs";
            this.textArgs.Size = new System.Drawing.Size(544, 31);
            this.textArgs.TabIndex = 19;
            // 
            // textCommand
            // 
            this.textCommand.Location = new System.Drawing.Point(189, 458);
            this.textCommand.Name = "textCommand";
            this.textCommand.Size = new System.Drawing.Size(544, 31);
            this.textCommand.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(32, 523);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 24);
            this.label8.TabIndex = 17;
            this.label8.Text = "引数";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(32, 461);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 24);
            this.label9.TabIndex = 16;
            this.label9.Text = "コマンド";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 712);
            this.Controls.Add(this.textArgs);
            this.Controls.Add(this.textCommand);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.buttonDialog);
            this.Controls.Add(this.textNotification);
            this.Controls.Add(this.textPassword);
            this.Controls.Add(this.textUsername);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textLogin);
            this.Controls.Add(this.textTarget);
            this.Controls.Add(this.textInterval);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "設定";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textInterval;
        private System.Windows.Forms.TextBox textTarget;
        private System.Windows.Forms.TextBox textLogin;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textUsername;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.TextBox textNotification;
        private System.Windows.Forms.Button buttonDialog;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textArgs;
        private System.Windows.Forms.TextBox textCommand;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}

