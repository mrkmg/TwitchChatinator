namespace TwitchChatinator.Forms.Launchers
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
            this.label1 = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.PollTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.EditButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.RenameButton = new System.Windows.Forms.Button();
            this.CopyButton = new System.Windows.Forms.Button();
            this.NewBarButton = new System.Windows.Forms.Button();
            this.NewPieButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // List
            // 
            this.List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.List.FormattingEnabled = true;
            this.List.Location = new System.Drawing.Point(85, 13);
            this.List.Name = "List";
            this.List.Size = new System.Drawing.Size(126, 21);
            this.List.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "_poll Style";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(7, 165);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 6;
            this.StartButton.Text = "Start _poll";
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
            this.label6.Text = "_poll Title";
            // 
            // PollTitle
            // 
            this.PollTitle.Location = new System.Drawing.Point(85, 44);
            this.PollTitle.Name = "PollTitle";
            this.PollTitle.Size = new System.Drawing.Size(126, 20);
            this.PollTitle.TabIndex = 1;
            this.PollTitle.Text = "Strawpoll";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(90, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "_options";
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Location = new System.Drawing.Point(85, 170);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(139, 13);
            this.InfoLabel.TabIndex = 14;
            this.InfoLabel.Text = "INFO_LABEL_STARTTIME";
            // 
            // EditButton
            // 
            this.EditButton.Location = new System.Drawing.Point(217, 12);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(57, 23);
            this.EditButton.TabIndex = 15;
            this.EditButton.Text = "Edit";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(280, 12);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(57, 23);
            this.DeleteButton.TabIndex = 16;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // RenameButton
            // 
            this.RenameButton.Location = new System.Drawing.Point(343, 13);
            this.RenameButton.Name = "RenameButton";
            this.RenameButton.Size = new System.Drawing.Size(57, 23);
            this.RenameButton.TabIndex = 17;
            this.RenameButton.Text = "Rename";
            this.RenameButton.UseVisualStyleBackColor = true;
            this.RenameButton.Click += new System.EventHandler(this.RenameButton_Click);
            // 
            // CopyButton
            // 
            this.CopyButton.Location = new System.Drawing.Point(217, 42);
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.Size = new System.Drawing.Size(57, 23);
            this.CopyButton.TabIndex = 18;
            this.CopyButton.Text = "Copy";
            this.CopyButton.UseVisualStyleBackColor = true;
            this.CopyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // NewBarButton
            // 
            this.NewBarButton.Location = new System.Drawing.Point(280, 42);
            this.NewBarButton.Name = "NewBarButton";
            this.NewBarButton.Size = new System.Drawing.Size(57, 23);
            this.NewBarButton.TabIndex = 19;
            this.NewBarButton.Text = "New Bar";
            this.NewBarButton.UseVisualStyleBackColor = true;
            this.NewBarButton.Click += new System.EventHandler(this.NewBarButton_Click);
            // 
            // NewPieButton
            // 
            this.NewPieButton.Location = new System.Drawing.Point(343, 42);
            this.NewPieButton.Name = "NewPieButton";
            this.NewPieButton.Size = new System.Drawing.Size(57, 23);
            this.NewPieButton.TabIndex = 20;
            this.NewPieButton.Text = "New Pie";
            this.NewPieButton.UseVisualStyleBackColor = true;
            this.NewPieButton.Click += new System.EventHandler(this.NewPieButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(406, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(57, 23);
            this.button1.TabIndex = 22;
            this.button1.Text = "Import";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(406, 13);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(57, 23);
            this.ExportButton.TabIndex = 21;
            this.ExportButton.Text = "Export";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // LaunchPoll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 209);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ExportButton);
            this.Controls.Add(this.NewPieButton);
            this.Controls.Add(this.NewBarButton);
            this.Controls.Add(this.CopyButton);
            this.Controls.Add(this.RenameButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.PollTitle);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.List);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LaunchPoll";
            this.Text = "Start a _poll - Chatinator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox List;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox PollTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button RenameButton;
        private System.Windows.Forms.Button CopyButton;
        private System.Windows.Forms.Button NewBarButton;
        private System.Windows.Forms.Button NewPieButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button ExportButton;
    }
}