using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitchChatinator
{
    public partial class SetupGiveaway : Form
    {
        private string OptionsName;
        private GiveawayOptions Options;

        private Font RollerFont;
        private Font EntriesFont;
        private Font TitleFont;

        private RunGiveaway RunGiveAway;

        public SetupGiveaway(string N)
        {
            OptionsName = N;

            InitializeComponent();
            Populate();
            CalcuateAndShowHeight();

            Closed += SetupGiveaway_Closed;

            ChangeBackgroundImage.Text = Options.BackgroundImage.Image == null ? "Load Image" : "Remove Image";
            ChangeForegroundImage.Text = Options.ForegroundImage.Image == null ? "Load Image" : "Remove Image";

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
            if (RunGiveAway != null && !RunGiveAway.IsDisposed)
            {
                RunGiveAway.Close();
            }
        }

        private void EventHandler_Change(object sender, EventArgs e)
        {
            RefreshPreview();
        }

        private void RefreshPreview()
        {
            if (RunGiveAway != null && !RunGiveAway.IsDisposed)
            {
                RunGiveAway.Close();
            }

            setOptionsToValues();
            GiveawayOptions.PreviewOptions = Options;

            RunGiveAway = new RunGiveaway(DateTime.MinValue, "_preview", "Demo Giveaway");
            RunGiveAway.Show();
            var RollStartTimer = new Timer();
            RollStartTimer.Tick += (o, args) =>
            {
                RunGiveAway.Roll();
                RollStartTimer.Dispose();
            };
            RollStartTimer.Interval = 10000;
            RollStartTimer.Enabled = true;
            RunGiveAway.Closed += (o, args) => RollStartTimer.Dispose();

            Focus();
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            CalcuateAndShowHeight();
        }

        void setOptionsToValues()
        {
            Options.Width = (int)WidthInput.Value;

            Options.MarginTop = (int)MarginTop.Value;
            Options.MarginBottom = (int)MarginBottom.Value;
            Options.MarginLeft = (int)MarginLeft.Value;
            Options.MarginRight = (int)MarginRight.Value;
            Options.Spacing = (int)Spacing.Value;

            Options.ChromaKey = ChromaKey.BackColor;

            Options.RollerFont = RollerFont;
            Options.RollerFontColor = EntriesFontColor.BackColor;

            Options.EntriesFont = EntriesFont;
            Options.EntriesFontColor = RollerFontColor.BackColor;

            Options.TitleFont = TitleFont;
            Options.TitleFontColor = TitleFontColor.BackColor;
        }

        void SaveButton_Click(object sender, EventArgs e)
        {
           setOptionsToValues();

            Options.Save(OptionsName);
            Close();
        }

        void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        void CalcuateAndShowHeight()
        {
            HeightInput.Value = MarginTop.Value + MarginBottom.Value + TitleFont.Height + RollerFont.Height + EntriesFont.Height + (Spacing.Value * 2);
        }

        private bool Populate()
        {
            try
            {
                Options = GiveawayOptions.Load(OptionsName);
            }
            catch (Exception e)
            {
                throw e;
            }

            NameLabel.Text = OptionsName;

            WidthInput.Value = Options.Width;

            MarginTop.Value = Options.MarginTop;
            MarginBottom.Value = Options.MarginBottom;
            MarginLeft.Value = Options.MarginLeft;
            MarginRight.Value = Options.MarginRight;
            Spacing.Value = Options.Spacing;

            ChromaKey.BackColor = Options.ChromaKey;

            RollerFont = Options.RollerFont;
            RollerFontColor.BackColor = Options.RollerFontColor;

            EntriesFont = Options.EntriesFont;
            EntriesFontColor.BackColor = Options.EntriesFontColor;

            TitleFont = Options.TitleFont;
            TitleFontColor.BackColor = Options.TitleFontColor;

            return true;
        }

        void ColorClickHandler(object sender, EventArgs e)
        {
            TextBox T = (TextBox)sender;
            using (ColorDialog CD = new ColorDialog())
            {
                CD.Color = T.BackColor;
                if (CD.ShowDialog() == DialogResult.OK)
                {
                    T.BackColor = CD.Color;
                }
            }

            SaveButton.Focus();
        }

        void EntriesFontSelector_Click(object sender, EventArgs e)
        {
            using (FontDialog FD = new FontDialog())
            {
                FD.Font = EntriesFont;
                if (FD.ShowDialog() == DialogResult.OK)
                {
                    EntriesFont = FD.Font;
                    RefreshPreview();
                }
            }
        }

        void RollerFontSelector_Click(object sender, EventArgs e)
        {
            using (FontDialog FD = new FontDialog())
            {
                FD.Font = RollerFont;
                if (FD.ShowDialog() == DialogResult.OK)
                {
                    RollerFont = FD.Font;
                    RefreshPreview();
                }
            }
        }

        void TitleFontSelector_Click(object sender, EventArgs e)
        {
            using (FontDialog FD = new FontDialog())
            {
                FD.Font = TitleFont;
                if (FD.ShowDialog() == DialogResult.OK)
                {
                    TitleFont = FD.Font;
                    RefreshPreview();
                }
            }
        }

        private void ChangeBackgroundImage_Click(object sender, EventArgs e)
        {
            if (Options.BackgroundImage.Image == null)
            {
                FileDialog FD = new OpenFileDialog();
                FD.Filter = "PNG Files (*.png)|*.png";
                FD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                FD.Title = "Choose A Background Image";
                if (FD.ShowDialog() == DialogResult.OK)
                {
                    Options.BackgroundImage.Name = Path.GetFileNameWithoutExtension(FD.FileName);
                    Options.BackgroundImage.Image = Image.FromFile(FD.FileName);
                    ((Button)sender).Text = "Remove Image";
                }
            }
            else
            {
                ((Button)sender).Text = "Load Image";
                Options.BackgroundImage.Image = null;
            }
        }

        private void ChangeForegroundImage_Click(object sender, EventArgs e)
        {
            if (Options.ForegroundImage.Image == null)
            {
                FileDialog FD = new OpenFileDialog();
                FD.Filter = "PNG Files (*.png)|*.png";
                FD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                FD.Title = "Choose A Foreground Image";
                if (FD.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Options.ForegroundImage.Name = Path.GetFileNameWithoutExtension(FD.FileName);
                        Options.ForegroundImage.Image = Image.FromFile(FD.FileName);
                        ((Button)sender).Text = "Remove Image";
                    }
                    catch
                    {
                        MessageBox.Show("Could not load image.");
                    }
                }
            }
            else
            {
                ((Button)sender).Text = "Load Image";
                Options.ForegroundImage.Image = null;
            }
        }
    }
}
