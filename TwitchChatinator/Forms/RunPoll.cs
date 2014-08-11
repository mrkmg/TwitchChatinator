using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitchChatinator
{
    public partial class RunPoll : Form
    {
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        public bool _stopThread = true;
        public int ss = 1;

        public RunPoll()
        {
            InitializeComponent();
            CenterToScreen();
            SetStyle(ControlStyles.ResizeRedraw, true);
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Text = "Fun with graphics";
            this.Resize += RunPoll_Resize;
            this.Paint += RunPoll_Paint;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            BackColor = PollSetup.getColorFromString(Settings.Default.PollChromaKey);
            this.KeyUp += RunPoll_KeyUp;


            Timer t = new Timer();
            t.Tick += t_Tick;
            t.Interval = 32;
            t.Start();
        }

        void t_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        void RunPoll_Paint(object sender, PaintEventArgs e)
        {
            int countOptions = 0;
            //if(Settings.Default.PollOption1 !=)
            Graphics g = this.CreateGraphics();
            Pen p = new Pen(Color.Red, 7);
            g.DrawLine(p, ss++, 1, 100, 100);
        }

        void RunPoll_Resize(object sender, EventArgs e)
        {
            // Invalidate(); Will be invalidated soon
        }

        void RunPoll_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        //Dragging Forms.
        // http://stackoverflow.com/questions/4767831/drag-borderless-windows-form-by-mouse
        // Thank you http://stackoverflow.com/users/39106/filip-ekberg
        
        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);
    
            if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
                message.Result = (IntPtr)HTCAPTION;
        }
    }
}
