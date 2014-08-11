namespace TwitchChatinator
{
    partial class SetCredentialsScreen
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
            this.LoginButton = new System.Windows.Forms.Button();
            this.UsernameInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PasswordInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CancelButtonC = new System.Windows.Forms.Button();
            this.ChannelInput = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.GetTokenLink = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.DataSourceDropdown = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // LoginButton
            // 
            this.LoginButton.Location = new System.Drawing.Point(36, 240);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(75, 23);
            this.LoginButton.TabIndex = 0;
            this.LoginButton.Text = "Save";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // UsernameInput
            // 
            this.UsernameInput.Location = new System.Drawing.Point(38, 39);
            this.UsernameInput.Name = "UsernameInput";
            this.UsernameInput.Size = new System.Drawing.Size(145, 20);
            this.UsernameInput.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Twitch Username";
            // 
            // PasswordInput
            // 
            this.PasswordInput.Location = new System.Drawing.Point(38, 92);
            this.PasswordInput.Name = "PasswordInput";
            this.PasswordInput.Size = new System.Drawing.Size(145, 20);
            this.PasswordInput.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Twitch OAuth Token";
            // 
            // CancelButtonC
            // 
            this.CancelButtonC.Location = new System.Drawing.Point(110, 240);
            this.CancelButtonC.Name = "CancelButtonC";
            this.CancelButtonC.Size = new System.Drawing.Size(75, 23);
            this.CancelButtonC.TabIndex = 5;
            this.CancelButtonC.Text = "Cancel";
            this.CancelButtonC.UseVisualStyleBackColor = true;
            this.CancelButtonC.Click += new System.EventHandler(this.CancelButtonC_Click);
            // 
            // ChannelInput
            // 
            this.ChannelInput.Location = new System.Drawing.Point(38, 157);
            this.ChannelInput.Name = "ChannelInput";
            this.ChannelInput.Size = new System.Drawing.Size(145, 20);
            this.ChannelInput.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Twitch Channel";
            // 
            // GetTokenLink
            // 
            this.GetTokenLink.AutoSize = true;
            this.GetTokenLink.Location = new System.Drawing.Point(99, 115);
            this.GetTokenLink.Name = "GetTokenLink";
            this.GetTokenLink.Size = new System.Drawing.Size(84, 13);
            this.GetTokenLink.TabIndex = 8;
            this.GetTokenLink.TabStop = true;
            this.GetTokenLink.Text = "Get Token Here";
            this.GetTokenLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.GetTokenLink_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 192);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Data Source";
            // 
            // DataSourceDropdown
            // 
            this.DataSourceDropdown.FormattingEnabled = true;
            this.DataSourceDropdown.Items.AddRange(new object[] {
            "SQLite",
            "Memory"});
            this.DataSourceDropdown.Location = new System.Drawing.Point(38, 209);
            this.DataSourceDropdown.Name = "DataSourceDropdown";
            this.DataSourceDropdown.Size = new System.Drawing.Size(145, 21);
            this.DataSourceDropdown.TabIndex = 11;
            // 
            // SetCredentialsScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(214, 318);
            this.Controls.Add(this.DataSourceDropdown);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.GetTokenLink);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ChannelInput);
            this.Controls.Add(this.CancelButtonC);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PasswordInput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UsernameInput);
            this.Controls.Add(this.LoginButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SetCredentialsScreen";
            this.Text = "Set Credentials";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.TextBox UsernameInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PasswordInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CancelButtonC;
        private System.Windows.Forms.TextBox ChannelInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel GetTokenLink;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox DataSourceDropdown;
    }
}