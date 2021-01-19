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
            this.buttonDialog2 = new System.Windows.Forms.Button();
            this.textTarget2 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonDialog3 = new System.Windows.Forms.Button();
            this.textTarget3 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
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
            this.label3.Location = new System.Drawing.Point(35, 279);
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
            this.textLogin.Location = new System.Drawing.Point(191, 279);
            this.textLogin.Name = "textLogin";
            this.textLogin.Size = new System.Drawing.Size(544, 31);
            this.textLogin.TabIndex = 10;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(189, 710);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(119, 51);
            this.buttonSave.TabIndex = 16;
            this.buttonSave.Text = "保存";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(466, 710);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(119, 51);
            this.buttonClose.TabIndex = 17;
            this.buttonClose.Text = "閉じる";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 346);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 24);
            this.label4.TabIndex = 8;
            this.label4.Text = "ユーザー名";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 408);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 24);
            this.label5.TabIndex = 9;
            this.label5.Text = "パスワード";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(35, 478);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 24);
            this.label6.TabIndex = 10;
            this.label6.Text = "通知URL";
            // 
            // textUsername
            // 
            this.textUsername.Location = new System.Drawing.Point(191, 343);
            this.textUsername.Name = "textUsername";
            this.textUsername.Size = new System.Drawing.Size(544, 31);
            this.textUsername.TabIndex = 11;
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(191, 405);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(544, 31);
            this.textPassword.TabIndex = 12;
            // 
            // textNotification
            // 
            this.textNotification.Location = new System.Drawing.Point(191, 475);
            this.textNotification.Name = "textNotification";
            this.textNotification.Size = new System.Drawing.Size(544, 31);
            this.textNotification.TabIndex = 13;
            // 
            // buttonDialog
            // 
            this.buttonDialog.Location = new System.Drawing.Point(749, 116);
            this.buttonDialog.Name = "buttonDialog";
            this.buttonDialog.Size = new System.Drawing.Size(33, 23);
            this.buttonDialog.TabIndex = 5;
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
            this.textArgs.Location = new System.Drawing.Point(191, 610);
            this.textArgs.Name = "textArgs";
            this.textArgs.Size = new System.Drawing.Size(544, 31);
            this.textArgs.TabIndex = 15;
            // 
            // textCommand
            // 
            this.textCommand.Location = new System.Drawing.Point(191, 548);
            this.textCommand.Name = "textCommand";
            this.textCommand.Size = new System.Drawing.Size(544, 31);
            this.textCommand.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(34, 613);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 24);
            this.label8.TabIndex = 17;
            this.label8.Text = "引数";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(34, 551);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 24);
            this.label9.TabIndex = 16;
            this.label9.Text = "コマンド";
            // 
            // buttonDialog2
            // 
            this.buttonDialog2.Location = new System.Drawing.Point(749, 174);
            this.buttonDialog2.Name = "buttonDialog2";
            this.buttonDialog2.Size = new System.Drawing.Size(33, 23);
            this.buttonDialog2.TabIndex = 7;
            this.buttonDialog2.Text = "...";
            this.buttonDialog2.UseVisualStyleBackColor = true;
            this.buttonDialog2.Click += new System.EventHandler(this.buttonDialog2_Click);
            // 
            // textTarget2
            // 
            this.textTarget2.Location = new System.Drawing.Point(189, 170);
            this.textTarget2.Name = "textTarget2";
            this.textTarget2.Size = new System.Drawing.Size(544, 31);
            this.textTarget2.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(33, 173);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 24);
            this.label10.TabIndex = 20;
            this.label10.Text = "監視先2";
            // 
            // buttonDialog3
            // 
            this.buttonDialog3.Location = new System.Drawing.Point(749, 226);
            this.buttonDialog3.Name = "buttonDialog3";
            this.buttonDialog3.Size = new System.Drawing.Size(33, 23);
            this.buttonDialog3.TabIndex = 9;
            this.buttonDialog3.Text = "...";
            this.buttonDialog3.UseVisualStyleBackColor = true;
            this.buttonDialog3.Click += new System.EventHandler(this.buttonDialog3_Click);
            // 
            // textTarget3
            // 
            this.textTarget3.Location = new System.Drawing.Point(189, 222);
            this.textTarget3.Name = "textTarget3";
            this.textTarget3.Size = new System.Drawing.Size(544, 31);
            this.textTarget3.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(33, 225);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 24);
            this.label11.TabIndex = 23;
            this.label11.Text = "監視先3";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 786);
            this.Controls.Add(this.buttonDialog3);
            this.Controls.Add(this.textTarget3);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.buttonDialog2);
            this.Controls.Add(this.textTarget2);
            this.Controls.Add(this.label10);
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
        private System.Windows.Forms.Button buttonDialog2;
        private System.Windows.Forms.TextBox textTarget2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonDialog3;
        private System.Windows.Forms.TextBox textTarget3;
        private System.Windows.Forms.Label label11;
    }
}

