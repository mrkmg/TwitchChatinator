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
        public PollSetup()
        {
            InitializeComponent();
            ChromaKeyInput.MouseDown += ChromaKeyInput_MouseDown;
            Option1Color.MouseDown += Option1Color_MouseDown;
            Option2Color.MouseDown += Option2Color_MouseDown;
            Option3Color.MouseDown += Option3Color_MouseDown;
            Option4Color.MouseDown += Option4Color_MouseDown;

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
            
            Settings.Default.Save();
            Close();
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

        
    }
}
