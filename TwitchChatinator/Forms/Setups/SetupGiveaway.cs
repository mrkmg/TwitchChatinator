using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TwitchChatinator.Forms.Runners;
using TwitchChatinator.Options;

namespace TwitchChatinator.Forms.Setups
{
    public partial class SetupGiveaway : Form
    {
        private Font _entriesFont;
        private GiveawayOptions _options;
        private readonly string _optionsName;

        private Font _rollerFont;

        private RunGiveaway _runGiveAway;
        private Font _titleFont;

        public SetupGiveaway(string n)
        {
            _optionsName = n;

            InitializeComponent();
            Populate();
            CalculateAndShowHeight();

            Closed += SetupGiveaway_Closed;

            ChangeBackgroundImage.Text = _options.BackgroundImage.Image == null ? "Load Image" : "Remove Image";
            ChangeForegroundImage.Text = _options.ForegroundImage.Image == null ? "Load Image" : "Remove Image";

            ChromaKey.Click += ColorClickHandler;
            EntriesFontColor.Click += ColorClickHandler;
            RollerFontColor.Click += ColorClickHandler;
            TitleFontColor.Click += ColorClickHandler;

            EntriesFontSelector.Click += EntriesFontSelector_Click;
            RollerFontSelector.Click += RollerFontSelector_Click;
            TitleFontSelector.Click += TitleFontSelector_Click;

            MarginTop.ValueChanged += ValueChanged;
            MarginBottom.ValueChanged += ValueChanged;
            EntriesFontSelector.FontChanged += ValueChanged;
            RollerFontSelector.FontChanged += ValueChanged;
            TitleFontSelector.FontChanged += ValueChanged;
            Spacing.ValueChanged += ValueChanged;

            CancelButton.Click += CancelButton_Click;
            SaveButton.Click += SaveButton_Click;
            TestButton.Click += TestButton_Click;

            WidthInput.ValueChanged += EventHandler_Change;

            MarginTop.ValueChanged += EventHandler_Change;
            MarginBottom.ValueChanged += EventHandler_Change;
            MarginLeft.ValueChanged += EventHandler_Change;
            MarginRight.ValueChanged += EventHandler_Change;
            Spacing.ValueChanged += EventHandler_Change;

            ChromaKey.BackColorChanged += EventHandler_Change;
            EntriesFontColor.BackColorChanged += EventHandler_Change;
            RollerFontColor.BackColorChanged += EventHandler_Change;
            TitleFontColor.BackColorChanged += EventHandler_Change;

            RefreshPreview();
        }

        private void SetupGiveaway_Closed(object sender, EventArgs e)
        {
            if (_runGiveAway != null && !_runGiveAway.IsDisposed)
            {
                _runGiveAway.Close();
            }
        }

        private void EventHandler_Change(object sender, EventArgs e)
        {
            RefreshPreview();
        }

        private void RefreshPreview()
        {
            if (_runGiveAway != null && !_runGiveAway.IsDisposed)
            {
                _runGiveAway.Close();
            }

            SetOptionsToValues();
            GiveawayOptions.PreviewOptions = _options;

            _runGiveAway = new RunGiveaway(DateTime.MinValue, "_preview", "Demo Giveaway");
            _runGiveAway.Show();
            Focus();
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            CalculateAndShowHeight();
        }

        private void SetOptionsToValues()
        {
            _options.Width = (int) WidthInput.Value;

            _options.MarginTop = (int) MarginTop.Value;
            _options.MarginBottom = (int) MarginBottom.Value;
            _options.MarginLeft = (int) MarginLeft.Value;
            _options.MarginRight = (int) MarginRight.Value;
            _options.Spacing = (int) Spacing.Value;

            _options.ChromaKey = ChromaKey.BackColor;

            _options.RollerFont = _rollerFont;
            _options.RollerFontColor = EntriesFontColor.BackColor;

            _options.EntriesFont = _entriesFont;
            _options.EntriesFontColor = RollerFontColor.BackColor;

            _options.TitleFont = _titleFont;
            _options.TitleFontColor = TitleFontColor.BackColor;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SetOptionsToValues();

            _options.Save(_optionsName);
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            _runGiveAway?.Roll();
        }

