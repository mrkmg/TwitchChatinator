using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TwitchChatinator.Forms.Components;
using TwitchChatinator.Forms.Launchers;
using TwitchChatinator.Libs;
using TwitchChatinator.Options;

namespace TwitchChatinator.Forms.Runners
{
    public partial class RunPollBar : PaintWindow
    {
        private SolidBrush[] _barBrushes;

        private Rectangle[] _barRectangles;
        private SolidBrush _countBrush;
        private readonly int _countEntries;

        private readonly PollData _data;

        private readonly Timer _drawingTick;

        private SolidBrush _labelBrush;
        private BarGraphOptions _options;
        private readonly string _optionsName;
        private readonly string _pollTitle;
        private StringFormat _sfCount;
        private StringFormat _sfLabel;
        private readonly DateTime _startTime;

        private StringFormat _titleStringFormat;
        private SolidBrush _totalBrush;

        private Rectangle _totalRec;
        private StringFormat _totalStringFormat;

        public RunPollBar(DateTime start, string name, string title, string[] pollvars)
        {
            InitializeComponent();

            Text = "Poll ( " + name + ") - Chatinator";

            _startTime = start;
            _optionsName = name;
            _pollTitle = title;
            _countEntries = pollvars.Count();
            _data = new PollData(_countEntries) {Options = pollvars};

            ReadOptions();

            SetupVars();

            SetClientSizeCore(_options.Width, _options.Height);
            BackColor = _options.ChromaKey;

            Paint += RunPollBar_Paint;
            Load += RunPollBar_Load;
            FormClosing += RunPollBar_FormClosing;
            Disposed += RunPollBar_Disposed;

            _drawingTick = new Timer();
            _drawingTick.Tick += DrawingTick_Tick;
            _drawingTick.Interval = 200;
            _drawingTick.Start();
        }

        private void RunPollBar_Disposed(object sender, EventArgs e)
        {
            _labelBrush.Dispose();
            _countBrush.Dispose();
            _totalBrush.Dispose();

            foreach (var b in _barBrushes)
            {
                b?.Dispose();
            }

            _drawingTick.Stop();
            _drawingTick.Dispose();

            _titleStringFormat.Dispose();
            _totalStringFormat.Dispose();
            _sfLabel.Dispose();
            _sfCount.Dispose();
        }

        private void RunPollBar_FormClosing(object sender, FormClosingEventArgs e)
        {
            PositionsSaver.Put("BAR:" + _optionsName, Location);
        }

        private void RunPollBar_Load(object sender, EventArgs e)
        {
            var p = PositionsSaver.Get("BAR:" + _optionsName);
            if (p != Point.Empty)
            {
                Location = p;
            }
        }

        private void RunPollBar_Paint(object sender, PaintEventArgs e)
        {
            if (_data == null)
            {
                return;
            }

            using (var graphic = CreateGraphics())
            {
                if (_options.BackgroundImage?.Image != null)
                {
                    graphic.DrawImage(_options.BackgroundImage.Image, new Point(0, 0));
                }

                graphic.DrawString(_pollTitle, _options.TitleFont, _totalBrush, _totalRec, _titleStringFormat);
                graphic.DrawString(_data.TotalVotes + " Votes", _options.TitleFont, _totalBrush, _totalRec,
                    _totalStringFormat);
                for (var i = 0; i < _countEntries; i++)
                {
                    if (_data.TotalVotes > 0)
                        _barRectangles[i].Width = (_options.Width - _options.MarginRight - _options.MarginLeft)*
                                                 _data.Amounts[i]/_data.TotalVotes;
                    else
                        _barRectangles[i].Width = 0;
                    graphic.FillRectangle(_barBrushes[i%4], _barRectangles[i]);
                    graphic.DrawString(_data.Options[i], _options.OptionFont, _labelBrush, _barRectangles[i], _sfLabel);
                    if (_data.Amounts[i] > 0)
                        graphic.DrawString(_data.Amounts[i].ToString(), _options.CountFont, _countBrush, _barRectangles[i],
                            _sfCount);
                }

                if (_options.ForegroundImage?.Image != null)
                {
                    graphic.DrawImage(_options.ForegroundImage.Image, new Point(0, 0));
                }
            }
        }

