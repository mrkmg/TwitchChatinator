namespace TwitchChatinator
{
    partial class PollSetup
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Option1Input = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Option2Input = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Option3Input = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Option4Input = new System.Windows.Forms.TextBox();
            this.SavePollSetup = new System.Windows.Forms.Button();
            this.ChromaKeyInput = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.AllowMultiDropdown = new System.Windows.Forms.ComboBox();
            this.Option1Color = new System.Windows.Forms.TextBox();
            this.Option2Color = new System.Windows.Forms.TextBox();
            this.Option3Color = new System.Windows.Forms.TextBox();
            this.Option4Color = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 24.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(115, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "Poll Setup";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Option 1";
            // 
            // Option1Input
            // 
            this.Option1Input.Location = new System.Drawing.Point(96, 60);
            this.Option1Input.Name = "Option1Input";
            this.Option1Input.Size = new System.Drawing.Size(100, 20);
            this.Option1Input.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Option 2";
            // 
            // Option2Input
            // 
            this.Option2Input.Location = new System.Drawing.Point(96, 91);
            this.Option2Input.Name = "Option2Input";
            this.Option2Input.Size = new System.Drawing.Size(100, 20);
            this.Option2Input.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Option 3";
            // 
            // Option3Input
            // 
            this.Option3Input.Location = new System.Drawing.Point(96, 120);
            this.Option3Input.Name = "Option3Input";
            this.Option3Input.Size = new System.Drawing.Size(100, 20);
            this.Option3Input.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Option 4";
            // 
            // Option4Input
            // 
            this.Option4Input.Location = new System.Drawing.Point(96, 150);
            this.Option4Input.Name = "Option4Input";
            this.Option4Input.Size = new System.Drawing.Size(100, 20);
            this.Option4Input.TabIndex = 3;
            // 
            // SavePollSetup
            // 
            this.SavePollSetup.Location = new System.Drawing.Point(150, 263);
            this.SavePollSetup.Name = "SavePollSetup";
            this.SavePollSetup.Size = new System.Drawing.Size(75, 23);
            this.SavePollSetup.TabIndex = 4;
            this.SavePollSetup.Text = "Save";
            this.SavePollSetup.UseVisualStyleBackColor = true;
            this.SavePollSetup.Click += new System.EventHandler(this.SavePollSetup_Click);
            // 
            // ChromaKeyInput
            // 
            this.ChromaKeyInput.Location = new System.Drawing.Point(96, 179);
            this.ChromaKeyInput.Name = "ChromaKeyInput";
            this.ChromaKeyInput.Size = new System.Drawing.Size(61, 20);
            this.ChromaKeyInput.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 182);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Chroma Key";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(183, 182);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Allow Multiple Votes";
            // 
            // AllowMultiDropdown
            // 
            this.AllowMultiDropdown.FormattingEnabled = true;
            this.AllowMultiDropdown.Items.AddRange(new object[] {
            "Yes",
            "No"});
            this.AllowMultiDropdown.Location = new System.Drawing.Point(291, 177);
            this.AllowMultiDropdown.Name = "AllowMultiDropdown";
            this.AllowMultiDropdown.Size = new System.Drawing.Size(62, 21);
            this.AllowMultiDropdown.TabIndex = 16;
            // 
            // Option1Color
            // 
            this.Option1Color.Location = new System.Drawing.Point(202, 60);
            this.Option1Color.Name = "Option1Color";
            this.Option1Color.Size = new System.Drawing.Size(62, 20);
            this.Option1Color.TabIndex = 7;
            // 
            // Option2Color
            // 
            this.Option2Color.Location = new System.Drawing.Point(202, 91);
            this.Option2Color.Name = "Option2Color";
            this.Option2Color.Size = new System.Drawing.Size(62, 20);
            this.Option2Color.TabIndex = 11;
            // 
            // Option3Color
            // 
            this.Option3Color.Location = new System.Drawing.Point(202, 120);
            this.Option3Color.Name = "Option3Color";
            this.Option3Color.Size = new System.Drawing.Size(62, 20);
            this.Option3Color.TabIndex = 13;
            // 
            // Option4Color
            // 
            this.Option4Color.Location = new System.Drawing.Point(202, 150);
            this.Option4Color.Name = "Option4Color";
            this.Option4Color.Size = new System.Drawing.Size(62, 20);
            this.Option4Color.TabIndex = 14;
            // 
            // PollSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 298);
            this.Controls.Add(this.Option4Color);
            this.Controls.Add(this.Option3Color);
            this.Controls.Add(this.Option2Color);
            this.Controls.Add(this.Option1Color);
            this.Controls.Add(this.AllowMultiDropdown);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.SavePollSetup);
            this.Controls.Add(this.ChromaKeyInput);
            this.Controls.Add(this.Option4Input);
            this.Controls.Add(this.Option3Input);
            this.Controls.Add(this.Option2Input);
            this.Controls.Add(this.Option1Input);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PollSetup";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PollSetup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Option1Input;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Option2Input;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Option3Input;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Option4Input;
        private System.Windows.Forms.Button SavePollSetup;
        private System.Windows.Forms.TextBox ChromaKeyInput;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox AllowMultiDropdown;
        private System.Windows.Forms.TextBox Option1Color;
        private System.Windows.Forms.TextBox Option2Color;
        private System.Windows.Forms.TextBox Option3Color;
        private System.Windows.Forms.TextBox Option4Color;

    }
}