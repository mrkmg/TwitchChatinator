using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TwitchChatinator.Properties;

namespace TwitchChatinator.Forms.Components
{
    //Thanks to http://www.codeproject.com/Articles/11114/Move-window-form-without-Titlebar-in-C
    //2014-08-13
    public class PaintWindow : Form
    {
        private const int WmNclbuttondown = 0xA1;
        private const int Htcaption = 0x2;

        public PaintWindow()
        {
            MouseDown += PaintWindow_MouseDown;
            KeyUp += PaintWindow_KeyUp;
            SetStyle(ControlStyles.ResizeRedraw, true);
            Resize += PaintWindow_Resize;
            FormBorderStyle = FormBorderStyle.None;
            Icon = Resources.ChatinatorIcon;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        private void PaintWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void PaintWindow_Resize(object sender, EventArgs e)
        {
            // Invalidate(); Will be invalidated by timer
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void PaintWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WmNclbuttondown, Htcaption, 0);
            }
        }

        private void InitializeComponent()
        {
            var resources = new ComponentResourceManager(typeof (PaintWindow));
            SuspendLayout();
            // 
            // PaintWindow
            // 
            ClientSize = new Size(284, 261);
            Icon = (Icon) resources.GetObject("$this.Icon");
            Name = "PaintWindow";
            ResumeLayout(false);
        }
    }
}