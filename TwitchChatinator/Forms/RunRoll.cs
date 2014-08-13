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
    public partial class RunRoll : PaintWindow
    {
        const short STAGE_WAITING = 0;
        const short STAGE_ROLLING = 1;
        const short STAGE_POSTROLL = 2;
        const short STAGE_DIE = 255;

        const double ROLLING_MAGIC_NUM = 1.35;

        int LeftMargin;
        int RightMargin;
        int TopMargin;
        int BottomMargin;
        int TotalWidth;

        Font TitleFont;
        Font RollerFont;
        Font EntriesFont;

        Color TitleColor;
        Color RollerColor;
        Color EntriesColor;
        Color ChromaKey;

        int RollerTop;
        int EntriesTop;

        string RollerText;
        string TitleText;

        SolidBrush TitleBrush;
        SolidBrush RollerBrush;
        SolidBrush EntriesBrush;

        DateTime StartTime;
        DateTime EndTime;

        int Stage = STAGE_WAITING;
        System.Windows.Forms.Timer DrawingTick;

        int Count = 0;
        List<string> EntryList;
        int CurrentEntryIndex;
        int TotalEntriesFound;

        public RunRoll(DateTime startTime)
        {
            StartTime = startTime;
            SetupVars();

            InitializeComponent();

            this.Text = "Roller - Chatinator";
            this.BackColor = ChromaKey;

            this.Paint += RunRoll_Paint;
            this.MouseDown += RunRoll_MouseDown;
            this.MouseMove += RunRoll_MouseMove;
            this.FormClosing += RunRoll_FormClosing;
            this.Load += RunRoll_Load;
            using (Graphics G = CreateGraphics())
            {
                int WindowHeight =
                        TopMargin +
                        BottomMargin +
                        RollerTop +
                        EntriesTop +
                        TitleFont.Height +
                        RollerFont.Height +
                        EntriesFont.Height;
                int WindowWidth = TotalWidth;
                SetClientSizeCore(WindowWidth, WindowHeight);
            }

            DrawingTick = new System.Windows.Forms.Timer();
            DrawingTick.Tick += DrawingTick_Tick;
            DrawingTick.Interval = 200;
            DrawingTick.Start();
        }

        void RunRoll_Load(object sender, EventArgs e)
        {
            Location = Settings.Default.RollWindowLocation;
        }

        void RunRoll_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.RollWindowLocation = Location;
            Settings.Default.Save();
            DrawingTick.Stop();
        }

        void RunRoll_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle RollerRec = new Rectangle(
                    LeftMargin,
                    TopMargin + TitleFont.Height + RollerTop,
                    TotalWidth - LeftMargin - RightMargin,
                    RollerFont.Height
                );

            if (RollerRec.Contains(e.X, e.Y))
            {
                Cursor = Cursors.Hand;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }

        void RunRoll_MouseDown(object sender, MouseEventArgs e)
        {

            Rectangle RollerRec = new Rectangle(
                    LeftMargin,
                    TopMargin + TitleFont.Height + RollerTop,
                    TotalWidth - LeftMargin - RightMargin,
                    RollerFont.Height
                );

            if (RollerRec.Contains(e.X, e.Y))
            {
                if (EndTime == DateTime.MinValue) EndTime = DateTime.Now;


                if (Stage == STAGE_WAITING || Stage == STAGE_POSTROLL)
                {
                    using (DataStore DS = Program.getSelectedDataStore())
                    {
                        DataSetSelection DSS = new DataSetSelection();
                        DSS.Start = StartTime;
                        DSS.End = EndTime;
                        EntryList = DS.GetUniqueUsersString(DSS);
                        if (EntryList.Count == 0) EntryList.Add("No Entries :-(");
                    }
                    Program.Shuffle(EntryList);
                    TotalEntriesFound = Math.Min(EntryList.Count, 100);
                    CurrentEntryIndex = 0;
                    DrawingTick.Interval = 1;
                    Stage = STAGE_ROLLING;
                    if (!DrawingTick.Enabled)
                        DrawingTick.Enabled = true;

                }
            }
        }



        void RunRoll_Paint(object sender, PaintEventArgs e)
        {
            switch (Stage)
            {
                case STAGE_WAITING:
                    PaintWaiting();
                    break;
                case STAGE_ROLLING:
                    PaintRolling();
                    break;
                case STAGE_POSTROLL:
                    PaintPostroll();
                    break;
                default:
                    Close();
                    break;
            }
        }

        private void PaintPostroll()
        {
            using (Graphics Graphic = CreateGraphics())
            {
                StringFormat SF = new StringFormat();
                SF.Alignment = StringAlignment.Center;
                SF.LineAlignment = StringAlignment.Center;

                Rectangle TitleRec = new Rectangle(
                        LeftMargin,
                        TopMargin,
                        TotalWidth - LeftMargin - RightMargin,
                        TitleFont.Height
                    );
                Rectangle RollerRec = new Rectangle(
                        LeftMargin,
                        TitleRec.Bottom + RollerTop,
                        TotalWidth - LeftMargin - RightMargin,
                        RollerFont.Height
                    );
                Rectangle EntriesRec = new Rectangle(
                        LeftMargin,
                        RollerRec.Bottom + EntriesTop,
                        TotalWidth - LeftMargin - RightMargin,
                        EntriesFont.Height
                    );

                Graphic.DrawString(TitleText, TitleFont, TitleBrush, TitleRec, SF);
                Graphic.DrawString(EntryList[CurrentEntryIndex - 1], RollerFont, RollerBrush, RollerRec, SF);
                Graphic.DrawString("Of " + Count.ToString() + ", you won!", EntriesFont, EntriesBrush, EntriesRec, SF);
            }
            DrawingTick.Enabled = false;
        }

        private void PaintRolling()
        {
            using (Graphics Graphic = CreateGraphics())
            {
                StringFormat SF = new StringFormat();
                SF.Alignment = StringAlignment.Center;
                SF.LineAlignment = StringAlignment.Center;

                Rectangle TitleRec = new Rectangle(
                        LeftMargin,
                        TopMargin,
                        TotalWidth - LeftMargin - RightMargin,
                        TitleFont.Height
                    );
                Rectangle RollerRec = new Rectangle(
                        LeftMargin,
                        TitleRec.Bottom + RollerTop,
                        TotalWidth - LeftMargin - RightMargin,
                        RollerFont.Height
                    );
                Rectangle EntriesRec = new Rectangle(
                        LeftMargin,
                        RollerRec.Bottom + EntriesTop,
                        TotalWidth - LeftMargin - RightMargin,
                        EntriesFont.Height
                    );

                Graphic.DrawString(TitleText, TitleFont, TitleBrush, TitleRec, SF);
                Graphic.DrawString(EntryList[CurrentEntryIndex - 1], RollerFont, RollerBrush, RollerRec, SF);
                Graphic.DrawString("Rolling Through " + Count.ToString() + " Entries", EntriesFont, EntriesBrush, EntriesRec, SF);
            }
        }

        void PaintWaiting()
        {
            using (Graphics Graphic = CreateGraphics())
            {
                StringFormat SF = new StringFormat();
                SF.Alignment = StringAlignment.Center;
                SF.LineAlignment = StringAlignment.Center;

                Rectangle TitleRec = new Rectangle(
                        LeftMargin,
                        TopMargin,
                        TotalWidth - LeftMargin - RightMargin,
                        TitleFont.Height
                    );
                Rectangle RollerRec = new Rectangle(
                        LeftMargin,
                        TitleRec.Bottom + RollerTop,
                        TotalWidth - LeftMargin - RightMargin,
                        RollerFont.Height
                    );
                Rectangle EntriesRec = new Rectangle(
                        LeftMargin,
                        RollerRec.Bottom + EntriesTop,
                        TotalWidth - LeftMargin - RightMargin,
                        EntriesFont.Height
                    );

                Graphic.DrawString(TitleText, TitleFont, TitleBrush, TitleRec, SF);
                Graphic.DrawString(RollerText, RollerFont, RollerBrush, RollerRec, SF);
                Graphic.DrawString("Total Entries: " + Count.ToString(), EntriesFont, EntriesBrush, EntriesRec, SF);
            }

        }

        void DrawingTick_Tick(object sender, EventArgs e)
        {
            switch (Stage)
            {
                case STAGE_WAITING:
                    using (DataStore DS = Program.getSelectedDataStore())
                    {
                        DataSetSelection DSS = new DataSetSelection();
                        DSS.Start = StartTime;
                        try
                        {
                            Count = DS.GetUniqueUsersCount(DSS);
                        }
                        catch (Exception ex)
                        {
                            Log.LogException(ex);
                        }
                    }
                    break;
                case STAGE_ROLLING:
                    DrawingTick.Interval = (int)Math.Pow(ROLLING_MAGIC_NUM,(CurrentEntryIndex*20)/TotalEntriesFound);
                    CurrentEntryIndex++;
                    if (CurrentEntryIndex == TotalEntriesFound - 1)
                    {
                        Stage = STAGE_POSTROLL;
                    }
                    break;
            }
            Invalidate();
        }

        private void SetupVars()
        {
            LeftMargin = Settings.Default.RollLeftMargin;
            RightMargin = Settings.Default.RollRightMargin;
            TopMargin = Settings.Default.RollTopMargin;
            BottomMargin = Settings.Default.RollBottomMargin;
            TotalWidth = Settings.Default.RollTotalWidth;

            TitleFont = Settings.Default.RollTitleFont;
            RollerFont = Settings.Default.RollRollerFont;
            EntriesFont = Settings.Default.RollEntriesFont;

            TitleColor = Settings.Default.RollTitleColor;
            RollerColor = Settings.Default.RollRollerColor;
            EntriesColor = Settings.Default.RollEntriesColor;
            ChromaKey = Settings.Default.RollChromaKey;

            RollerTop = Settings.Default.RollRollerTop;
            EntriesTop = Settings.Default.RollEntriesTop;

            RollerText = Settings.Default.RollRollerText;
            TitleText = Settings.Default.RollTitleText;

            TitleBrush = new SolidBrush(TitleColor);
            RollerBrush = new SolidBrush(RollerColor);
            EntriesBrush = new SolidBrush(EntriesColor);

        }
    }
}