        private void GeneratePreviewData()
        {
            _data.TotalVotes = 0;
            for (var i = 0; i < _countEntries; i++)
            {
                _data.Amounts[i] = 100*(i + 1);
                _data.TotalVotes += 100*(i + 1);
            }
        }

        private void GetData()
        {
            if (_countEntries <= 0) return;

            var dss = new DataSetSelection {Start = _startTime};
            var rawData = DataStore.GetDataSet(dss);

            var totalRows = 0;
            var rowData = new int[_countEntries];
            var recordedUsers = new List<string>();

            foreach (DataRow row in rawData.Tables[0].Rows)
            {
                for (var i = 0; i < _countEntries; i++)
                {
                    if (!row.ItemArray[3].ToString().ToLower().Contains(_data.Options[i].ToLower()) ||
                        (!_options.AllowMulti && recordedUsers.Contains(row.ItemArray[2].ToString()))) continue;

                    recordedUsers.Add(row.ItemArray[2].ToString());
                    rowData[i]++;
                    totalRows++;
                    break;
                }
            }

            for (var i = 0; i < _countEntries; i++)
            {
                _data.Amounts[i] = rowData[i];
            }
            _data.TotalVotes = totalRows;
        }

        private void DrawingTick_Tick(object sender, EventArgs e)
        {
            if (_optionsName == "_preview")
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

        private void ReadOptions()
        {
            _options = BarGraphOptions.Load(_optionsName);
        }

        private void SetupVars()
        {
            _labelBrush = new SolidBrush(_options.OptionFontColor);
            _countBrush = new SolidBrush(_options.CountFontColor);
            _totalBrush = new SolidBrush(_options.TitleFontColor);

            _totalRec = new Rectangle
            {
                Height = _options.TitleFont.Height,
                Width = _options.Width - _options.MarginLeft - _options.MarginRight
            };


            _titleStringFormat = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip)
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };

            _totalStringFormat = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip)
            {
                Alignment = StringAlignment.Far,
                LineAlignment = StringAlignment.Center
            };

            _sfLabel = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip)
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.None
            };

            _sfCount = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip)
            {
                Alignment = StringAlignment.Far,
                LineAlignment = StringAlignment.Near
            };

            var offTop = 0;

            switch (_options.TotalPosition)
            {
                case "Top":
                    _totalRec.X = _options.MarginLeft;
                    _totalRec.Y = _options.MarginTop;
                    offTop = _totalRec.Height + _options.MarginTop;
                    break;
                case "Bottom":
                default:
                    _totalRec.X = _options.MarginLeft;
                    _totalRec.Y = _options.Height - _options.MarginBottom - _options.TitleFont.Height;
                    offTop = _options.MarginTop;
                    break;
            }

            _barRectangles = new Rectangle[_countEntries];

            var avaliableHeight = _options.Height - _totalRec.Height - _options.MarginBottom - _options.MarginTop -
                                  _countEntries*_options.BarSpacing;
            var barHeight = avaliableHeight/_countEntries;

            for (var i = 0; i < _countEntries; i++)
            {
                _barRectangles[i].X = _options.MarginLeft;
                _barRectangles[i].Y = offTop + barHeight*i + i*_options.BarSpacing;
                _barRectangles[i].Height = barHeight;
            }

            _barBrushes = new SolidBrush[4];
            _barBrushes[0] = new SolidBrush(_options.Option1Color);
            _barBrushes[1] = new SolidBrush(_options.Option2Color);
            _barBrushes[2] = new SolidBrush(_options.Option3Color);
            _barBrushes[3] = new SolidBrush(_options.Option4Color);
        }
    }
}