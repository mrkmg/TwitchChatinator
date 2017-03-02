using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace TwitchChatinator.Forms.Components
{
    /// <summary>
    ///     The InputBox class is used to show a prompt in a dialog box using the static method Show().
    /// </summary>
    /// <remarks>
    ///     Copyright © 2003 Reflection IT
    ///     This software is provided 'as-is', without any express or implied warranty.
    ///     In no event will the authors be held liable for any damages arising from the
    ///     use of this software.
    ///     Permission is granted to anyone to use this software for any purpose,
    ///     including commercial applications, subject to the following restrictions:
    ///     1. The origin of this software must not be misrepresented; you must not claim
    ///     that you wrote the original software.
    ///     2. No substantial portion of the source code of this library may be redistributed
    ///     without the express written permission of the copyright holders, where
    ///     "substantial" is defined as enough code to be recognizably from this library.
    /// </remarks>
    public class InputBox : Form
    {
        protected Button ButtonCancel;
        protected Button ButtonOk;

        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private readonly Container _components = null;

        protected ErrorProvider ErrorProviderText;
        protected Label LabelPrompt;
        protected TextBox TextBoxText;

        private InputBox()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        protected InputBoxValidatingHandler Validator { get; set; }

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ButtonOk = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.TextBoxText = new System.Windows.Forms.TextBox();
            this.LabelPrompt = new System.Windows.Forms.Label();
            this.ErrorProviderText = new System.Windows.Forms.ErrorProvider();
            this.SuspendLayout();
            // 
            // ButtonOk
            // 
            this.ButtonOk.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this.ButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOk.Location = new System.Drawing.Point(288, 72);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.TabIndex = 2;
            this.ButtonOk.Text = "OK";
            this.ButtonOk.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this.ButtonCancel.CausesValidation = false;
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(376, 72);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.TabIndex = 3;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // TextBoxText
            // 
            this.TextBoxText.Location = new System.Drawing.Point(16, 32);
            this.TextBoxText.Name = "TextBoxText";
            this.TextBoxText.Size = new System.Drawing.Size(416, 20);
            this.TextBoxText.TabIndex = 1;
            this.TextBoxText.Text = "";
            this.TextBoxText.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxText_Validating);
            this.TextBoxText.TextChanged += new System.EventHandler(this.textBoxText_TextChanged);
            // 
            // LabelPrompt
            // 
            this.LabelPrompt.AutoSize = true;
            this.LabelPrompt.Location = new System.Drawing.Point(15, 15);
            this.LabelPrompt.Name = "LabelPrompt";
            this.LabelPrompt.Size = new System.Drawing.Size(39, 13);
            this.LabelPrompt.TabIndex = 0;
            this.LabelPrompt.Text = "prompt";
            // 
            // ErrorProviderText
            // 
            this.ErrorProviderText.DataMember = null;
            // 
            // InputBox
            // 
            this.AcceptButton = this.ButtonOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(464, 104);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.LabelPrompt,
                this.TextBoxText,
                this.ButtonCancel,
                this.ButtonOk
            });
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBox";
            this.Text = "Title";
            this.ResumeLayout(false);
        }

        #endregion

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Validator = null;
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     Displays a prompt in a dialog box, waits for the user to input text or click a button.
        /// </summary>
        /// <param name="prompt">String expression displayed as the message in the dialog box</param>
        /// <param name="title">String expression displayed in the title bar of the dialog box</param>
        /// <param name="defaultResponse">String expression displayed in the text box as the default response</param>
        /// <param name="validator">Delegate used to validate the text</param>
        /// <param name="xpos">
        ///     Numeric expression that specifies the distance of the left edge of the dialog box from the left edge
        ///     of the screen.
        /// </param>
        /// <param name="ypos">
        ///     Numeric expression that specifies the distance of the upper edge of the dialog box from the top of
        ///     the screen
        /// </param>
        /// <returns>An InputBoxResult object with the Text and the OK property set to true when OK was clicked.</returns>
        public static InputBoxResult Show(string prompt, string title, string defaultResponse,
            InputBoxValidatingHandler validator, int xpos, int ypos)
        {
            using (var form = new InputBox())
            {
                form.LabelPrompt.Text = prompt;
                form.Text = title;
                form.TextBoxText.Text = defaultResponse;
                if (xpos >= 0 && ypos >= 0)
                {
                    form.StartPosition = FormStartPosition.Manual;
                    form.Left = xpos;
                    form.Top = ypos;
                }
                form.Validator = validator;

                var result = form.ShowDialog();

                var retval = new InputBoxResult();
                if (result == DialogResult.OK)
                {
                    retval.Text = form.TextBoxText.Text;
                    retval.Ok = true;
                }
                return retval;
            }
        }

        /// <summary>
        ///     Displays a prompt in a dialog box, waits for the user to input text or click a button.
        /// </summary>
        /// <param name="prompt">String expression displayed as the message in the dialog box</param>
        /// <param name="title">String expression displayed in the title bar of the dialog box</param>
        /// <param name="defaultText">String expression displayed in the text box as the default response</param>
        /// <param name="validator">Delegate used to validate the text</param>
        /// <returns>An InputBoxResult object with the Text and the OK property set to true when OK was clicked.</returns>
        public static InputBoxResult Show(string prompt, string title, string defaultText,
            InputBoxValidatingHandler validator)
        {
            return Show(prompt, title, defaultText, validator, -1, -1);
        }


        /// <summary>
        ///     Reset the ErrorProvider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxText_TextChanged(object sender, EventArgs e)
        {
            ErrorProviderText.SetError(TextBoxText, "");
        }

        /// <summary>
        ///     Validate the Text using the Validator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxText_Validating(object sender, CancelEventArgs e)
        {
            if (Validator != null)
            {
                var args = new InputBoxValidatingArgs {Text = TextBoxText.Text};
                Validator(this, args);
                if (args.Cancel)
                {
                    e.Cancel = true;
                    ErrorProviderText.SetError(TextBoxText, args.Message);
                }
            }
        }
    }

    /// <summary>
    ///     Class used to store the result of an InputBox.Show message.
    /// </summary>
    public class InputBoxResult
    {
        public bool Ok;
        public string Text;
    }

    /// <summary>
    ///     EventArgs used to Validate an InputBox
    /// </summary>
    public class InputBoxValidatingArgs : EventArgs
    {
        public bool Cancel;
        public string Message;
        public string Text;
    }

    /// <summary>
    ///     Delegate used to Validate an InputBox
    /// </summary>
    public delegate void InputBoxValidatingHandler(object sender, InputBoxValidatingArgs e);
}