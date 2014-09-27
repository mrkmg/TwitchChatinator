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
    public partial class RunPollBar : PaintWindow
    {
        DateTime StartTime;
        string PollTitle;
        string OptionsName;
        BarGraphOptions Options;
        int CountEntries;

        SolidBrush LabelBrush;
        SolidBrush CountBrush;
        SolidBrush TotalBrush;

        SolidBrush[] BarBrushes;

        Rectangle TotalRec;

        Rectangle[] BarRectangles;

        Timer DrawingTick;

        PollData Data;

        StringFormat TitleStringFormat;
        StringFormat TotalStringFormat;
        StringFormat SFLabel;
        StringFormat SFCount;

        public RunPollBar(DateTime start, string name, string title, string[] pollvars)
        {
            InitializeComponent();

            this.Text = "Poll ( " + name + ") - Chatinator";

            StartTime = start;
            OptionsName = name;
            PollTitle = title;
            CountEntries = pollvars.Count();
            Data = new PollData(CountEntries);
            Data.options = pollvars;

            ReadOptions();

            SetupVars();

            this.SetClientSizeCore(Options.Width, Options.Height);
            this.BackColor = Options.ChromaKey;

            this.Paint += RunPollBar_Paint;
            this.Load += RunPollBar_Load;
            this.FormClosing += RunPollBar_FormClosing;
            this.Disposed += RunPollBar_Disposed;

            DrawingTick = new Timer();
            DrawingTick.Tick += DrawingTick_Tick;
            DrawingTick.Interval = 200;
            DrawingTick.Start();
        }

        void RunPollBar_Disposed(object sender, EventArgs e)
        {
            LabelBrush.Dispose();
            CountBrush.Dispose();
            TotalBrush.Dispose();

            foreach(SolidBrush B in BarBrushes) if(B != null) B.Dispose();

            DrawingTick.Stop();
            DrawingTick.Dispose();

            TitleStringFormat.Dispose();
            TotalStringFormat.Dispose();
            SFLabel.Dispose();
            SFCount.Dispose();
        }

        void RunPollBar_FormClosing(object sender, FormClosingEventArgs e)
        {
            PositionsSaver.put("BAR:" + OptionsName, Location);
        }

        void RunPollBar_Load(object sender, EventArgs e)
        {
            Point p = PositionsSaver.get("BAR:" + OptionsName);
            if (p != Point.Empty)
            {
                Location = p;
            }
        }

        void RunPollBar_Paint(object sender, PaintEventArgs e)
        {
            if (Data == null)
            {
                return;
            }

            using (Graphics Graphic = CreateGraphics())
            {
                if (Options.BackgroundImage != null && Options.BackgroundImage.Image != null)
                {
                    Graphic.DrawImage(Options.BackgroundImage.Image, new Point(0, 0));
                }

                Graphic.DrawString(PollTitle, Options.TitleFont, TotalBrush, TotalRec, TitleStringFormat);
                Graphic.DrawString(Data.totalVotes.ToString() + " Votes", Options.TitleFont, TotalBrush, TotalRec, TotalStringFormat);
                for (int i = 0; i < CountEntries; i++)
                {
                    if (Data.totalVotes > 0)
                        BarRectangles[i].Width = ((Options.Width - Options.MarginRight - Options.MarginLeft) * Data.amounts[i]) / Data.totalVotes;
                    else
                        BarRectangles[i].Width = 0;
                    Graphic.FillRectangle(BarBrushes[i % 4], BarRectangles[i]);
                    Graphic.DrawString(Data.options[i], Options.OptionFont, LabelBrush, BarRectangles[i], SFLabel);
                    if (Data.amounts[i] > 0)
                        Graphic.DrawString(Data.amounts[i].ToString(), Options.CountFont, CountBrush, BarRectangles[i], SFCount);
                }

                if (Options.ForegroundImage != null && Options.ForegroundImage.Image != null)
                {
                    Graphic.DrawImage(Options.ForegroundImage.Image, new Point(0, 0));
                }
            }

        }

        void DrawingTick_Tick(object sender, EventArgs e)
        {
            int totalRows;
            int[] rowData;
            List<string> recordedUsers;

            if (CountEntries > 0)
            {
                DataSetSelection DSS = new DataSetSelection();
                DSS.Start = StartTime;
                DataSet RawData = DataStore.GetDataSet(DSS);

                totalRows = 0;
                rowData = new int[CountEntries];
                recordedUsers = new List<string>();

                foreach (DataRow row in RawData.Tables[0].Rows)
                {
                    for (int i = 0; i < CountEntries; i++)
                    {
                        if (
                            row.ItemArray[3].ToString().ToLower().Contains(Data.options[i].ToLower())
                            && (
                                Options.AllowMulti || !recordedUsers.Contains(row.ItemArray[2].ToString())
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

                for (int i = 0; i < CountEntries; i++)
                {
                    Data.amounts[i] = rowData[i];
                }
                Data.totalVotes = totalRows;
            }

            if (!Disposing)
                Invalidate();
        }

        void ReadOptions()
        {
                Options = BarGraphOptions.Load(OptionsName);
        }

        void SetupVars()
        {
            LabelBrush = new SolidBrush(Options.OptionFontColor);
            CountBrush = new SolidBrush(Options.CountFontColor);
            TotalBrush = new SolidBrush(Options.TitleFontColor);

            TotalRec = new Rectangle();
            TotalRec.Height = Options.TitleFont.Height;
            TotalRec.Width = Options.Width - Options.MarginLeft - Options.MarginRight;


            TitleStringFormat = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);
            TitleStringFormat.Alignment = StringAlignment.Near;
            TitleStringFormat.LineAlignment = StringAlignment.Center;

            TotalStringFormat = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);
            TotalStringFormat.Alignment = StringAlignment.Far;
            TotalStringFormat.LineAlignment = StringAlignment.Center;

            SFLabel = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);
            SFLabel.Alignment = StringAlignment.Near;
            SFLabel.LineAlignment = StringAlignment.Center;

            SFCount = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);
            SFCount.Alignment = StringAlignment.Far;
            SFCount.LineAlignment = StringAlignment.Near;

            int offTop = 0;

            switch (Options.TotalPosition)
            {
                case "Top":
                    TotalRec.X = Options.MarginLeft;
                    TotalRec.Y = Options.MarginTop;
                    offTop = TotalRec.Height + Options.MarginTop;
                    break;
                case "Bottom":
                default:
                    TotalRec.X = Options.MarginLeft;
                    TotalRec.Y = Options.Height - Options.MarginBottom - Options.TitleFont.Height;
                    offTop = Options.MarginTop;
                    break;
            }

            BarRectangles = new Rectangle[CountEntries];

            int avaliableHeight = Options.Height - TotalRec.Height - Options.MarginBottom - Options.MarginTop - (CountEntries * Options.BarSpacing);
            int barHeight = (avaliableHeight / CountEntries);

            for (int i = 0; i < CountEntries; i++)
            {
                BarRectangles[i].X = Options.MarginLeft;
                BarRectangles[i].Y = offTop + (barHeight * i) + (i * Options.BarSpacing);
                BarRectangles[i].Height = barHeight;
            }

            BarBrushes = new SolidBrush[4];
            BarBrushes[0] = new SolidBrush(Options.Option1Color);
            BarBrushes[1] = new SolidBrush(Options.Option2Color);
            BarBrushes[2] = new SolidBrush(Options.Option3Color);
            BarBrushes[3] = new SolidBrush(Options.Option4Color);
        }
    }
}
