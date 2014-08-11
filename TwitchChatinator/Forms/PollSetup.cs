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

            Option1Input.Text = Settings.Default.PollOption1;
            Option2Input.Text = Settings.Default.PollOption2;
            Option3Input.Text = Settings.Default.PollOption3;
            Option4Input.Text = Settings.Default.PollOption4;
            ChromaKeyInput.Text = Settings.Default.PollChromaKey;

            ChromaKeyInput.BackColor = getColorFromString(ChromaKeyInput.Text);

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

        void ChromaKeyInput_MouseDown(object sender, MouseEventArgs e)
        {
            ColorDialog CD = new ColorDialog();

            if (CD.ShowDialog() == DialogResult.OK)
            {
                ChromaKeyInput.Text = CD.Color.R.ToString().PadLeft(3,'0') + CD.Color.G.ToString().PadLeft(3,'0') + CD.Color.B.ToString().PadLeft(3,'0');
                ChromaKeyInput.BackColor = CD.Color;
            }
        }

        private void SavePollSetup_Click(object sender, EventArgs e)
        {
            Settings.Default.PollOption1 = Option1Input.Text;
            Settings.Default.PollOption2 = Option2Input.Text;
            Settings.Default.PollOption3 = Option3Input.Text;
            Settings.Default.PollOption4 = Option4Input.Text;
            Settings.Default.PollChromaKey = ChromaKeyInput.Text;
            Settings.Default.Save();
            Close();
        }

        
    }
}
