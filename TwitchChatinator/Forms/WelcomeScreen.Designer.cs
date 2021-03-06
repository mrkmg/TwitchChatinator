﻿namespace TwitchChatinator.Forms
{
    partial class WelcomeScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeScreen));
            this.SetCredentialsButton = new System.Windows.Forms.Button();
            this.StartListenButton = new System.Windows.Forms.Button();
            this.StartPollButton = new System.Windows.Forms.Button();
            this.ListeningStatus = new System.Windows.Forms.PictureBox();
            this.StartRollButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.CopyRandomButton = new System.Windows.Forms.Button();
            this.ConnectedLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ListeningStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // SetCredentialsButton
            // 
            this.SetCredentialsButton.Location = new System.Drawing.Point(12, 152);
            this.SetCredentialsButton.Name = "SetCredentialsButton";
            this.SetCredentialsButton.Size = new System.Drawing.Size(87, 44);
            this.SetCredentialsButton.TabIndex = 2;
            this.SetCredentialsButton.Text = "Set Twitch Credentials";
            this.SetCredentialsButton.UseVisualStyleBackColor = true;
            this.SetCredentialsButton.Click += new System.EventHandler(this.SetCredentialsButton_Click);
            // 
            // StartListenButton
            // 
            this.StartListenButton.Location = new System.Drawing.Point(108, 152);
            this.StartListenButton.Name = "StartListenButton";
            this.StartListenButton.Size = new System.Drawing.Size(79, 44);
            this.StartListenButton.TabIndex = 3;
            this.StartListenButton.Text = "Start Listening";
            this.StartListenButton.UseVisualStyleBackColor = true;
            this.StartListenButton.Visible = false;
            this.StartListenButton.Click += new System.EventHandler(this.StartListenButton_Click);
            // 
            // StartPollButton
            // 
            this.StartPollButton.Location = new System.Drawing.Point(12, 12);
            this.StartPollButton.Name = "StartPollButton";
            this.StartPollButton.Size = new System.Drawing.Size(87, 44);
            this.StartPollButton.TabIndex = 7;
            this.StartPollButton.Text = "Start Poll";
            this.StartPollButton.UseVisualStyleBackColor = true;
            this.StartPollButton.Click += new System.EventHandler(this.StartPollButton_Click);
            // 
            // ListeningStatus
            // 
            this.ListeningStatus.Image = global::TwitchChatinator.Properties.Resources.Red;
            this.ListeningStatus.Location = new System.Drawing.Point(12, 112);
            this.ListeningStatus.Name = "ListeningStatus";
            this.ListeningStatus.Size = new System.Drawing.Size(32, 32);
            this.ListeningStatus.TabIndex = 4;
            this.ListeningStatus.TabStop = false;
            this.ListeningStatus.Visible = false;
            // 
            // StartRollButton
            // 
            this.StartRollButton.Location = new System.Drawing.Point(108, 12);
            this.StartRollButton.Name = "StartRollButton";
            this.StartRollButton.Size = new System.Drawing.Size(79, 44);
            this.StartRollButton.TabIndex = 7;
            this.StartRollButton.Text = "Start Roll";
            this.StartRollButton.UseVisualStyleBackColor = true;
            this.StartRollButton.Click += new System.EventHandler(this.StartRollButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(108, 62);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(79, 44);
            this.ExportButton.TabIndex = 7;
            this.ExportButton.Text = "Export Data";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // CopyRandomButton
            // 
            this.CopyRandomButton.Location = new System.Drawing.Point(12, 62);
            this.CopyRandomButton.Name = "CopyRandomButton";
            this.CopyRandomButton.Size = new System.Drawing.Size(87, 44);
            this.CopyRandomButton.TabIndex = 10;
            this.CopyRandomButton.Text = "Copy Random User";
            this.CopyRandomButton.UseVisualStyleBackColor = true;
            this.CopyRandomButton.Click += new System.EventHandler(this.CopyRandomButton_Click);
            // 
            // ConnectedLabel
            // 
            this.ConnectedLabel.AutoSize = true;
            this.ConnectedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectedLabel.Location = new System.Drawing.Point(50, 118);
            this.ConnectedLabel.Name = "ConnectedLabel";
            this.ConnectedLabel.Size = new System.Drawing.Size(137, 24);
            this.ConnectedLabel.TabIndex = 11;
            this.ConnectedLabel.Text = "Not Connected";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 199);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Made by MrKMG";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(132, 199);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(55, 13);
            this.VersionLabel.TabIndex = 13;
            this.VersionLabel.Text = "VERSION";
            this.VersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // WelcomeScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(201, 222);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ConnectedLabel);
            this.Controls.Add(this.CopyRandomButton);
            this.Controls.Add(this.StartRollButton);
            this.Controls.Add(this.ExportButton);
            this.Controls.Add(this.StartPollButton);
            this.Controls.Add(this.ListeningStatus);
            this.Controls.Add(this.StartListenButton);
            this.Controls.Add(this.SetCredentialsButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WelcomeScreen";
            this.Text = "Chatinator - Welcome";
            ((System.ComponentModel.ISupportInitialize)(this.ListeningStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button SetCredentialsButton;
        public System.Windows.Forms.Button StartListenButton;
        public System.Windows.Forms.PictureBox ListeningStatus;
        private System.Windows.Forms.Button StartPollButton;
        private System.Windows.Forms.Button StartRollButton;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button CopyRandomButton;
        private System.Windows.Forms.Label ConnectedLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label VersionLabel;
    }
}