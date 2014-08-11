using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TwitchChatinator
{
    public partial class RunPoll : Form
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        public bool _stopThread = false;
        Thread GetDataThread;
        PollData data;
        public int ss = 1;

        delegate void SetHeightCallback(int height);

        private const int POLLBAR_PADDING = 10;
        private const int POLLBAR_HEIGHT = 50;
        private const float POLLBAR_TEXTSIZE = 16f;
        private Color[] POLLBAR_COLORS;

        public RunPoll()
        {
            POLLBAR_COLORS = new Color[4];
            POLLBAR_COLORS[0] = PollSetup.getColorFromString(Settings.Default.PollOption1Color);
            POLLBAR_COLORS[1] = PollSetup.getColorFromString(Settings.Default.PollOption2Color);
            POLLBAR_COLORS[2] = PollSetup.getColorFromString(Settings.Default.PollOption3Color);
            POLLBAR_COLORS[3] = PollSetup.getColorFromString(Settings.Default.PollOption4Color);

            InitializeComponent();
            CenterToScreen();
            SetStyle(ControlStyles.ResizeRedraw, true);
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Text = "Fun with graphics";
            this.Resize += RunPoll_Resize;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            BackColor = PollSetup.getColorFromString(Settings.Default.PollChromaKey);
            this.KeyUp += RunPoll_KeyUp;
            this.Paint += RunPoll_Paint;
            this.FormClosing += RunPoll_FormClosing;
            this.MouseDown += RunPoll_MouseDown;
            Width = 400;

            GetDataThread = new Thread(new ThreadStart(GetDataRunner));
            GetDataThread.Start();

            System.Windows.Forms.Timer DrawTimer = new System.Windows.Forms.Timer();
            DrawTimer.Interval = 32;
            DrawTimer.Tick += DrawGraph;

            DrawTimer.Start();
        }

        void RunPoll_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopThread();
        }

        void DrawGraph(object sender, EventArgs ev)
        {
            try
            {
                Invalidate();
            }
            catch (Exception e)
            {
                //Just Log
                Log.LogException(e);
            }
        }

        void setHeight(int hei)
        {
            if (this.InvokeRequired)
            {
                SetHeightCallback h = new SetHeightCallback(setHeight);
                this.Invoke(h, new object[] { hei });
            }
            else
            {
                Height = hei;
            }
        }


        void RunPoll_Paint(object sender, PaintEventArgs e)
        {
            if (data != null)
            {
                int windowWidth = Width;
                int windowHeight = Height;

                int countOptions = data.options.Length;

                Graphics G = this.CreateGraphics();
                SolidBrush TextBrush = new SolidBrush(Color.Black);
                List<SolidBrush> Bars = new List<SolidBrush>();
                Font F = new Font(FontFamily.GenericSansSerif, POLLBAR_TEXTSIZE);

                int TextXPos;
                int TextYPos;
                int BarX1;
                int BarY1;
                int BarW;
                for (int i = 0; i < countOptions; i++)
                {
                    TextXPos = 15;
                    TextYPos = (POLLBAR_PADDING * (i + 1)) + (POLLBAR_HEIGHT * i) + (POLLBAR_HEIGHT / 2) - 16;

                    BarX1 = 10;
                    BarY1 = (POLLBAR_PADDING * (i+1)) + (POLLBAR_HEIGHT * i);
                    if (data.totalVotes == 0)
                        BarW = 0;
                    else
                        BarW = (windowWidth * data.amounts[i] / data.totalVotes) - 15;

                    Bars.Add(new SolidBrush(POLLBAR_COLORS[i]));

                    G.FillRectangle(Bars[i], BarX1, BarY1, BarW, POLLBAR_HEIGHT);
                    G.DrawString(data.options[i], F, TextBrush, TextXPos, TextYPos);
                }

                foreach (SolidBrush a in Bars)
                {
                    a.Dispose();
                }
                TextBrush.Dispose();
                F.Dispose();
                G.Dispose();
            }
        }

        void GetDataRunner()
        {
            int setItems = 0;

            if (Settings.Default.PollOption4 != String.Empty)
            {
                setItems = 4;
            }
            else if (Settings.Default.PollOption3 != String.Empty)
            {
                setItems = 3;
            }
            else if (Settings.Default.PollOption2 != String.Empty)
            {
                setItems = 2;
            }
            else if (Settings.Default.PollOption1 != String.Empty)
            {
                setItems = 1;
            }

            setHeight((setItems * POLLBAR_HEIGHT) + ((setItems + 1) * POLLBAR_PADDING));

            data = new PollData(setItems);
            for (int i = 0; i < setItems; i++)
            {
                switch (i)
                {
                    case 0:
                        data.options[i] = Settings.Default.PollOption1;
                        break;
                    case 1:
                        data.options[i] = Settings.Default.PollOption2;
                        break;
                    case 2:
                        data.options[i] = Settings.Default.PollOption3;
                        break;
                    case 3:
                        data.options[i] = Settings.Default.PollOption4;
                        break;
                }
            }

            int totalRows;
            int[] rowData;
            List<string> recordedUsers;

            using (DataStore DS = Program.getSelectedDataStore())
            {
                DataSetSelection DSS = new DataSetSelection();
                DSS.Start = DateTime.Now;
                while (!_stopThread)
                {
                    if (setItems > 0)
                    {
                        DataSet RawData = DS.getDataSet(DSS);

                        totalRows = 0;
                        rowData = new int[setItems];
                        recordedUsers = new List<string>();

                        foreach (DataRow row in RawData.Tables[0].Rows)
                        {
                            for (int i = 0; i < setItems; i++)
                            {
                                if (
                                    row.ItemArray[2].ToString().ToLower().Contains(data.options[i].ToLower())
                                    && (
                                        Settings.Default.PollAllowMulti || !recordedUsers.Contains(row.ItemArray[1].ToString())
                                       )
                                   )
                                {
                                    recordedUsers.Add(row.ItemArray[1].ToString());
                                    rowData[i]++;
                                    totalRows++;
                                    break;
                                }
                            }
                        }

                        for (int i = 0; i < setItems; i++)
                        {
                            data.amounts[i] = rowData[i];
                        }
                        data.totalVotes = totalRows;
                    }
                    Thread.Sleep(50);
                }
            }
        }

        void RunPoll_Resize(object sender, EventArgs e)
        {
            // Invalidate(); Will be invalidated by timer
        }

        void RunPoll_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }


        //Thanks to http://www.codeproject.com/Articles/11114/Move-window-form-without-Titlebar-in-C
        //<3
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void RunPoll_MouseDown(object sender, MouseEventArgs e)
        {
           if(e.Button == MouseButtons.Left)
           {
               ReleaseCapture();
               SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
           }
        }

        private void StopThread()
        {
            _stopThread = true;
        }
    }

    class PollData
    {
        public string[] options;
        public int[] amounts;
        public int totalVotes = 0;

        public PollData(int count)
        {
            options = new string[count];
            amounts = new int[count];
        }
    }
}
