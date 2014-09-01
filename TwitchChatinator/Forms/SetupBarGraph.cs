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
    public partial class SetupBarGraph : Form
    {
        private string OptionsName;
        private BarGraphOptions Options;

        private Font OptionLabelFont;
        private Font CountFont;
        private Font TotalFont;

        public SetupBarGraph(string N)
        {
            InitializeComponent();
            OptionsName = N;
            Populate();
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
        }

        void SaveButton_Click(object sender, EventArgs e)
        {
            Options.Height = (int)HeightInput.Value;
            Options.Width = (int)WidthInput.Value;

            Options.MarginTop = (int)MarginTop.Value;
            Options.MarginBottom = (int)MarginBottom.Value;
            Options.MarginLeft = (int)MarginLeft.Value;
            Options.MarginRight = (int)MarginRight.Value;
            Options.BarSpacing = (int)BarSpacing.Value;

            Options.ChromaKey = ChromaKey.BackColor;
            Options.Option1Color = Option1Color.BackColor;
            Options.Option2Color = Option2Color.BackColor;
            Options.Option3Color = Option3Color.BackColor;
            Options.Option4Color = Option4Color.BackColor;

            Options.OptionFont = OptionLabelFont;
            Options.OptionFontColor = OptionFontColor.BackColor;

            Options.CountFont = CountFont;
            Options.CountFontColor = CountFontColor.BackColor;

            Options.TotalFont = TotalFont;
            Options.TotalFontColor = TotalFontColor.BackColor;

            Options.TotalPosition = TotalPosition.SelectedItem.ToString();

            Options.Save(OptionsName);
        }

        void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool Populate()
        {
            try
            {
                Options = BarGraphOptions.Load(OptionsName);
            }
            catch (Exception e)
            {
                throw e;
            }

            NameLabel.Text = OptionsName;

            HeightInput.Value = Options.Height;
            WidthInput.Value = Options.Width;

            MarginTop.Value = Options.MarginTop;
            MarginBottom.Value = Options.MarginBottom;
            MarginLeft.Value = Options.MarginLeft;
            MarginRight.Value = Options.MarginRight;
            BarSpacing.Value = Options.BarSpacing;

            ChromaKey.BackColor = Options.ChromaKey;
            Option1Color.BackColor = Options.Option1Color;
            Option2Color.BackColor = Options.Option2Color;
            Option3Color.BackColor = Options.Option3Color;
            Option4Color.BackColor = Options.Option4Color;

            OptionLabelFont = Options.OptionFont;
            OptionFontColor.BackColor = Options.OptionFontColor;

            CountFont = Options.CountFont;
            CountFontColor.BackColor = Options.CountFontColor;

            TotalFont = Options.TotalFont;
            TotalFontColor.BackColor = Options.TotalFontColor;

            TotalPosition.SelectedItem = Options.TotalPosition;

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

        void OptionFontSelector_Click(object sender, EventArgs e)
        {
            using (FontDialog FD = new FontDialog())
            {
                FD.Font = OptionLabelFont;
                if (FD.ShowDialog() == DialogResult.OK)
                {
                    OptionLabelFont = FD.Font;
                }
            }
        }

        void CountFontSelector_Click(object sender, EventArgs e)
        {
            using (FontDialog FD = new FontDialog())
            {
                FD.Font = CountFont;
                if (FD.ShowDialog() == DialogResult.OK)
                {
                    CountFont = FD.Font;
                }
            }
        }

        void TotalFontSelector_Click(object sender, EventArgs e)
        {
            using (FontDialog FD = new FontDialog())
            {
                FD.Font = TotalFont;
                if (FD.ShowDialog() == DialogResult.OK)
                {
                    TotalFont = FD.Font;
                }
            }
        }
    }
}
