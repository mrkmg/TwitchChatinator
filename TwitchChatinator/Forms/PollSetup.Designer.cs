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
            this.label2.TabIndex = 1;
            this.label2.Text = "Option 1";
            // 
            // Option1Input
            // 
            this.Option1Input.Location = new System.Drawing.Point(96, 60);
            this.Option1Input.Name = "Option1Input";
            this.Option1Input.Size = new System.Drawing.Size(257, 20);
            this.Option1Input.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Option 2";
            // 
            // Option2Input
            // 
            this.Option2Input.Location = new System.Drawing.Point(96, 91);
            this.Option2Input.Name = "Option2Input";
            this.Option2Input.Size = new System.Drawing.Size(257, 20);
            this.Option2Input.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Option 3";
            // 
            // Option3Input
            // 
            this.Option3Input.Location = new System.Drawing.Point(96, 120);
            this.Option3Input.Name = "Option3Input";
            this.Option3Input.Size = new System.Drawing.Size(257, 20);
            this.Option3Input.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Option 4";
            // 
            // Option4Input
            // 
            this.Option4Input.Location = new System.Drawing.Point(96, 150);
            this.Option4Input.Name = "Option4Input";
            this.Option4Input.Size = new System.Drawing.Size(257, 20);
            this.Option4Input.TabIndex = 2;
            // 
            // SavePollSetup
            // 
            this.SavePollSetup.Location = new System.Drawing.Point(150, 235);
            this.SavePollSetup.Name = "SavePollSetup";
            this.SavePollSetup.Size = new System.Drawing.Size(75, 23);
            this.SavePollSetup.TabIndex = 3;
            this.SavePollSetup.Text = "Save";
            this.SavePollSetup.UseVisualStyleBackColor = true;
            this.SavePollSetup.Click += new System.EventHandler(this.SavePollSetup_Click);
            // 
            // ChromaKeyInput
            // 
            this.ChromaKeyInput.Location = new System.Drawing.Point(96, 179);
            this.ChromaKeyInput.Name = "ChromaKeyInput";
            this.ChromaKeyInput.Size = new System.Drawing.Size(257, 20);
            this.ChromaKeyInput.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 182);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Chroma Key";
            // 
            // PollSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 270);
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

    }
}