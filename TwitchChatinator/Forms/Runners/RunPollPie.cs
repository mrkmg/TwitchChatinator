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
    public sealed partial class RunPollPie : PaintWindow
    {
        private SolidBrush _countBrush;
        private readonly int _countEntries;

        private readonly PollData _data;

        private readonly Timer _drawingTick;

        private SolidBrush _labelBrush;
        private PieGraphOptions _options;
        private readonly string _optionsName;

        private SolidBrush[] _pieBrushes;
        private Rectangle _pieRec;
        private readonly string _pollTitle;
        private StringFormat _sfCount;
        private StringFormat _sfLabel;
        private readonly DateTime _startTime;

        private StringFormat _titleStringFormat;
        private SolidBrush _totalBrush;

        private Rectangle _totalRec;
        private StringFormat _totalStringFormat;

        public RunPollPie(DateTime start, string name, string title, string[] pollvars)
        {
            InitializeComponent();

            Text = @"Poll ( " + name + @") - Chatinator";

            _startTime = start;
            _optionsName = name;
            _pollTitle = title;
            _countEntries = pollvars.Length;
            _data = new PollData(_countEntries) {Options = pollvars};

            ReadOptions();

            SetupVars();

            SetClientSizeCore(_options.Width, _options.Height);
            BackColor = _options.ChromaKey;

            Paint += RunPollPie_Paint;
            Load += RunPollPie_Load;
            FormClosing += RunPollPie_FormClosing;
            Disposed += RunPollPie_Disposed;

            _drawingTick = new Timer();
            _drawingTick.Tick += DrawingTick_Tick;
            _drawingTick.Interval = 200;
            _drawingTick.Start();
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

            foreach (var row in rawData)
            {
                for (var i = 0; i < _countEntries; i++)
                {
                    if (!row.Message.ToLower().Contains(_data.Options[i].ToLower())) continue;
                    if (!_options.AllowMulti && recordedUsers.Contains(row.Username)) continue;
                    recordedUsers.Add(row.Username);
                    rowData[i]++;
                    totalRows++;
                    break;
                }
            }

//            foreach (DataRow row in rawData.Tables[0].Rows)
//            {
//                for (var i = 0; i < _countEntries; i++)
//                {
//                    if (!row.ItemArray[3].ToString().ToLower().Contains(_data.Options[i].ToLower()) ||
//                        (!_options.AllowMulti && recordedUsers.Contains(row.ItemArray[2].ToString()))) continue;
//
//                    recordedUsers.Add(row.ItemArray[2].ToString());
//                    rowData[i]++;
//                    totalRows++;
//                    break;
//                }
//            }

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

        private void RunPollPie_Disposed(object sender, EventArgs e)
        {
            _labelBrush.Dispose();
            _countBrush.Dispose();
            _totalBrush.Dispose();

            foreach (var b in _pieBrushes)
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

        private void RunPollPie_FormClosing(object sender, FormClosingEventArgs e)
        {
            PositionsSaver.Put("PIE:" + _optionsName, Location);
        }

        private void RunPollPie_Load(object sender, EventArgs e)
        {
            var p = PositionsSaver.Get("PIE:" + _optionsName);
            if (p != Point.Empty)
            {
                Location = p;
            }
        }

        private void RunPollPie_Paint(object sender, PaintEventArgs e)
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

                if (_data.TotalVotes > 0)
                {
                    // http://www.codeproject.com/Articles/463284/Create-Pie-Chart-Using-Graphics-in-Csharp-NET
                    // Thank you [hari19113](http://www.codeproject.com/script/Membership/View.aspx?mid=8124215)
                    var degrees = new float[_data.Options.Length];

                    var labelsToDraw = new List<LabelToDraw>();

                    for (var i = 0; i < _countEntries; i++)
                    {
                        var sum = degrees.Sum();
                        degrees[i] = _data.Amounts[i]/(float) _data.TotalVotes*360;
                        graphic.FillPie(_pieBrushes[i%4], _pieRec, sum, degrees[i]);

                        if (_data.Amounts[i] <= 0) continue;

                        var optionWidth = graphic.MeasureString(_data.Options[i], _options.OptionFont).Width;
                        var titleWidth = graphic.MeasureString("(" + _data.Amounts[i] + ")", _options.CountFont).Width;
                        var labelHeight = Math.Max(_options.TitleFont.Height, _options.CountFont.Height);
                        var textPoint = PointOnCircle((float) _pieRec.Width/3, sum + degrees[i]/2,
                            new PointF(_pieRec.X + _pieRec.Width/2, _pieRec.Y + _pieRec.Height/2));
                        textPoint.X -= (optionWidth + titleWidth)/2;
                        // ReSharper disable once PossibleLossOfFraction
                        textPoint.Y -= labelHeight/2;
                        labelsToDraw.Add(new LabelToDraw(_data.Options[i], _options.OptionFont, _labelBrush, textPoint));
                        var countPoint = new PointF(textPoint.X + optionWidth, textPoint.Y);
                        labelsToDraw.Add(new LabelToDraw("(" + _data.Amounts[i] + ")", _options.CountFont, _countBrush,
                            countPoint));
                    }

                    foreach (var l in labelsToDraw)
                    {
                        l.Draw(graphic);
                    }
                }

                if (_options.ForegroundImage?.Image != null)
                {
                    graphic.DrawImage(_options.ForegroundImage.Image, new Point(0, 0));
                }
            }
        }

        //http://stackoverflow.com/questions/839899/how-do-i-calculate-a-point-on-a-circle-s-circumference
        // ReSharper disable once CommentTypo
        //Thank you [Justin Ethier](http://stackoverflow.com/users/101258/justin-ethier)
        private static PointF PointOnCircle(float radius, float angleInDegrees, PointF origin)
        {
            // Convert from degrees to radians via multiplication by PI/180        
            var x = (float) (radius*Math.Cos(angleInDegrees*Math.PI/180F)) + origin.X;
            var y = (float) (radius*Math.Sin(angleInDegrees*Math.PI/180F)) + origin.Y;

            return new PointF(x, y);
        }

        private void ReadOptions()
        {
            _options = PieGraphOptions.Load(_optionsName);
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
                LineAlignment = StringAlignment.Center
            };

            _sfCount = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip)
            {
                Alignment = StringAlignment.Far,
                LineAlignment = StringAlignment.Near
            };

            int offTop;

            switch (_options.TotalPosition)
            {
                case "Top":
                    _totalRec.X = _options.MarginLeft;
                    _totalRec.Y = _options.MarginTop;
                    offTop = _options.TitleFont.Height;
                    break;
                //case "Bottom":
                default:
                    _totalRec.X = _options.MarginLeft;
                    _totalRec.Y = _options.Height - _options.MarginBottom - _options.TitleFont.Height;
                    offTop = 0;
                    break;
            }

            var avaliableHeight = _options.Height - _options.MarginTop - _options.MarginBottom - _options.TitleFont.Height;
            var avaliableWidth = _options.Width - _options.MarginLeft - _options.MarginRight;


            _pieRec = new Rectangle();

            if (avaliableHeight > avaliableWidth)
            {
                var diff = avaliableHeight - avaliableWidth;

                _pieRec.X = _options.MarginTop;
                _pieRec.Y = _options.MarginLeft + diff/2 + offTop;
                _pieRec.Width = _options.Width - _options.MarginLeft - _options.MarginRight;
                _pieRec.Height = _options.Height - _options.MarginTop - _options.MarginBottom - _options.TitleFont.Height -
                                diff;
            }
            else if (avaliableWidth > avaliableHeight)
            {
                var diff = avaliableWidth - avaliableHeight;

                _pieRec.X = _options.MarginTop + diff/2;
                _pieRec.Y = _options.MarginLeft + offTop;
                _pieRec.Width = _options.Width - _options.MarginLeft - _options.MarginRight - diff;
                _pieRec.Height = _options.Height - _options.MarginTop - _options.MarginBottom - _options.TitleFont.Height;
            }
            else
            {
                _pieRec.X = _options.MarginLeft;
                _pieRec.Y = _options.MarginTop + offTop;
                _pieRec.Height = _options.Height - _options.MarginTop - _options.MarginBottom - _options.TitleFont.Height;
                _pieRec.Width = _options.Width - _options.MarginLeft - _options.MarginRight;
            }

            _pieBrushes = new SolidBrush[4];
            _pieBrushes[0] = new SolidBrush(_options.Option1Color);
            _pieBrushes[1] = new SolidBrush(_options.Option2Color);
            _pieBrushes[2] = new SolidBrush(_options.Option3Color);
            _pieBrushes[3] = new SolidBrush(_options.Option4Color);
        }
    }

    internal class LabelToDraw
    {
        private readonly Brush _brush;
        private readonly Font _font;
        private readonly PointF _point;
        private readonly string _text;

        public LabelToDraw(string t, Font f, Brush b, PointF p)
        {
            _text = t;
            _font = f;
            _brush = b;
            _point = p;
        }

        public void Draw(Graphics g)
        {
            g.DrawString(_text, _font, _brush, _point);
        }
    }
}