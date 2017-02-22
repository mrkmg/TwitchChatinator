﻿using System;
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
    public partial class RunGiveaway : PaintWindow
    {
        const short STAGE_WAITING = 0;
        const short STAGE_ROLLING = 1;
        const short STAGE_POSTROLL = 2;
        const short STAGE_DIE = 255;

        const double ROLLING_MAGIC_NUM = 1.35;
        const int ROLLING_ENTRIES_COUNT = 100;

        SolidBrush TitleBrush;
        SolidBrush RollerBrush;
        SolidBrush EntriesBrush;

        Rectangle TitleRec;
        Rectangle RollerRec;
        Rectangle EntriesRec;

        DateTime StartTime;
        DateTime EndTime;

        string GiveawayTitle;
        string OptionsName;
        GiveawayOptions Options;

        int Stage = STAGE_WAITING;
        System.Windows.Forms.Timer DrawingTick;

        int Count = 0;
        List<string> EntryList;
        int CurrentEntryIndex;
        int TotalEntriesFound;

        public RunGiveaway(DateTime startTime, string name, string title)
        {
            InitializeComponent();

            Text = "Giveaway (" + name + ") - Chatinator";

            StartTime = startTime;
            GiveawayTitle = title;
            OptionsName = name;

            ReadOptions();
            SetupVars();

            this.Text = "Roller - Chatinator";
            this.BackColor = Options.ChromaKey;

            this.Paint += RunRoll_Paint;
            this.FormClosing += RunRoll_FormClosing;
            this.Load += RunRoll_Load;

            DrawingTick = new System.Windows.Forms.Timer();
            DrawingTick.Tick += DrawingTick_Tick;
            DrawingTick.Interval = 200;
            DrawingTick.Start();
        }

        void ReadOptions()
        {
            Options = GiveawayOptions.Load(OptionsName);
        }

        void RunRoll_Load(object sender, EventArgs e)
        {
            Point p = PositionsSaver.get("GIVEAWAY:" + OptionsName);
            if (p != Point.Empty)
            {
                Location = p;
            }
        }

        void RunRoll_FormClosing(object sender, FormClosingEventArgs e)
        {
            PositionsSaver.put("GIVEAWAY:" + OptionsName, Location);
            DrawingTick.Stop();
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

        private void PaintScreen(string Title, string Roller, string Entries)
        {
            using (Graphics Graphic = CreateGraphics())
            {
                StringFormat SF = new StringFormat();
                SF.Alignment = StringAlignment.Center;
                SF.LineAlignment = StringAlignment.Center;

                if (Options.BackgroundImage != null && Options.BackgroundImage.Image != null)
                {
                    Graphic.DrawImage(Options.BackgroundImage.Image, new Point(0, 0));
                }

                Graphic.DrawString(Title, Options.TitleFont, TitleBrush, TitleRec, SF);
                Graphic.DrawString(Roller, Options.RollerFont, RollerBrush, RollerRec, SF);
                Graphic.DrawString(Entries, Options.EntriesFont, EntriesBrush, EntriesRec, SF);

                if (Options.ForegroundImage != null && Options.ForegroundImage.Image != null)
                {
                    Graphic.DrawImage(Options.ForegroundImage.Image, new Point(0, 0));
                }
            }
        }

        void PaintPostroll()
        {
            PaintScreen(GiveawayTitle, EntryList[CurrentEntryIndex % (EntryList.Count())], "Of " + Count.ToString() + ", you won!");
            DrawingTick.Enabled = false;
        }

        void PaintRolling()
        {
            PaintScreen(GiveawayTitle, EntryList[CurrentEntryIndex % (EntryList.Count())], "Rolling!!!");
        }

        void PaintWaiting()
        {
            PaintScreen(GiveawayTitle, "Taking Submissions", "Total Entries: " + Count.ToString());
        }

        void UpdateCount()
        {
            if (OptionsName == "_preview")
            {
                Count = 5000;
                return;
            }

            DataSetSelection DSS = new DataSetSelection();
            DSS.Start = StartTime;
            try
            {
                Count = DataStore.GetUniqueUsersCount(DSS);
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
            }
        }

        void GetEntries()
        {
            if (OptionsName == "_preview")
            {
                EntryList = new List<string>();
                for (var i = 1; i <= 5000; i++) EntryList.Add("User" + i);
                return;
            }

            DataSetSelection DSS = new DataSetSelection();
            DSS.Start = StartTime;
            DSS.End = EndTime;
            EntryList = DataStore.GetUniqueUsersString(DSS);
        }

        public void Roll()
        {
            if (EndTime == DateTime.MinValue) EndTime = DateTime.Now;

            if (Stage == STAGE_WAITING || Stage == STAGE_POSTROLL)
            {
                GetEntries();
                if (EntryList.Count == 0) EntryList.Add("No Entries :-(");
                EntryList.Shuffle();
                TotalEntriesFound = Math.Min(EntryList.Count, ROLLING_ENTRIES_COUNT);
                CurrentEntryIndex = 0;
                DrawingTick.Interval = 1;
                Stage = STAGE_ROLLING;
                if (!DrawingTick.Enabled)
                    DrawingTick.Enabled = true;
            }
        }

        void DrawingTick_Tick(object sender, EventArgs e)
        {
            switch (Stage)
            {
                case STAGE_WAITING:
                    UpdateCount();
                    break;
                case STAGE_ROLLING:
                    DrawingTick.Interval = Math.Max(16, (int)Math.Pow(ROLLING_MAGIC_NUM, (CurrentEntryIndex * 20) / ROLLING_ENTRIES_COUNT));
                    CurrentEntryIndex++;
                    if (CurrentEntryIndex == ROLLING_ENTRIES_COUNT - 1)
                    {
                        Stage = STAGE_POSTROLL;
                    }
                    break;
            }
            Invalidate();
        }

        void SetupVars()
        {
            TitleBrush = new SolidBrush(Options.TitleFontColor);
            RollerBrush = new SolidBrush(Options.RollerFontColor);
            EntriesBrush = new SolidBrush(Options.EntriesFontColor);

            TitleRec = new Rectangle(
                        Options.MarginLeft,
                        Options.MarginTop,
                        Options.Width - Options.MarginLeft - Options.MarginRight,
                        Options.TitleFont.Height
                    );
            RollerRec = new Rectangle(
                    Options.MarginLeft,
                    TitleRec.Bottom + Options.Spacing,
                    Options.Width - Options.MarginLeft - Options.MarginRight,
                    Options.RollerFont.Height
                );
            EntriesRec = new Rectangle(
                    Options.MarginLeft,
                    RollerRec.Bottom + Options.Spacing,
                    Options.Width - Options.MarginLeft - Options.MarginRight,
                    Options.EntriesFont.Height
                );

            int WindowHeight = EntriesRec.Bottom + Options.MarginBottom;
            int WindowWidth = Options.Width;
            SetClientSizeCore(WindowWidth, WindowHeight);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            TitleBrush.Dispose();
            EntriesBrush.Dispose();
            RollerBrush.Dispose();

            base.Dispose(disposing);
        }
    }
}
