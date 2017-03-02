using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TwitchChatinator.Forms.Runners;
using TwitchChatinator.Options;

namespace TwitchChatinator.Forms.Setups
{
    public partial class SetupBarGraph : Form
    {
        private Font _countFont;

        private Font _optionLabelFont;
        private BarGraphOptions _options;
        private readonly string _optionsName;

        private RunPollBar _runPollBar;
        private Font _totalFont;

        public SetupBarGraph(string n)
        {
            InitializeComponent();
            _optionsName = n;
            Populate();

            ChangeBackgroundImage.Text = _options.BackgroundImage.Image == null ? "Load Image" : "Remove Image";
            ChangeForegroundImage.Text = _options.ForegroundImage.Image == null ? "Load Image" : "Remove Image";

            ChromaKey.Click += ColorClickHandler;
            Option1Color.Click += ColorClickHandler;
            Option2Color.Click += ColorClickHandler;
            Option3Color.Click += ColorClickHandler;
            Option4Color.Click += ColorClickHandler;
            OptionFontColor.Click += ColorClickHandler;
            CountFontColor.Click += ColorClickHandler;
            TotalFontColor.Click += ColorClickHandler;

            OptionFontSelector.Click += OptionFontSelector_Click;
            CountFontSelector.Click += CountFontSelector_Click;
            TotalFontSelector.Click += TotalFontSelector_Click;

            CancelButton.Click += CancelButton_Click;
            SaveButton.Click += SaveButton_Click;

            FormClosed += SetupBarGraph_FormClosed;

            RefreshPreview();

            HeightInput.ValueChanged += EventHandler_Change;
            WidthInput.ValueChanged += EventHandler_Change;

            MarginTop.ValueChanged += EventHandler_Change;
            MarginBottom.ValueChanged += EventHandler_Change;
            MarginLeft.ValueChanged += EventHandler_Change;
            MarginRight.ValueChanged += EventHandler_Change;
            BarSpacing.ValueChanged += EventHandler_Change;

            ChromaKey.BackColorChanged += EventHandler_Change;
            Option1Color.BackColorChanged += EventHandler_Change;
            Option2Color.BackColorChanged += EventHandler_Change;
            Option3Color.BackColorChanged += EventHandler_Change;
            Option4Color.BackColorChanged += EventHandler_Change;

            OptionFontColor.BackColorChanged += EventHandler_Change;

            CountFontColor.BackColorChanged += EventHandler_Change;

            TotalFontColor.BackColorChanged += EventHandler_Change;

            TotalPosition.SelectedIndexChanged += EventHandler_Change;
        }

        private void EventHandler_Change(object sender, EventArgs e)
        {
            RefreshPreview();
        }