        private void CalculateAndShowHeight()
        {
            HeightInput.Value = MarginTop.Value + MarginBottom.Value + _titleFont.Height + _rollerFont.Height +
                                _entriesFont.Height + Spacing.Value*2;
        }

        private void Populate()
        {
            _options = GiveawayOptions.Load(_optionsName);

            NameLabel.Text = _optionsName;

            WidthInput.Value = _options.Width;

            MarginTop.Value = _options.MarginTop;
            MarginBottom.Value = _options.MarginBottom;
            MarginLeft.Value = _options.MarginLeft;
            MarginRight.Value = _options.MarginRight;
            Spacing.Value = _options.Spacing;

            ChromaKey.BackColor = _options.ChromaKey;

            _rollerFont = _options.RollerFont;
            RollerFontColor.BackColor = _options.RollerFontColor;

            _entriesFont = _options.EntriesFont;
            EntriesFontColor.BackColor = _options.EntriesFontColor;

            _titleFont = _options.TitleFont;
            TitleFontColor.BackColor = _options.TitleFontColor;
        }

        private void ColorClickHandler(object sender, EventArgs e)
        {
            var T = (TextBox) sender;
            using (var cd = new ColorDialog())
            {
                cd.Color = T.BackColor;
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    T.BackColor = cd.Color;
                }
            }

            SaveButton.Focus();
        }

        private void EntriesFontSelector_Click(object sender, EventArgs e)
        {
            using (var fd = new FontDialog())
            {
                fd.Font = _entriesFont;
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    _entriesFont = fd.Font;
                    RefreshPreview();
                }
            }
        }

        private void RollerFontSelector_Click(object sender, EventArgs e)
        {
            using (var fd = new FontDialog())
            {
                fd.Font = _rollerFont;
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    _rollerFont = fd.Font;
                    RefreshPreview();
                }
            }
        }

        private void TitleFontSelector_Click(object sender, EventArgs e)
        {
            using (var fd = new FontDialog())
            {
                fd.Font = _titleFont;
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    _titleFont = fd.Font;
                    RefreshPreview();
                }
            }
        }

        private void ChangeBackgroundImage_Click(object sender, EventArgs e)
        {
            if (_options.BackgroundImage.Image == null)
            {
                FileDialog fd = new OpenFileDialog();
                fd.Filter = @"PNG Files (*.png)|*.png";
                fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                fd.Title = @"Choose A Background Image";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    _options.BackgroundImage.Name = Path.GetFileNameWithoutExtension(fd.FileName);
                    _options.BackgroundImage.Image = Image.FromFile(fd.FileName);
                    ((Button) sender).Text = @"Remove Image";
                }
            }
            else
            {
                ((Button) sender).Text = @"Load Image";
                _options.BackgroundImage.Image = null;
            }
        }

        private void ChangeForegroundImage_Click(object sender, EventArgs e)
        {
            if (_options.ForegroundImage.Image == null)
            {
                FileDialog fd = new OpenFileDialog();
                fd.Filter = @"PNG Files (*.png)|*.png";
                fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                fd.Title = @"Choose A Foreground Image";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _options.ForegroundImage.Name = Path.GetFileNameWithoutExtension(fd.FileName);
                        _options.ForegroundImage.Image = Image.FromFile(fd.FileName);
                        ((Button) sender).Text = @"Remove Image";
                    }
                    catch
                    {
                        MessageBox.Show(@"Could not load image.");
                    }
                }
            }
            else
            {
                ((Button) sender).Text = @"Load Image";
                _options.ForegroundImage.Image = null;
            }
        }
    }
}