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
    public partial class PollSetup : Form
    {
        public delegate void Save();
        public event Save OnSave;

        Font TitleFont;
        Font CountFont;
        Font TotalFont;

        public PollSetup()
        {
            InitializeComponent();

            ChromaKeyInput.MouseDown += ColorClickHandler;
            Option1Color.MouseDown += ColorClickHandler;
            Option2Color.MouseDown += ColorClickHandler;
            Option3Color.MouseDown += ColorClickHandler;
            Option4Color.MouseDown += ColorClickHandler;
            TitleColorInput.MouseDown += ColorClickHandler;
            CountColorInput.MouseDown += ColorClickHandler;
            TotalColorInput.MouseDown += ColorClickHandler;

            ChromaKeyInput.BackColor = Settings.Default.PollChromaKey;

            Option1Input.Text = Settings.Default.PollOption1;
            Option2Input.Text = Settings.Default.PollOption2;
            Option3Input.Text = Settings.Default.PollOption3;
            Option4Input.Text = Settings.Default.PollOption4;
            Option1Color.BackColor = Settings.Default.PollOption1Color;
            Option2Color.BackColor = Settings.Default.PollOption2Color;
            Option3Color.BackColor = Settings.Default.PollOption3Color;
            Option4Color.BackColor = Settings.Default.PollOption4Color;

            AllowMultiDropdown.Text = Settings.Default.PollAllowMulti ? "Yes" : "No";

            LeftMarginNum.Value = Settings.Default.PollLeftMargin;
            RightMarginNum.Value = Settings.Default.PollRightMargin;
            TopMarginNum.Value = Settings.Default.PollTopMargin;
            BottomMarginNum.Value = Settings.Default.PollBottomMargin;

            BarHeightNum.Value = Settings.Default.PollBarHeight;
            BarWidthNum.Value = Settings.Default.PollBarWidth;
            BarSpacingNum.Value = Settings.Default.PollBarSpacing;

            TotalWidthNum.Value = Settings.Default.PollTotalWidth;

            TitleFont = Settings.Default.PollTitleFont;
            CountFont = Settings.Default.PollCountFont;
            TotalFont = Settings.Default.PollTotalFont;
        }

        private void SavePollSetup_Click(object sender, EventArgs e)
        {
            Settings.Default.PollOption1 = Option1Input.Text;
            Settings.Default.PollOption2 = Option2Input.Text;
            Settings.Default.PollOption3 = Option3Input.Text;
            Settings.Default.PollOption4 = Option4Input.Text;

            Settings.Default.PollOption1Color = Option1Color.BackColor;
            Settings.Default.PollOption2Color = Option2Color.BackColor;
            Settings.Default.PollOption3Color = Option3Color.BackColor;
            Settings.Default.PollOption4Color = Option4Color.BackColor;

            Settings.Default.PollChromaKey = ChromaKeyInput.BackColor;

            Settings.Default.PollTitleColor = TitleColorInput.BackColor;
            Settings.Default.PollCountColor = CountColorInput.BackColor;
            Settings.Default.PollTotalColor = TotalColorInput.BackColor;

            Settings.Default.PollAllowMulti = AllowMultiDropdown.Text == "Yes";

            Settings.Default.PollLeftMargin = (int)LeftMarginNum.Value;
            Settings.Default.PollRightMargin = (int)RightMarginNum.Value;
            Settings.Default.PollTopMargin = (int)TopMarginNum.Value;
            Settings.Default.PollBottomMargin = (int)BottomMarginNum.Value;

            Settings.Default.PollBarHeight = (int)BarHeightNum.Value;
            Settings.Default.PollBarWidth = (int)BarWidthNum.Value;
            Settings.Default.PollBarSpacing = (int)BarSpacingNum.Value;

            Settings.Default.PollTotalWidth = (int)TotalWidthNum.Value;


            Settings.Default.PollTitleFont = TitleFont;
            Settings.Default.PollCountFont = CountFont;
            Settings.Default.PollTotalFont = TotalFont;

            
            Settings.Default.Save();
            if (OnSave != null)
            {
                OnSave();
            }
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

            SavePollSetup.Focus();
        }

        private void TitleFontButton_Click(object sender, EventArgs e)
        {
            using (FontDialog FD = new FontDialog())
            {
                FD.Font = TitleFont;
                if (FD.ShowDialog() == DialogResult.OK)
                {
                    TitleFont = FD.Font;
                }
            }
        }

        private void CountFontButton_Click(object sender, EventArgs e)
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

        private void TotalFontButton_Click(object sender, EventArgs e)
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
