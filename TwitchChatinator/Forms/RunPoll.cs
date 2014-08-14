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

namespace TwitchChatinator
{
    public partial class RunPoll : PaintWindow
    {
        bool _stopThread = false;
        Thread GetDataThread;
        PollData data;
        DateTime StartTime;

        int LeftMargin;
        int RightMargin;
        int TopMargin;
        int BottomMargin;
        int BarHeight;
        int BarWidth;
        int BarSpacing;
        string FontName;
        float TitleSize;
        float CountSize;
        float TotalSize;
        string TitleColor;
        string CountColor;
        string TotalColor;
        int TotalWidth;
        Color[] BarColors;

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

            this.Text = "Poll - Chatinator";

            BackColor = PollSetup.getColorFromString(Settings.Default.PollChromaKey);
            this.Paint += RunPoll_Paint;
            this.FormClosing += RunPoll_FormClosing;
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

            int W = LeftMargin + BarWidth + TotalWidth + RightMargin;
            int H = TopMargin + (setItems * BarHeight) + ((setItems - 1) * BarSpacing) + BottomMargin;
            this.SetClientSizeCore(W, H);

            data = new PollData(setItems);

            GetDataThread = new Thread(new ThreadStart(GetDataRunner));
            GetDataThread.Start();
        }

        void RunPoll_Load(object sender, EventArgs e)
        {
            Location = Settings.Default.PollWindowLocation;
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

            LeftMargin = Settings.Default.PollLeftMargin;
            RightMargin = Settings.Default.PollRightMargin;
            TopMargin = Settings.Default.PollTopMargin;
            BottomMargin = Settings.Default.PollBottomMargin;
            BarHeight = Settings.Default.PollBarHeight;
            BarWidth = Settings.Default.PollBarWidth;
            BarSpacing = Settings.Default.PollBarSpacing;
            TitleSize = Settings.Default.PollTitleSize;
            TitleColor = Settings.Default.PollTitleColor;
            CountSize = Settings.Default.PollCountSize;
            CountColor = Settings.Default.PollCountColor;
            FontName = Settings.Default.PollFontName;
            TotalSize = Settings.Default.PollTotalSize;
            TotalColor = Settings.Default.PollTotalColor;
            TotalWidth = Settings.Default.PollTotalWidth;

            TitleBrush = new SolidBrush(PollSetup.getColorFromString(TitleColor));
            CountBrush = new SolidBrush(PollSetup.getColorFromString(CountColor));
            TotalBrush = new SolidBrush(PollSetup.getColorFromString(TotalColor));
            TitleFont = new Font(FontName, TitleSize);
            CountFont = new Font(FontName, CountSize);
            TotalFont = new Font(FontName, TotalSize);
            BarsBrush = new List<SolidBrush>();
            for (int i = 0; i <= 3; i++)
                BarsBrush.Add(new SolidBrush(BarColors[i]));
        }

        void RunPoll_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.PollWindowLocation = Location;
            Settings.Default.Save();
            StopThread();
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

                    for (int i = 0; i < Count; i++)
                    {
                        BarX = LeftMargin;
                        BarY = TopMargin + (i * BarSpacing) + (i * BarHeight);
                        if (data.totalVotes == 0)
                            BarW = 0;
                        else 
                            BarW = (BarWidth * data.amounts[i] / data.amounts.Sum());
                        BarH = BarHeight;
                        TitleX = LeftMargin + 10;
                        TitleY = TopMargin + (i * BarSpacing) + ((i) * BarHeight) + 3;
                        CountX = LeftMargin + 10;
                        CountY = TopMargin + (i * BarSpacing) + ((i + 1) * BarHeight) - CountFont.Height - 5;

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

                    TotalX = LeftMargin + BarWidth;
                    TotalY = ((Height / 2) - (TotalFont.Height / 2));
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
                        RawData = DS.GetDataSet(DSS);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            TitleBrush.Dispose();
            CountBrush.Dispose();
            TotalBrush.Dispose();

            TitleFont.Dispose();
            CountFont.Dispose();
            TotalFont.Dispose();

            for (int i = 0; i <= 3; i++)
                BarsBrush[i].Dispose();

            base.Dispose(disposing);
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
