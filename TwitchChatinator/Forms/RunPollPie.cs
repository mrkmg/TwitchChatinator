using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TwitchChatinator
{
    public partial class RunPollPie : PaintWindow
    {
        DateTime StartTime;
        string PollTitle;
        string OptionsName;
        PieGraphOptions Options;
        int CountEntries;

        SolidBrush LabelBrush;
        SolidBrush CountBrush;
        SolidBrush TotalBrush;

        SolidBrush[] PieBrushes;

        Rectangle TotalRec;
        Rectangle PieRec;

        Timer DrawingTick;

        PollData Data;

        StringFormat TitleStringFormat;
        StringFormat TotalStringFormat;
        StringFormat SFLabel;
        StringFormat SFCount;

        public RunPollPie(DateTime start, string name, string title, string[] pollvars)
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

            this.Paint += RunPollPie_Paint;
            this.Load += RunPollPie_Load;
            this.FormClosing += RunPollPie_FormClosing;
            this.Disposed += RunPollPie_Disposed;

            DrawingTick = new Timer();
            DrawingTick.Tick += DrawingTick_Tick;
            DrawingTick.Interval = 200;
            DrawingTick.Start();

        }

        void GeneratePreviewData()
        {
            Data.totalVotes = 0;
            for (int i = 0; i < CountEntries; i++)
            {
                Data.amounts[i] = 100 * (i + 1);
                Data.totalVotes += 100 * (i + 1);
            }
        }

        void GetData()
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
        }

        void DrawingTick_Tick(object sender, EventArgs e)
        {
            if (OptionsName == "_preview")
            {
                GeneratePreviewData();
            }
            else
            {
                GetData();
            }

            if (!Disposing)
                Invalidate();
        }

        void RunPollPie_Disposed(object sender, EventArgs e)
        {
            LabelBrush.Dispose();
            CountBrush.Dispose();
            TotalBrush.Dispose();

            foreach (SolidBrush B in PieBrushes) if (B != null) B.Dispose();

            DrawingTick.Stop();
            DrawingTick.Dispose();

            TitleStringFormat.Dispose();
            TotalStringFormat.Dispose();
            SFLabel.Dispose();
            SFCount.Dispose();
        }

        void RunPollPie_FormClosing(object sender, FormClosingEventArgs e)
        {
            PositionsSaver.put("PIE:" + OptionsName, Location);
        }

        void RunPollPie_Load(object sender, EventArgs e)
        {
            Point p = PositionsSaver.get("PIE:" + OptionsName);
            if (p != Point.Empty)
            {
                Location = p;
            }
        }

        void RunPollPie_Paint(object sender, PaintEventArgs e)
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

                if (Data.totalVotes > 0)
                {
                    // http://www.codeproject.com/Articles/463284/Create-Pie-Chart-Using-Graphics-in-Csharp-NET
                    // Thank you [hari19113](http://www.codeproject.com/script/Membership/View.aspx?mid=8124215)
                    float[] degrees = new float[Data.options.Count()];

                    List<LabelToDraw> LabelsToDraw = new List<LabelToDraw>();

                    float sum = 0;
                    for (int i = 0; i < CountEntries; i++)
                    {
                        sum = degrees.Sum();
                        degrees[i] = ((float)Data.amounts[i] / (float)Data.totalVotes) * 360;
                        Graphic.FillPie(PieBrushes[i % 4], PieRec, sum, degrees[i]);

                        if (Data.amounts[i] > 0)
                        {

                            var OptionWidth = Graphic.MeasureString(Data.options[i], Options.OptionFont).Width;
                            var TitleWidth = Graphic.MeasureString("(" + Data.amounts[i].ToString() + ")", Options.CountFont).Width;
                            var LabelHeight = Math.Max(Options.TitleFont.Height, Options.CountFont.Height);
                            PointF textPoint = PointOnCircle((float)PieRec.Width / 3, sum + degrees[i]/2, new PointF(PieRec.X + PieRec.Width / 2, (PieRec.Y + PieRec.Height / 2)));
                            textPoint.X -= (float)(OptionWidth + TitleWidth)/2;
                            textPoint.Y -= LabelHeight / 2;
                            LabelsToDraw.Add(new LabelToDraw(Data.options[i], Options.OptionFont, LabelBrush, textPoint));
                            PointF countPoint = new PointF(textPoint.X + OptionWidth, textPoint.Y);
                            LabelsToDraw.Add(new LabelToDraw("(" + Data.amounts[i].ToString() + ")", Options.CountFont, CountBrush, countPoint));
                        }

                    }

                    foreach(LabelToDraw L in LabelsToDraw)
                    {
                        L.Draw(Graphic);
                    }

                }

                if (Options.ForegroundImage != null && Options.ForegroundImage.Image != null)
                {
                    Graphic.DrawImage(Options.ForegroundImage.Image, new Point(0, 0));
                }
            }
        }

        //http://stackoverflow.com/questions/839899/how-do-i-calculate-a-point-on-a-circle-s-circumference
        //Thank you [Justin Ethier](http://stackoverflow.com/users/101258/justin-ethier)
        static PointF PointOnCircle(float radius, float angleInDegrees, PointF origin)
        {
            // Convert from degrees to radians via multiplication by PI/180        
            float x = (float)(radius * Math.Cos(angleInDegrees * Math.PI / 180F)) + origin.X;
            float y = (float)(radius * Math.Sin(angleInDegrees * Math.PI / 180F)) + origin.Y;

            return new PointF(x, y);
        }

        void ReadOptions()
        {
            Options = PieGraphOptions.Load(OptionsName);
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
                    offTop = Options.TitleFont.Height;
                    break;
                case "Bottom":
                default:
                    TotalRec.X = Options.MarginLeft;
                    TotalRec.Y = Options.Height - Options.MarginBottom - Options.TitleFont.Height;
                    offTop = 0;
                    break;
            }

            var AvaliableHeight = Options.Height - Options.MarginTop - Options.MarginBottom - Options.TitleFont.Height;
            var AvaliableWidth = Options.Width - Options.MarginLeft - Options.MarginRight;


            PieRec = new Rectangle();

            if (AvaliableHeight > AvaliableWidth)
            {
                int diff = AvaliableHeight - AvaliableWidth;

                PieRec.X = Options.MarginTop;
                PieRec.Y = Options.MarginLeft + diff/2 + offTop;
                PieRec.Width = Options.Width - Options.MarginLeft - Options.MarginRight;
                PieRec.Height = Options.Height - Options.MarginTop - Options.MarginBottom - Options.TitleFont.Height - diff;
            }
            else if(AvaliableWidth > AvaliableHeight)
            {
                int diff = AvaliableWidth - AvaliableHeight;

                PieRec.X = Options.MarginTop + diff/2;
                PieRec.Y = Options.MarginLeft + offTop;
                PieRec.Width = Options.Width - Options.MarginLeft - Options.MarginRight - diff;
                PieRec.Height = Options.Height - Options.MarginTop - Options.MarginBottom - Options.TitleFont.Height;
            }
            else
            {
                PieRec.X = Options.MarginLeft;
                PieRec.Y = Options.MarginTop + offTop;
                PieRec.Height = Options.Height - Options.MarginTop - Options.MarginBottom - Options.TitleFont.Height;
                PieRec.Width = Options.Width - Options.MarginLeft - Options.MarginRight;
            }

            PieBrushes = new SolidBrush[4];
            PieBrushes[0] = new SolidBrush(Options.Option1Color);
            PieBrushes[1] = new SolidBrush(Options.Option2Color);
            PieBrushes[2] = new SolidBrush(Options.Option3Color);
            PieBrushes[3] = new SolidBrush(Options.Option4Color);

        }
    }

    class LabelToDraw
    {
        string text;
        Font font;
        Brush brush;
        PointF point;

        public LabelToDraw(string t, Font f, Brush b, PointF p)
        {
            text = t;
            font = f;
            brush = b;
            point = p;
        }

        public void Draw(Graphics G)
        {
            G.DrawString(text, font, brush, point);
        }
    }
}
