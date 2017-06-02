using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using TwitchChatinator.Forms.Components;
using TwitchChatinator.Libs;
using TwitchChatinator.Options;

namespace TwitchChatinator.Forms.Runners
{
    public sealed partial class RunGiveaway : PaintWindow
    {
        private const short StageWaiting = 0;
        private const short StageRolling = 1;
        private const short StagePostroll = 2;
//        private const short StageDie = 255;

        private const double RollingMagicNum = 1.35;
        private const int RollingEntriesCount = 100;

        private int _count;
        private int _currentEntryIndex;
        private readonly Timer _drawingTick;
        private DateTime _endTime;
        private SolidBrush _entriesBrush;
        private Rectangle _entriesRec;
        private List<string> _entryList;

        private readonly string _giveawayTitle;
        private GiveawayOptions _options;
        private readonly string _optionsName;
        private SolidBrush _rollerBrush;
        private Rectangle _rollerRec;

        private int _stage = StageWaiting;

        private DateTime _startTime;

        private string _keyword;

        private SolidBrush _titleBrush;

        private Rectangle _titleRec;

        public RunGiveaway(DateTime startTime, string name, string title, string keyword)
        {
            InitializeComponent();

            Text = @"Giveaway (" + name + @") - Chatinator";

            _startTime = startTime;
            _giveawayTitle = title;
            _optionsName = name;
            _keyword = keyword;

            ReadOptions();
            SetupVars();

            Text = @"Giveaway - Chatinator";
            BackColor = _options.ChromaKey;

            Paint += RunRoll_Paint;
            FormClosing += RunRoll_FormClosing;
            Load += RunRoll_Load;

            _drawingTick = new Timer();
            _drawingTick.Tick += DrawingTick_Tick;
            _drawingTick.Interval = 200;
            _drawingTick.Start();
        }

        private void ReadOptions()
        {
            _options = GiveawayOptions.Load(_optionsName);
        }

        private void RunRoll_Load(object sender, EventArgs e)
        {
            var p = PositionsSaver.Get("GIVEAWAY:" + _optionsName);
            if (p != Point.Empty)
            {
                Location = p;
            }
        }

        private void RunRoll_FormClosing(object sender, FormClosingEventArgs e)
        {
            PositionsSaver.Put("GIVEAWAY:" + _optionsName, Location);
            _drawingTick.Stop();
        }

        private void RunRoll_Paint(object sender, PaintEventArgs e)
        {
            switch (_stage)
            {
                case StageWaiting:
                    PaintWaiting();
                    break;
                case StageRolling:
                    PaintRolling();
                    break;
                case StagePostroll:
                    PaintPostroll();
                    break;
                default:
                    Close();
                    break;
            }
        }

        private void PaintScreen(string title, string roller, string entries)
        {
            using (var graphic = CreateGraphics())
            {
                graphic.TextRenderingHint = TextRenderingHint.AntiAlias;

                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                if (_options.BackgroundImage?.Image != null)
                {
                    graphic.DrawImage(_options.BackgroundImage.Image, 0, 0, _options.BackgroundImage.Image.Width, _options.BackgroundImage.Image.Height);
                }

                graphic.DrawString(title, _options.TitleFont, _titleBrush, _titleRec, sf);
                graphic.DrawString(roller, _options.RollerFont, _rollerBrush, _rollerRec, sf);
                graphic.DrawString(entries, _options.EntriesFont, _entriesBrush, _entriesRec, sf);

                if (_options.ForegroundImage?.Image != null)
                {
                    graphic.DrawImage(_options.ForegroundImage.Image, 0, 0, _options.ForegroundImage.Image.Width, _options.ForegroundImage.Image.Height);
                }
            }
        }

        private void PaintPostroll()
        {
            PaintScreen(_giveawayTitle, _entryList[_currentEntryIndex%_entryList.Count], "Of " + _count + ", you won!");
            _drawingTick.Enabled = false;
        }

        private void PaintRolling()
        {
            PaintScreen(_giveawayTitle, _entryList[_currentEntryIndex%_entryList.Count], "Rolling!!!");
        }

        private void PaintWaiting()
        {
            PaintScreen(_giveawayTitle, "Taking Submissions", "Total Entries: " + _count);
        }

        private void UpdateCount()
        {
            if (_optionsName == "_preview")
            {
                _count = 5000;
                return;
            }

            var dss = new DataSetSelection {Start = _startTime, MessagePartial = _keyword};
            try
            {
                _count = DataStore.GetUniqueUsersCount(dss);
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
            }
        }

        private void GetEntries()
        {
            if (_optionsName == "_preview")
            {
                _entryList = new List<string>();
                for (var i = 1; i <= 5000; i++) _entryList.Add("User" + i);
                return;
            }

            var dss = new DataSetSelection
            {
                Start = _startTime,
                End = _endTime,
                MessagePartial = _keyword
            };
            _entryList = DataStore.GetUniqueUsersString(dss);
        }

        public void Roll()
        {
            if (_endTime == DateTime.MinValue) _endTime = DateTime.Now;

            if (_stage == StageWaiting || _stage == StagePostroll)
            {
                GetEntries();
                if (_entryList.Count == 0) _entryList.Add("No Entries :-(");
                _entryList.Shuffle();
                _currentEntryIndex = 0;
                _drawingTick.Interval = 1;
                _stage = StageRolling;
                if (!_drawingTick.Enabled)
                    _drawingTick.Enabled = true;
            }
        }

        private void DrawingTick_Tick(object sender, EventArgs e)
        {
            switch (_stage)
            {
                case StageWaiting:
                    UpdateCount();
                    break;
                case StageRolling:
                    if (_currentEntryIndex == RollingEntriesCount - 1)
                    {
                        _stage = StagePostroll;
                        Invalidate();
                        return;
                    }
                    _drawingTick.Interval = Math.Max(64,
                        // ReSharper disable once PossibleLossOfFraction
                        (int) Math.Pow(RollingMagicNum, _currentEntryIndex*20/RollingEntriesCount));
                    _currentEntryIndex++;
                    if (_currentEntryIndex == RollingEntriesCount - 1)
                    {
                        _drawingTick.Interval = 1000;
                    }
                    break;
            }
            Invalidate();
        }

        private void SetupVars()
        {
            _titleBrush = new SolidBrush(_options.TitleFontColor);
            _rollerBrush = new SolidBrush(_options.RollerFontColor);
            _entriesBrush = new SolidBrush(_options.EntriesFontColor);

            _titleRec = new Rectangle(
                _options.MarginLeft,
                _options.MarginTop,
                _options.Width - _options.MarginLeft - _options.MarginRight,
                _options.TitleFont.Height
                );
            _rollerRec = new Rectangle(
                _options.MarginLeft,
                _titleRec.Bottom + _options.Spacing,
                _options.Width - _options.MarginLeft - _options.MarginRight,
                _options.RollerFont.Height
                );
            _entriesRec = new Rectangle(
                _options.MarginLeft,
                _rollerRec.Bottom + _options.Spacing,
                _options.Width - _options.MarginLeft - _options.MarginRight,
                _options.EntriesFont.Height
                );

            var windowHeight = _entriesRec.Bottom + _options.MarginBottom;
            var windowWidth = _options.Width;
            SetClientSizeCore(windowWidth, windowHeight);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }

            _titleBrush.Dispose();
            _entriesBrush.Dispose();
            _rollerBrush.Dispose();

            base.Dispose(disposing);
        }
    }
}