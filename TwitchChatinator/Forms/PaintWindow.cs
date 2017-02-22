﻿using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TwitchChatinator
{
    //Thanks to http://www.codeproject.com/Articles/11114/Move-window-form-without-Titlebar-in-C
    //2014-08-13
    public class PaintWindow : Form
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        public PaintWindow()
        {
            MouseDown += PaintWindow_MouseDown;
            KeyUp += PaintWindow_KeyUp;
            SetStyle(ControlStyles.ResizeRedraw, true);
            Resize += PaintWindow_Resize;
            FormBorderStyle = FormBorderStyle.None;
            Icon = Properties.Resources.ChatinatorIcon;
        }

        void PaintWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        void PaintWindow_Resize(object sender, EventArgs e)
        {
            // Invalidate(); Will be invalidated by timer
        }

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void PaintWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;    // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaintWindow));
            SuspendLayout();
            // 
            // PaintWindow
            // 
            ClientSize = new System.Drawing.Size(284, 261);
            Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            Name = "PaintWindow";
            ResumeLayout(false);
        }
    }


}
