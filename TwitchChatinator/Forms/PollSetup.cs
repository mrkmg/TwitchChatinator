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

        public PollSetup()
        {
            InitializeComponent();

            ChromaKeyInput.MouseDown += colorMouseDown;
            Option1Color.MouseDown += colorMouseDown;
            Option2Color.MouseDown += colorMouseDown;
            Option3Color.MouseDown += colorMouseDown;
            Option4Color.MouseDown += colorMouseDown;
            TitleColorInput.MouseDown += colorMouseDown;
            CountColorInput.MouseDown += colorMouseDown;
            TotalColorInput.MouseDown += colorMouseDown;

            Option1Input.Text = Settings.Default.PollOption1;
            Option2Input.Text = Settings.Default.PollOption2;
            Option3Input.Text = Settings.Default.PollOption3;
            Option4Input.Text = Settings.Default.PollOption4;
            Option1Color.Text = Settings.Default.PollOption1Color;
            Option2Color.Text = Settings.Default.PollOption2Color;
            Option3Color.Text = Settings.Default.PollOption3Color;
            Option4Color.Text = Settings.Default.PollOption4Color;

            AllowMultiDropdown.Text = Settings.Default.PollAllowMulti ? "Yes" : "No";

            LeftMarginNum.Value = Settings.Default._PollLeftMargin;
            RightMarginNum.Value = Settings.Default._PollRightMargin;
            TopMarginNum.Value = Settings.Default._PollTopMargin;
            BottomMarginNum.Value = Settings.Default._PollBottomMargin;

            BarHeightNum.Value = Settings.Default._PollBarHeight;
            BarWidthNum.Value = Settings.Default._PollBarWidth;
            BarSpacingNum.Value = Settings.Default._PollBarSpacing;

            FontInput.Text = Settings.Default._PollFontName;

            TitleSizeNum.Value = (int)Settings.Default._PollTitleSize;
            CountSizeNum.Value = (int)Settings.Default._PollCountSize;
            TotalSizeNum.Value = (int)Settings.Default._PollTotalSize;

            TitleColorInput.Text = Settings.Default._PollTitleColor;
            CountColorInput.Text = Settings.Default._PollCountColor;
            TotalColorInput.Text = Settings.Default._PollTotalColor;

            ChromaKeyInput.Text = Settings.Default.PollChromaKey;

            ChromaKeyInput.BackColor = getColorFromString(ChromaKeyInput.Text);
            Option1Color.BackColor = getColorFromString(Option1Color.Text);
            Option2Color.BackColor = getColorFromString(Option2Color.Text);
            Option3Color.BackColor = getColorFromString(Option3Color.Text);
            Option4Color.BackColor = getColorFromString(Option4Color.Text);

            ChromaKeyInput.ForeColor = getColorFromString(ChromaKeyInput.Text);
            Option1Color.ForeColor = getColorFromString(Option1Color.Text);
            Option2Color.ForeColor = getColorFromString(Option2Color.Text);
            Option3Color.ForeColor = getColorFromString(Option3Color.Text);
            Option4Color.ForeColor = getColorFromString(Option4Color.Text);

            TitleColorInput.BackColor = getColorFromString(TitleColorInput.Text);
            CountColorInput.BackColor = getColorFromString(CountColorInput.Text);
            TotalColorInput.BackColor = getColorFromString(TotalColorInput.Text);

            TitleColorInput.ForeColor = getColorFromString(TitleColorInput.Text);
            CountColorInput.ForeColor = getColorFromString(CountColorInput.Text);
            TotalColorInput.ForeColor = getColorFromString(TotalColorInput.Text);
        }

        public static Color getColorFromString(string s)
        {
            
            Color C;

            if (s.Length == 9)
            {
                int r = int.Parse(s.Substring(0, 3));
                int g = int.Parse(s.Substring(3, 3));
                int b = int.Parse(s.Substring(6, 3));

                if(r >= 0 && r <= 255 &&
                   g >= 0 && g <= 255 &&
                   b >= 0 && b <= 255)
                {
                    C = Color.FromArgb(255,r,g,b);
                }
                else
                {
                    C = new Color();
                }

            } else {
                C = new Color();
            }

            return C;
        }

        private void SavePollSetup_Click(object sender, EventArgs e)
        {
            Settings.Default.PollOption1 = Option1Input.Text;
            Settings.Default.PollOption2 = Option2Input.Text;
            Settings.Default.PollOption3 = Option3Input.Text;
            Settings.Default.PollOption4 = Option4Input.Text;

            Settings.Default.PollOption1Color = Option1Color.Text;
            Settings.Default.PollOption2Color = Option2Color.Text;
            Settings.Default.PollOption3Color = Option3Color.Text;
            Settings.Default.PollOption4Color = Option4Color.Text;
            Settings.Default.PollChromaKey = ChromaKeyInput.Text;

            Settings.Default.PollAllowMulti = AllowMultiDropdown.Text == "Yes";

            Settings.Default._PollLeftMargin = (int)LeftMarginNum.Value;
            Settings.Default._PollRightMargin = (int)RightMarginNum.Value;
            Settings.Default._PollTopMargin = (int)TopMarginNum.Value;
            Settings.Default._PollBottomMargin = (int)BottomMarginNum.Value;

            Settings.Default._PollBarHeight = (int)BarHeightNum.Value;
            Settings.Default._PollBarWidth = (int)BarWidthNum.Value;
            Settings.Default._PollBarSpacing = (int)BarSpacingNum.Value;

            Settings.Default._PollFontName = FontInput.Text;

            Settings.Default._PollTitleSize = (float)TitleSizeNum.Value;
            Settings.Default._PollCountSize = (float)CountSizeNum.Value;
            Settings.Default._PollTotalSize = (float)TotalSizeNum.Value;

            Settings.Default._PollTitleColor = TitleColorInput.Text;
            Settings.Default._PollCountColor = CountColorInput.Text;
            Settings.Default._PollTotalColor = TotalColorInput.Text;

            
            Settings.Default.Save();
            if (OnSave != null)
            {
                OnSave();
            }
        }

        void colorMouseDown(object sender, MouseEventArgs e)
        {
            TextBox source = (TextBox)sender;
            ColorDialog CD = new ColorDialog();

            if (CD.ShowDialog() == DialogResult.OK)
            {
                source.Text = CD.Color.R.ToString().PadLeft(3, '0') + CD.Color.G.ToString().PadLeft(3, '0') + CD.Color.B.ToString().PadLeft(3, '0');
                source.BackColor = CD.Color;
                source.ForeColor = CD.Color;
            }

            SavePollSetup.Focus();
        }

        private void FontInput_TextChanged(object sender, EventArgs e)
        {
            FontInput.Font = new Font(FontInput.Text, FontInput.Font.Size);
        }

        
    }
}
