namespace TwitchChatinator
{
    partial class WelcomeScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SetCredentialsButton = new System.Windows.Forms.Button();
            this.StartListenButton = new System.Windows.Forms.Button();
            this.ShowMessageBrowser = new System.Windows.Forms.Button();
            this.ListeningStatus = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
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
            this.StartListenButton.Size = new System.Drawing.Size(80, 44);
            this.StartListenButton.TabIndex = 3;
            this.StartListenButton.Text = "Start Listening";
            this.StartListenButton.UseVisualStyleBackColor = true;
            this.StartListenButton.Visible = false;
            this.StartListenButton.Click += new System.EventHandler(this.StartListenButton_Click);
            // 
            // ShowMessageBrowser
            // 
            this.ShowMessageBrowser.Location = new System.Drawing.Point(13, 63);
            this.ShowMessageBrowser.Name = "ShowMessageBrowser";
            this.ShowMessageBrowser.Size = new System.Drawing.Size(85, 44);
            this.ShowMessageBrowser.TabIndex = 5;
            this.ShowMessageBrowser.Text = "Message Browser";
            this.ShowMessageBrowser.UseVisualStyleBackColor = true;
            this.ShowMessageBrowser.Click += new System.EventHandler(this.ShowMessageBrowser_Click);
            // 
            // ListeningStatus
            // 
            this.ListeningStatus.Image = global::TwitchChatinator.Properties.Resources.Red;
            this.ListeningStatus.Location = new System.Drawing.Point(190, 17);
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
            // WelcomeScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 223);
            this.Controls.Add(this.ShowMessageBrowser);
            this.Controls.Add(this.ListeningStatus);
            this.Controls.Add(this.StartListenButton);
            this.Controls.Add(this.SetCredentialsButton);
            this.Controls.Add(this.pictureBox1);
            this.Name = "WelcomeScreen";
            this.Text = "Twitch Chatinator";
            ((System.ComponentModel.ISupportInitialize)(this.ListeningStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button SetCredentialsButton;
        public System.Windows.Forms.Button StartListenButton;
        public System.Windows.Forms.PictureBox ListeningStatus;
        private System.Windows.Forms.Button ShowMessageBrowser;
    }
}