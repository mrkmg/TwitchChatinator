namespace TwitchChatinator
{
    partial class LaunchGiveaway
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
            this.label1 = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.GiveawayTitle = new System.Windows.Forms.TextBox();
            this.RollButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // List
            // 
            this.List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.List.FormattingEnabled = true;
            this.List.Location = new System.Drawing.Point(98, 12);
            this.List.Name = "List";
            this.List.Size = new System.Drawing.Size(126, 21);
            this.List.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Giveaway Style";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(12, 70);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(113, 23);
            this.StartButton.TabIndex = 2;
            this.StartButton.Text = "Start Giveaway";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Giveaway Title";
            // 
            // GiveawayTitle
            // 
            this.GiveawayTitle.Location = new System.Drawing.Point(98, 44);
            this.GiveawayTitle.Name = "GiveawayTitle";
            this.GiveawayTitle.Size = new System.Drawing.Size(126, 20);
            this.GiveawayTitle.TabIndex = 1;
            this.GiveawayTitle.Text = "Giveaway";
            // 
            // RollButton
            // 
            this.RollButton.Location = new System.Drawing.Point(131, 70);
            this.RollButton.Name = "RollButton";
            this.RollButton.Size = new System.Drawing.Size(93, 23);
            this.RollButton.TabIndex = 3;
            this.RollButton.Text = "Roll";
            this.RollButton.UseVisualStyleBackColor = true;
            this.RollButton.Click += new System.EventHandler(this.RollButton_Click);
            // 
            // LaunchGiveaway
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 105);
            this.Controls.Add(this.RollButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.GiveawayTitle);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.List);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LaunchGiveaway";
            this.Text = "Start a Giveaway - Chatinator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox List;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox GiveawayTitle;
        private System.Windows.Forms.Button RollButton;
    }
}