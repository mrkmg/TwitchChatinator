namespace TwitchChatinator
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
            this.ShowMessageBrowser = new System.Windows.Forms.Button();
            this.ShowPollConfig = new System.Windows.Forms.Button();
            this.StartPollButton = new System.Windows.Forms.Button();
            this.ListeningStatus = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ResetButton = new System.Windows.Forms.Button();
            this.StartRollButton = new System.Windows.Forms.Button();
            this.SetupRollButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.CopyRandomButton = new System.Windows.Forms.Button();
            this.ConnectedLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ListeningStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // SetCredentialsButton
            // 
            this.SetCredentialsButton.Location = new System.Drawing.Point(12, 12);
            this.SetCredentialsButton.Name = "SetCredentialsButton";
            this.SetCredentialsButton.Size = new System.Drawing.Size(85, 44);
            this.SetCredentialsButton.TabIndex = 2;
            this.SetCredentialsButton.Text = "Set Twitch Credentials";
            this.SetCredentialsButton.UseVisualStyleBackColor = true;
            this.SetCredentialsButton.Click += new System.EventHandler(this.SetCredentialsButton_Click);
            // 
            // StartListenButton
            // 
            this.StartListenButton.Location = new System.Drawing.Point(103, 12);
            this.StartListenButton.Name = "StartListenButton";
            this.StartListenButton.Size = new System.Drawing.Size(86, 44);
            this.StartListenButton.TabIndex = 3;
            this.StartListenButton.Text = "Start Listening";
            this.StartListenButton.UseVisualStyleBackColor = true;
            this.StartListenButton.Visible = false;
            this.StartListenButton.Click += new System.EventHandler(this.StartListenButton_Click);
            // 
            // ShowMessageBrowser
            // 
            this.ShowMessageBrowser.Location = new System.Drawing.Point(429, 277);
            this.ShowMessageBrowser.Name = "ShowMessageBrowser";
            this.ShowMessageBrowser.Size = new System.Drawing.Size(85, 44);
            this.ShowMessageBrowser.TabIndex = 5;
            this.ShowMessageBrowser.Text = "Message Browser";
            this.ShowMessageBrowser.UseVisualStyleBackColor = true;
            this.ShowMessageBrowser.Click += new System.EventHandler(this.ShowMessageBrowser_Click);
            // 
            // ShowPollConfig
            // 
            this.ShowPollConfig.Location = new System.Drawing.Point(13, 327);
            this.ShowPollConfig.Name = "ShowPollConfig";
            this.ShowPollConfig.Size = new System.Drawing.Size(85, 44);
            this.ShowPollConfig.TabIndex = 6;
            this.ShowPollConfig.Text = "Setup Poll";
            this.ShowPollConfig.UseVisualStyleBackColor = true;
            this.ShowPollConfig.Click += new System.EventHandler(this.ShowPollConfig_Click);
            // 
            // StartPollButton
            // 
            this.StartPollButton.Location = new System.Drawing.Point(13, 162);
            this.StartPollButton.Name = "StartPollButton";
            this.StartPollButton.Size = new System.Drawing.Size(85, 44);
            this.StartPollButton.TabIndex = 7;
            this.StartPollButton.Text = "Start Poll";
            this.StartPollButton.UseVisualStyleBackColor = true;
            this.StartPollButton.Click += new System.EventHandler(this.StartPollButton_Click);
            // 
            // ListeningStatus
            // 
            this.ListeningStatus.Image = global::TwitchChatinator.Properties.Resources.Red;
            this.ListeningStatus.Location = new System.Drawing.Point(13, 79);
            this.ListeningStatus.Name = "ListeningStatus";
            this.ListeningStatus.Size = new System.Drawing.Size(32, 32);
            this.ListeningStatus.TabIndex = 4;
            this.ListeningStatus.TabStop = false;
            this.ListeningStatus.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TwitchChatinator.Properties.Resources.ChatinatorLogo;
            this.pictureBox1.Location = new System.Drawing.Point(228, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(286, 197);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(338, 327);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(85, 44);
            this.ResetButton.TabIndex = 8;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            // 
            // StartRollButton
            // 
            this.StartRollButton.Location = new System.Drawing.Point(104, 162);
            this.StartRollButton.Name = "StartRollButton";
            this.StartRollButton.Size = new System.Drawing.Size(85, 44);
            this.StartRollButton.TabIndex = 7;
            this.StartRollButton.Text = "Start Roll";
            this.StartRollButton.UseVisualStyleBackColor = true;
            this.StartRollButton.Click += new System.EventHandler(this.StartRollButton_Click);
            // 
            // SetupRollButton
            // 
            this.SetupRollButton.Location = new System.Drawing.Point(103, 327);
            this.SetupRollButton.Name = "SetupRollButton";
            this.SetupRollButton.Size = new System.Drawing.Size(85, 44);
            this.SetupRollButton.TabIndex = 9;
            this.SetupRollButton.Text = "Setup Roll";
            this.SetupRollButton.UseVisualStyleBackColor = true;
            this.SetupRollButton.Click += new System.EventHandler(this.SetupRollButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(429, 327);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(85, 44);
            this.ExportButton.TabIndex = 7;
            this.ExportButton.Text = "Export Data";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // CopyRandomButton
            // 
            this.CopyRandomButton.Location = new System.Drawing.Point(13, 212);
            this.CopyRandomButton.Name = "CopyRandomButton";
            this.CopyRandomButton.Size = new System.Drawing.Size(85, 44);
            this.CopyRandomButton.TabIndex = 10;
            this.CopyRandomButton.Text = "Copy Random User";
            this.CopyRandomButton.UseVisualStyleBackColor = true;
            this.CopyRandomButton.Click += new System.EventHandler(this.CopyRandomButton_Click);
            // 
            // ConnectedLabel
            // 
            this.ConnectedLabel.AutoSize = true;
            this.ConnectedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectedLabel.Location = new System.Drawing.Point(52, 83);
            this.ConnectedLabel.Name = "ConnectedLabel";
            this.ConnectedLabel.Size = new System.Drawing.Size(137, 24);
            this.ConnectedLabel.TabIndex = 11;
            this.ConnectedLabel.Text = "Not Connected";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(424, 211);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Made by MrKMG";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(424, 229);
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
            this.ClientSize = new System.Drawing.Size(527, 392);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ConnectedLabel);
            this.Controls.Add(this.CopyRandomButton);
            this.Controls.Add(this.SetupRollButton);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.StartRollButton);
            this.Controls.Add(this.ExportButton);
            this.Controls.Add(this.StartPollButton);
            this.Controls.Add(this.ShowPollConfig);
            this.Controls.Add(this.ShowMessageBrowser);
            this.Controls.Add(this.ListeningStatus);
            this.Controls.Add(this.StartListenButton);
            this.Controls.Add(this.SetCredentialsButton);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WelcomeScreen";
            this.Text = "Chatinator - Welcome";
            ((System.ComponentModel.ISupportInitialize)(this.ListeningStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button SetCredentialsButton;
        public System.Windows.Forms.Button StartListenButton;
        public System.Windows.Forms.PictureBox ListeningStatus;
        private System.Windows.Forms.Button ShowMessageBrowser;
        private System.Windows.Forms.Button ShowPollConfig;
        private System.Windows.Forms.Button StartPollButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button StartRollButton;
        private System.Windows.Forms.Button SetupRollButton;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button CopyRandomButton;
        private System.Windows.Forms.Label ConnectedLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label VersionLabel;
    }
}