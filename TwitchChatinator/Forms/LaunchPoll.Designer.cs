namespace TwitchChatinator
{
    partial class LaunchPoll
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
            this.List = new System.Windows.Forms.ComboBox();
            this.Option1 = new System.Windows.Forms.TextBox();
            this.Option2 = new System.Windows.Forms.TextBox();
            this.Option3 = new System.Windows.Forms.TextBox();
            this.Option4 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.PollTitle = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // List
            // 
            this.List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.List.FormattingEnabled = true;
            this.List.Location = new System.Drawing.Point(85, 13);
            this.List.Name = "List";
            this.List.Size = new System.Drawing.Size(187, 21);
            this.List.TabIndex = 0;
            // 
            // Option1
            // 
            this.Option1.Location = new System.Drawing.Point(85, 70);
            this.Option1.Name = "Option1";
            this.Option1.Size = new System.Drawing.Size(187, 20);
            this.Option1.TabIndex = 2;
            this.Option1.Text = "Yes";
            // 
            // Option2
            // 
            this.Option2.Location = new System.Drawing.Point(85, 96);
            this.Option2.Name = "Option2";
            this.Option2.Size = new System.Drawing.Size(187, 20);
            this.Option2.TabIndex = 3;
            this.Option2.Text = "No";
            // 
            // Option3
            // 
            this.Option3.Location = new System.Drawing.Point(85, 122);
            this.Option3.Name = "Option3";
            this.Option3.Size = new System.Drawing.Size(187, 20);
            this.Option3.TabIndex = 4;
            // 
            // Option4
            // 
            this.Option4.Location = new System.Drawing.Point(85, 148);
            this.Option4.Name = "Option4";
            this.Option4.Size = new System.Drawing.Size(187, 20);
            this.Option4.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Poll Style";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Option 1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Option 2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Option 3";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Option 4";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(103, 181);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 6;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Poll Title";
            // 
            // PollTitle
            // 
            this.PollTitle.Location = new System.Drawing.Point(85, 44);
            this.PollTitle.Name = "PollTitle";
            this.PollTitle.Size = new System.Drawing.Size(187, 20);
            this.PollTitle.TabIndex = 1;
            this.PollTitle.Text = "Strawpoll";
            // 
            // LaunchPoll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 236);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.PollTitle);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Option4);
            this.Controls.Add(this.Option3);
            this.Controls.Add(this.Option2);
            this.Controls.Add(this.Option1);
            this.Controls.Add(this.List);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LaunchPoll";
            this.Text = "Start a Poll - Chatinator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox List;
        private System.Windows.Forms.TextBox Option1;
        private System.Windows.Forms.TextBox Option2;
        private System.Windows.Forms.TextBox Option3;
        private System.Windows.Forms.TextBox Option4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox PollTitle;
    }
}