        private void SetupBarGraph_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_runPollBar != null && !_runPollBar.IsDisposed)
            {
                _runPollBar.Close();
            }
        }

        private void RefreshPreview()
        {
            if (_runPollBar != null && !_runPollBar.IsDisposed)
            {
                _runPollBar.Close();
            }

            SetOptionsToValues();
            BarGraphOptions.PreviewOptions = _options;

            var vals = new string[4];
            vals[0] = "One";
            vals[1] = "Two";
            vals[2] = "Three";
            vals[3] = "Four";

            foreach (Control control in Controls)
            {
                if (control is NumericUpDown)
                {
                    SetOptionsToValues();
                }
            }

            _runPollBar = new RunPollBar(DateTime.MinValue, "_preview", "Demo Poll", vals);
            _runPollBar.Show();
            Focus();
        }

        private void SetOptionsToValues()
        {
            _options.Height = (int) HeightInput.Value;
            _options.Width = (int) WidthInput.Value;

            _options.MarginTop = (int) MarginTop.Value;
            _options.MarginBottom = (int) MarginBottom.Value;
            _options.MarginLeft = (int) MarginLeft.Value;
            _options.MarginRight = (int) MarginRight.Value;
            _options.BarSpacing = (int) BarSpacing.Value;

            _options.ChromaKey = ChromaKey.BackColor;
            _options.Option1Color = Option1Color.BackColor;
            _options.Option2Color = Option2Color.BackColor;
            _options.Option3Color = Option3Color.BackColor;
            _options.Option4Color = Option4Color.BackColor;

            _options.OptionFont = _optionLabelFont;
            _options.OptionFontColor = OptionFontColor.BackColor;

            _options.CountFont = _countFont;
            _options.CountFontColor = CountFontColor.BackColor;

            _options.TitleFont = _totalFont;
            _options.TitleFontColor = TotalFontColor.BackColor;

            _options.TotalPosition = TotalPosition.SelectedItem.ToString();
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

        private bool Populate()
        {
            try
            {
                _options = BarGraphOptions.Load(_optionsName);
            }
            catch (Exception e)
            {
                throw e;
            }

            NameLabel.Text = _optionsName;

            HeightInput.Value = _options.Height;
            WidthInput.Value = _options.Width;

            MarginTop.Value = _options.MarginTop;
            MarginBottom.Value = _options.MarginBottom;
            MarginLeft.Value = _options.MarginLeft;
            MarginRight.Value = _options.MarginRight;
            BarSpacing.Value = _options.BarSpacing;

            ChromaKey.BackColor = _options.ChromaKey;
            Option1Color.BackColor = _options.Option1Color;
            Option2Color.BackColor = _options.Option2Color;
            Option3Color.BackColor = _options.Option3Color;
            Option4Color.BackColor = _options.Option4Color;

            _optionLabelFont = _options.OptionFont;
            OptionFontColor.BackColor = _options.OptionFontColor;

            _countFont = _options.CountFont;
            CountFontColor.BackColor = _options.CountFontColor;

            _totalFont = _options.TitleFont;
            TotalFontColor.BackColor = _options.TitleFontColor;

            TotalPosition.SelectedItem = _options.TotalPosition;
            return true;
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

        private void OptionFontSelector_Click(object sender, EventArgs e)
        {
            using (var fd = new FontDialog())
            {
                fd.Font = _optionLabelFont;
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    _optionLabelFont = fd.Font;
                    RefreshPreview();
                }
            }
        }

        private void CountFontSelector_Click(object sender, EventArgs e)
        {
            using (var fd = new FontDialog())
            {
                fd.Font = _countFont;
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    _countFont = fd.Font;
                    RefreshPreview();
                }
            }
        }

        private void TotalFontSelector_Click(object sender, EventArgs e)
        {
            using (var fd = new FontDialog())
            {
                fd.Font = _totalFont;
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    _totalFont = fd.Font;
                    RefreshPreview();
                }
            }
        }

        private void ChangeBackgroundImage_Click(object sender, EventArgs e)
        {
            if (_options.BackgroundImage.Image == null)
            {
                FileDialog fd = new OpenFileDialog();
                fd.Filter = "PNG Files (*.png)|*.png";
                fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                fd.Title = "Choose A Background Image";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    _options.BackgroundImage.Name = Path.GetFileNameWithoutExtension(fd.FileName);
                    _options.BackgroundImage.Image = Image.FromFile(fd.FileName);
                    ((Button) sender).Text = "Remove Image";
                }
            }
            else
            {
                ((Button) sender).Text = "Load Image";
                _options.BackgroundImage.Image = null;
            }
        }

        private void ChangeForegroundImage_Click(object sender, EventArgs e)
        {
            if (_options.ForegroundImage.Image == null)
            {
                FileDialog fd = new OpenFileDialog();
                fd.Filter = "PNG Files (*.png)|*.png";
                fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                fd.Title = "Choose A Foreground Image";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _options.ForegroundImage.Name = Path.GetFileNameWithoutExtension(fd.FileName);
                        _options.ForegroundImage.Image = Image.FromFile(fd.FileName);
                        ((Button) sender).Text = "Remove Image";
                    }
                    catch
                    {
                        MessageBox.Show("Could not load image.");
                    }
                }
            }
            else
            {
                ((Button) sender).Text = "Load Image";
                _options.ForegroundImage.Image = null;
            }
        }
    }
}