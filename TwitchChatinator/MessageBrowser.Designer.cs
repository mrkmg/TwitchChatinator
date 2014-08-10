namespace TwitchChatinator
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
            this.MessagesList = new System.Windows.Forms.DataGridView();
            this.GetDataButton = new System.Windows.Forms.Button();
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
            this.MessagesList.Size = new System.Drawing.Size(901, 364);
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
            // MessageBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 392);
            this.Controls.Add(this.GetDataButton);
            this.Controls.Add(this.MessagesList);
            this.Name = "MessageBrowser";
            this.Text = "Message Browser";
            ((System.ComponentModel.ISupportInitialize)(this.MessagesList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView MessagesList;
        private System.Windows.Forms.Button GetDataButton;

    }
}