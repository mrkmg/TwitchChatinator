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
            ChromaKeyInput.MouseDown += ChromaKeyInput_MouseDown;
            Option1Color.MouseDown += Option1Color_MouseDown;
            Option2Color.MouseDown += Option2Color_MouseDown;
            Option3Color.MouseDown += Option3Color_MouseDown;
            Option4Color.MouseDown += Option4Color_MouseDown;

            TitleColorInput.MouseDown += TitleColorInput_MouseDown;
            CountColorInput.MouseDown += CountColorInput_MouseDown;
            TotalColorInput.MouseDown += TotalColorInput_MouseDown;

            Option1Input.Text = Settings.Default.PollOption1;
            Option2Input.Text = Settings.Default.PollOption2;
            Option3Input.Text = Settings.Default.PollOption3;
            Option4Input.Text = Settings.Default.PollOption4;
            Option1Color.Text = Settings.Default.PollOption1Color;
            Option2Color.Text = Settings.Default.PollOption2Color;
            Option3Color.Text = Settings.Default.PollOption3Color;
            Option4Color.Text = Settings.Default.PollOption4Color;
            ChromaKeyInput.Text = Settings.Default.PollChromaKey;
            AllowMultiDropdown.Text = Settings.Default.PollAllowMulti ? "Yes" : "No";

            ChromaKeyInput.BackColor = getColorFromString(ChromaKeyInput.Text);
            Option1Color.BackColor = getColorFromString(Option1Color.Text);
            Option2Color.BackColor = getColorFromString(Option2Color.Text);
            Option3Color.BackColor = getColorFromString(Option3Color.Text);
            Option4Color.BackColor = getColorFromString(Option4Color.Text);



            TitleColorInput.BackColor = getColorFromString(TitleColorInput.Text);
            CountColorInput.BackColor = getColorFromString(CountColorInput.Text);
            TotalColorInput.BackColor = getColorFromString(TotalColorInput.Text);

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
        }

        void TotalColorInput_MouseDown(object sender, MouseEventArgs e)
        {
            ColorDialog CD = new ColorDialog();

            if (CD.ShowDialog() == DialogResult.OK)
            {
                TotalColorInput.Text = CD.Color.R.ToString().PadLeft(3, '0') + CD.Color.G.ToString().PadLeft(3, '0') + CD.Color.B.ToString().PadLeft(3, '0');
                TotalColorInput.BackColor = CD.Color;
            }
        }

        void CountColorInput_MouseDown(object sender, MouseEventArgs e)
        {
            ColorDialog CD = new ColorDialog();

            if (CD.ShowDialog() == DialogResult.OK)
            {
                CountColorInput.Text = CD.Color.R.ToString().PadLeft(3, '0') + CD.Color.G.ToString().PadLeft(3, '0') + CD.Color.B.ToString().PadLeft(3, '0');
                CountColorInput.BackColor = CD.Color;
            }
        }

        void TitleColorInput_MouseDown(object sender, MouseEventArgs e)
        {
            ColorDialog CD = new ColorDialog();

            if (CD.ShowDialog() == DialogResult.OK)
            {
                TitleColorInput.Text = CD.Color.R.ToString().PadLeft(3, '0') + CD.Color.G.ToString().PadLeft(3, '0') + CD.Color.B.ToString().PadLeft(3, '0');
                TitleColorInput.BackColor = CD.Color;
            }
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

        void ChromaKeyInput_MouseDown(object sender, MouseEventArgs e)
        {
            ColorDialog CD = new ColorDialog();

            if (CD.ShowDialog() == DialogResult.OK)
            {
                ChromaKeyInput.Text = CD.Color.R.ToString().PadLeft(3, '0') + CD.Color.G.ToString().PadLeft(3, '0') + CD.Color.B.ToString().PadLeft(3, '0');
                ChromaKeyInput.BackColor = CD.Color;
            }
        }

        private void Option1Color_MouseDown(object sender, EventArgs e)
        {
            ColorDialog CD = new ColorDialog();

            if (CD.ShowDialog() == DialogResult.OK)
            {
                Option1Color.Text = CD.Color.R.ToString().PadLeft(3, '0') + CD.Color.G.ToString().PadLeft(3, '0') + CD.Color.B.ToString().PadLeft(3, '0');
                Option1Color.BackColor = CD.Color;
            }
        }

        private void Option2Color_MouseDown(object sender, EventArgs e)
        {
            ColorDialog CD = new ColorDialog();

            if (CD.ShowDialog() == DialogResult.OK)
            {
                Option2Color.Text = CD.Color.R.ToString().PadLeft(3, '0') + CD.Color.G.ToString().PadLeft(3, '0') + CD.Color.B.ToString().PadLeft(3, '0');
                Option2Color.BackColor = CD.Color;
            }
        }

        private void Option3Color_MouseDown(object sender, EventArgs e)
        {
            ColorDialog CD = new ColorDialog();

            if (CD.ShowDialog() == DialogResult.OK)
            {
                Option3Color.Text = CD.Color.R.ToString().PadLeft(3, '0') + CD.Color.G.ToString().PadLeft(3, '0') + CD.Color.B.ToString().PadLeft(3, '0');
                Option3Color.BackColor = CD.Color;
            }
        }

        private void Option4Color_MouseDown(object sender, EventArgs e)
        {
            ColorDialog CD = new ColorDialog();

            if (CD.ShowDialog() == DialogResult.OK)
            {
                Option4Color.Text = CD.Color.R.ToString().PadLeft(3, '0') + CD.Color.G.ToString().PadLeft(3, '0') + CD.Color.B.ToString().PadLeft(3, '0');
                Option4Color.BackColor = CD.Color;
            }
        }

        private void FontInput_TextChanged(object sender, EventArgs e)
        {
            FontInput.Font = new Font(FontInput.Text, FontInput.Font.Size);
        }

        
    }
}
