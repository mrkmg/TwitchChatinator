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

        private int _LeftMargin;
        private int _RightMargin;
        private int _TopMargin;
        private int _BottomMargin;
        private int _BarHeight;
        private int _BarWidth;
        private int _BarSpacing;
        private string _FontName;
        private float _TitleSize;
        private float _CountSize;
        private float _TotalSize;
        private string _TitleColor;
        private string _CountColor;
        private string _TotalColor;
        private int _TotalWidth;
        private Color[] POLLBAR_COLORS;

        Graphics Graphic;
        SolidBrush TitleBrush;
        SolidBrush TotalBrush;
        SolidBrush CountBrush;
        Font TitleFont;
        Font CountFont;
        Font TotalFont;
        List<SolidBrush> BarsBrush;

        System.Drawing.Text.PrivateFontCollection OpenSans;

        public RunPoll()
        {
            POLLBAR_COLORS = new Color[4];
            POLLBAR_COLORS[0] = PollSetup.getColorFromString(Settings.Default.PollOption1Color);
            POLLBAR_COLORS[1] = PollSetup.getColorFromString(Settings.Default.PollOption2Color);
            POLLBAR_COLORS[2] = PollSetup.getColorFromString(Settings.Default.PollOption3Color);
            POLLBAR_COLORS[3] = PollSetup.getColorFromString(Settings.Default.PollOption4Color);

            _LeftMargin = Settings.Default._PollLeftMargin;
            _RightMargin = Settings.Default._PollRightMargin;
            _TopMargin = Settings.Default._PollTopMargin;
            _BottomMargin = Settings.Default._PollBottomMargin;
            _BarHeight = Settings.Default._PollBarHeight;
            _BarWidth = Settings.Default._PollBarWidth;
            _BarSpacing = Settings.Default._PollBarSpacing;
            _TitleSize = Settings.Default._PollTitleSize;
            _TitleColor = Settings.Default._PollTitleColor;
            _CountSize = Settings.Default._PollCountSize;
            _CountColor = Settings.Default._PollCountColor;
            _FontName = Settings.Default._PollFontName;
            _TotalSize = Settings.Default._PollTotalSize;
            _TotalColor = Settings.Default._PollTotalColor;
            _TotalWidth = Settings.Default._PollTotalWidth;

            Graphic = this.CreateGraphics();
            TitleBrush = new SolidBrush(PollSetup.getColorFromString(_TitleColor));
            CountBrush = new SolidBrush(PollSetup.getColorFromString(_CountColor));
            TotalBrush = new SolidBrush(PollSetup.getColorFromString(_TotalColor));
            TitleFont = new Font(_FontName, _TitleSize);
            CountFont = new Font(_FontName, _CountSize);
            TotalFont = new Font(_FontName, _TotalSize);
            BarsBrush = new List<SolidBrush>();
            for (int i = 0; i <= 3; i++)
                BarsBrush.Add(new SolidBrush(POLLBAR_COLORS[i]));

            InitializeComponent();
            CenterToScreen();
            SetStyle(ControlStyles.ResizeRedraw, true);
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Text = "Twitch Poll - Chatinator";
            this.Resize += RunPoll_Resize;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            BackColor = PollSetup.getColorFromString(Settings.Default.PollChromaKey);
            this.KeyUp += RunPoll_KeyUp;
            this.Paint += RunPoll_Paint;
            this.FormClosing += RunPoll_FormClosing;
            this.MouseDown += RunPoll_MouseDown;

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


        void RunPoll_Paint(object sender, PaintEventArgs e)
        {
            if (data != null)
            {
                int Count = data.options.Length;

                if(Width !=_LeftMargin + _BarWidth + _TotalWidth + _RightMargin)
                {
                    Width = _LeftMargin + _BarWidth + _TotalWidth + _RightMargin;
                }
                if(Height != _TopMargin + (Count * _BarHeight) + ( (Count - 1) * _BarSpacing) + _BottomMargin)
                {
                    Height = _TopMargin + (Count * _BarHeight) + ((Count - 1) * _BarSpacing) + _BottomMargin;
                }

                for(int i = 0; i<Count; i++){
                    //Draw Bar
                    Graphic.FillRectangle(BarsBrush[i])

                    Graphic.FillRectangle(BarsBrush[i],
                            _LeftMargin,
                            (int)(_TopMargin + (i * _BarSpacing) + (i * _BarHeight)),
                            (int)((_BarWidth * data.amounts[i]/(data.totalVotes==0?1:data.totalVotes))),
                            _BarHeight
                        );
                    //Write Text
                    Graphic.DrawString(
                            data.options[i],
                            TitleFont,
                            TitleBrush,
                            (int)(_TopMargin + (i * _BarSpacing) + ( (i - 1) * _BarHeight) + (_BarHeight / 2 )),
                            _LeftMargin + 10
                        );
                }

                Graphic.DrawString(
                        data.totalVotes.ToString(),
                        TotalFont,
                        TotalBrush,
                        _LeftMargin + _BarWidth + 10,
                        (Height / 2) - (_TotalSize/2)
                    );

                foreach (SolidBrush a in BarsBrush)
                {
                    a.Dispose();
                }
                TotalFont.Dispose();
                CountFont.Dispose();
                TitleFont.Dispose();

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
            if (e.Button == MouseButtons.Left)
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
