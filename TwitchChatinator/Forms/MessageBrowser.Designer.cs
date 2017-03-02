namespace TwitchChatinator.Forms
{
    partial class MessageBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageBrowser));
            this.MessagesList = new System.Windows.Forms.DataGridView();
            this.GetDataButton = new System.Windows.Forms.Button();
            this.StartTimeDatepicker = new System.Windows.Forms.DateTimePicker();
            this.EndTimeDatepicker = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.MessagesList)).BeginInit();
            this.SuspendLayout();
            // 
            // MessagesList
            // 
            this.MessagesList.AllowUserToAddRows = false;
            this.MessagesList.AllowUserToDeleteRows = false;
            this.MessagesList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MessagesList.Location = new System.Drawing.Point(0, 45);
            this.MessagesList.Name = "MessagesList";
            this.MessagesList.ReadOnly = true;
            this.MessagesList.Size = new System.Drawing.Size(901, 347);
            this.MessagesList.TabIndex = 0;
            // 
            // GetDataButton
            // 
            this.GetDataButton.Location = new System.Drawing.Point(13, 13);
            this.GetDataButton.Name = "GetDataButton";
            this.GetDataButton.Size = new System.Drawing.Size(75, 23);
            this.GetDataButton.TabIndex = 1;
            this.GetDataButton.Text = "Get DATA";
            this.GetDataButton.UseVisualStyleBackColor = true;
            this.GetDataButton.Click += new System.EventHandler(this.GetDataButton_Click);
            // 
            // StartTimeDatepicker
            // 
            this.StartTimeDatepicker.Checked = false;
            this.StartTimeDatepicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartTimeDatepicker.Location = new System.Drawing.Point(148, 13);
            this.StartTimeDatepicker.Name = "StartTimeDatepicker";
            this.StartTimeDatepicker.ShowCheckBox = true;
            this.StartTimeDatepicker.Size = new System.Drawing.Size(164, 20);
            this.StartTimeDatepicker.TabIndex = 2;
            // 
            // EndTimeDatepicker
            // 
            this.EndTimeDatepicker.Checked = false;
            this.EndTimeDatepicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndTimeDatepicker.Location = new System.Drawing.Point(341, 13);
            this.EndTimeDatepicker.Name = "EndTimeDatepicker";
            this.EndTimeDatepicker.ShowCheckBox = true;
            this.EndTimeDatepicker.Size = new System.Drawing.Size(152, 20);
            this.EndTimeDatepicker.TabIndex = 3;
            // 
            // MessageBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 392);
            this.Controls.Add(this.EndTimeDatepicker);
            this.Controls.Add(this.StartTimeDatepicker);
            this.Controls.Add(this.GetDataButton);
            this.Controls.Add(this.MessagesList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MessageBrowser";
            this.Text = "Message Browser - Chatinator";
            ((System.ComponentModel.ISupportInitialize)(this.MessagesList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView MessagesList;
        private System.Windows.Forms.Button GetDataButton;
        private System.Windows.Forms.DateTimePicker StartTimeDatepicker;
        private System.Windows.Forms.DateTimePicker EndTimeDatepicker;

    }
}