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
        /*
        DateTime StartTime;

        int LeftMargin;
        int RightMargin;
        int TopMargin;
        int BottomMargin;
        int BarHeight;
        int BarWidth;
        int BarSpacing;
        int TotalWidth;

        Color[] BarColors;

        SolidBrush TitleBrush;
        SolidBrush TotalBrush;
        SolidBrush CountBrush;

        Font TitleFont;
        Font CountFont;
        Font TotalFont;

        List<SolidBrush> BarsBrush;

        PollData data;

        System.Windows.Forms.Timer DrawingTick;

        public RunPoll(DateTime startTime)
        {
            StartTime = startTime;
            InitializeComponent();
            SetupVars();


            this.Text = "Poll - Chatinator";

            BackColor = Settings.Default.PollChromaKey;
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

            DrawingTick = new System.Windows.Forms.Timer();
            DrawingTick.Tick += DrawingTick_Tick;
            DrawingTick.Interval = 200;
            DrawingTick.Start();
        }

        void DrawingTick_Tick(object sender, EventArgs e)
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

            if (setItems > 0)
            {
                DataSetSelection DSS = new DataSetSelection();
                DSS.Start = StartTime;
                DataSet RawData = DataStore.GetDataSet(DSS);

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

            if(!Disposing)
                Invalidate();
        }

        void RunPoll_Load(object sender, EventArgs e)
        {
            Location = Settings.Default.PollWindowLocation;
        }

        void SetupVars()
        {
            BarColors = new Color[4];
            BarColors[0] = Settings.Default.PollOption1Color;
            BarColors[1] = Settings.Default.PollOption2Color;
            BarColors[2] = Settings.Default.PollOption3Color;
            BarColors[3] = Settings.Default.PollOption4Color;

            TitleBrush = new SolidBrush(Settings.Default.PollTitleColor);
            CountBrush = new SolidBrush(Settings.Default.PollCountColor);
            TotalBrush = new SolidBrush(Settings.Default.PollTotalColor);

            BarsBrush = new List<SolidBrush>();
            for (int i = 0; i <= 3; i++)
                BarsBrush.Add(new SolidBrush(BarColors[i]));

            TitleFont = Settings.Default.PollTitleFont;
            CountFont = Settings.Default.PollCountFont;
            TotalFont = Settings.Default.PollTotalFont;

            LeftMargin = Settings.Default.PollLeftMargin;
            RightMargin = Settings.Default.PollRightMargin;
            TopMargin = Settings.Default.PollTopMargin;
            BottomMargin = Settings.Default.PollBottomMargin;
            BarHeight = Settings.Default.PollBarHeight;
            BarWidth = Settings.Default.PollBarWidth;
            BarSpacing = Settings.Default.PollBarSpacing;
            TotalWidth = Settings.Default.PollTotalWidth;
        }

        void RunPoll_FormClosing(object sender, FormClosingEventArgs e)
        {
            DrawingTick.Stop();
            Settings.Default.PollWindowLocation = Location;
            Settings.Default.Save();
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


                    BarX = LeftMargin;
                    TitleX = LeftMargin + 10;
                    CountX = LeftMargin + 10;
                    BarH = BarHeight;

                    for (int i = 0; i < Count; i++)
                    {
                        BarY = TopMargin + (i * BarSpacing) + (i * BarHeight);
                        if (data.totalVotes == 0)
                            BarW = 0;
                        else
                            BarW = (BarWidth * data.amounts[i] / data.amounts.Sum());
                        TitleY = TopMargin + (i * BarSpacing) + ((i) * BarHeight);
                        CountY = TopMargin + (i * BarSpacing) + ((i + 1) * BarHeight) - (int)CountFont.GetHeight(Graphic);

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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);

            TitleBrush.Dispose();
            CountBrush.Dispose();
            TotalBrush.Dispose();

            for (int i = 0; i <= 3; i++)
                BarsBrush[i].Dispose();

        }
         * */
    }
}
