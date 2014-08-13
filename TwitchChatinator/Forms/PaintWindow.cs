using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TwitchChatinator
{
    public class PaintWindow : Form
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        public PaintWindow()
        {
            this.MouseDown += PaintWindow_MouseDown;
            this.KeyUp += PaintWindow_KeyUp;
            SetStyle(ControlStyles.ResizeRedraw, true);
            this.Resize += PaintWindow_Resize;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
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

        //Thanks to http://www.codeproject.com/Articles/11114/Move-window-form-without-Titlebar-in-C
        //<3
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
    }


}
