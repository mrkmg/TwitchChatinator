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

        bool _stopThread = false;
        Thread GetDataThread;
        PollData data;
        DateTime StartTime;

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
        private Color[] BarColors;

        SolidBrush TitleBrush;
        SolidBrush TotalBrush;
        SolidBrush CountBrush;
        Font TitleFont;
        Font CountFont;
        Font TotalFont;
        List<SolidBrush> BarsBrush;

        public RunPoll(DateTime startTime)
        {
            StartTime = startTime;
            SetupVars();

            InitializeComponent();
            Console.WriteLine(Settings.Default._PositionPollLocation.ToString());

            SetStyle(ControlStyles.ResizeRedraw, true);
            this.Text = "Twitch Poll - Chatinator";
            this.Resize += RunPoll_Resize;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            BackColor = PollSetup.getColorFromString(Settings.Default.PollChromaKey);
            this.KeyUp += RunPoll_KeyUp;
            this.Paint += RunPoll_Paint;
            this.FormClosing += RunPoll_FormClosing;
            this.MouseDown += RunPoll_MouseDown;
            this.Load += RunPoll_Load;

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

            int W = _LeftMargin + _BarWidth + _TotalWidth + _RightMargin;
            int H = _TopMargin + (setItems * _BarHeight) + ((setItems - 1) * _BarSpacing) + _BottomMargin;
            this.SetClientSizeCore(W, H);

            data = new PollData(setItems);

            GetDataThread = new Thread(new ThreadStart(GetDataRunner));
            GetDataThread.Start();
        }

        void RunPoll_Load(object sender, EventArgs e)
        {
            Location = Settings.Default._PositionPollLocation;
        }

        public DateTime getStartTime()
        {
            return StartTime;
        }

        void StopThread()
        {
            _stopThread = true;
        }

        void SetupVars()
        {
            BarColors = new Color[4];
            BarColors[0] = PollSetup.getColorFromString(Settings.Default.PollOption1Color);
            BarColors[1] = PollSetup.getColorFromString(Settings.Default.PollOption2Color);
            BarColors[2] = PollSetup.getColorFromString(Settings.Default.PollOption3Color);
            BarColors[3] = PollSetup.getColorFromString(Settings.Default.PollOption4Color);

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

            TitleBrush = new SolidBrush(PollSetup.getColorFromString(_TitleColor));
            CountBrush = new SolidBrush(PollSetup.getColorFromString(_CountColor));
            TotalBrush = new SolidBrush(PollSetup.getColorFromString(_TotalColor));
            TitleFont = new Font(_FontName, _TitleSize);
            CountFont = new Font(_FontName, _CountSize);
            TotalFont = new Font(_FontName, _TotalSize);
            BarsBrush = new List<SolidBrush>();
            for (int i = 0; i <= 3; i++)
                BarsBrush.Add(new SolidBrush(BarColors[i]));
        }

        void DisposeVars()
        {

            for (int i = 0; i <= 3; i++)
                BarsBrush[i].Dispose();

            TotalFont.Dispose();
            CountFont.Dispose();
            TitleFont.Dispose();
            TotalBrush.Dispose();
            CountBrush.Dispose();
            TitleBrush.Dispose();
        }

        void RunPoll_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default._PositionPollLocation = Location;
            Console.WriteLine(Location);
            Settings.Default.Save();
            StopThread();
            DisposeVars();
        }

        void DrawGraph()
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

        static int PtToPxV(float font, Graphics G)
        {
            return (int)((font * G.DpiY) / 72);
        }

        void RunPoll_Paint(object sender, PaintEventArgs e)
        {
            if (data != null)
            {
                using (Graphics Graphic = CreateGraphics())
                {
                    int Count = data.options.Length;

                    int BarX;
                    int BarY;
                    int BarW;
                    int BarH;
                    int TitleX;
                    int TitleY;
                    int CountX;
                    int CountY;
                    int TotalX;
                    int TotalY;
                    int MaxAmount = 1;

                    for (int i = 0; i < Count; i++)
                    {
                        if (data.amounts[i] > MaxAmount) MaxAmount = data.amounts[i];
                    }

                    for (int i = 0; i < Count; i++)
                    {
                        BarX = _LeftMargin;
                        BarY = _TopMargin + (i * _BarSpacing) + (i * _BarHeight);
                        BarW = (_BarWidth * data.amounts[i] / MaxAmount);
                        BarH = _BarHeight;
                        TitleX = _LeftMargin + 10;
                        TitleY = _TopMargin + (i * _BarSpacing) + ((i) * _BarHeight) + 3;
                        CountX = _LeftMargin + 10;
                        CountY = _TopMargin + (i * _BarSpacing) + ((i + 1) * _BarHeight) - PtToPxV(_CountSize, Graphic) - 5;

                        //Draw Bar
                        Graphic.FillRectangle(
                                BarsBrush[i],
                                BarX,
                                BarY,
                                BarW,
                                BarH
                            );
                        //Write Text
                        Graphic.DrawString(
                                data.options[i],
                                TitleFont,
                                TitleBrush,
                                TitleX,
                                TitleY
                            );
                        //Write Counts
                        Graphic.DrawString(
                                data.amounts[i].ToString(),
                                CountFont,
                                CountBrush,
                                CountX,
                                CountY
                            );
                    }

                    TotalX = _LeftMargin + _BarWidth;
                    TotalY = ((Height / 2) - (PtToPxV(_TotalSize, Graphic) / 2));
                    //Write Title
                    Graphic.DrawString(
                            data.totalVotes.ToString(),
                            TotalFont,
                            TotalBrush,
                            TotalX,
                            TotalY
                        );
                }
            }
        }

        void GetDataRunner()
        {
            int setItems = data.options.Length;
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


            DataSetSelection DSS = new DataSetSelection();
            DSS.Start = StartTime;
            while (!_stopThread)
            {
                if (setItems > 0)
                {
                    DataSet RawData;
                    using (DataStore DS = Program.getSelectedDataStore())
                    {
                        RawData = DS.getDataSet(DSS);
                    }

                    totalRows = 0;
                    rowData = new int[setItems];
                    recordedUsers = new List<string>();

                    foreach (DataRow row in RawData.Tables[0].Rows)
                    {
                        for (int i = 0; i < setItems; i++)
                        {
                            if (
                                row.ItemArray[3].ToString().ToLower().Contains(data.options[i].ToLower())
                                && (
                                    Settings.Default.PollAllowMulti || !recordedUsers.Contains(row.ItemArray[2].ToString())
                                   )
                               )
                            {
                                recordedUsers.Add(row.ItemArray[2].ToString());
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
                DrawGraph();
                Thread.Sleep(50);
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
