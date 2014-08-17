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
    public partial class SetupRoll : Form
    {

        public delegate void Save();
        public event Save OnSave;

        public SetupRoll()
        {
            InitializeComponent();

            this.Name = "Chatinator - Setup Roll";

            TopMarginInput.Value = Settings.Default.RollTopMargin;
            LeftMarginInput.Value = Settings.Default.RollLeftMargin;
            RightMarginInput.Value = Settings.Default.RollRightMargin;
            BottomMarginInput.Value = Settings.Default.RollBottomMargin;

            TitleColorInput.BackColor = Settings.Default.RollTitleColor;
            RollerColorInput.BackColor = Settings.Default.RollRollerColor;
            EntriesColorInput.BackColor = Settings.Default.RollEntriesColor;

            TitleTextInput.Font = Settings.Default.RollTitleFont;
            RollerTextInput.Font = Settings.Default.RollRollerFont;
            EntriesTextInput.Font = Settings.Default.RollEntriesFont;

            EntriesTopInput.Value = Settings.Default.RollEntriesTop;
            RollerTopInput.Value = Settings.Default.RollRollerTop;

            TitleTextInput.Text = Settings.Default.RollTitleText;
            RollerTextInput.Text = Settings.Default.RollRollerText;
            EntriesTextInput.Text = "Rolling!";

            ChromaKeyInput.BackColor = Settings.Default.RollChromaKey;
            TotalWidthInput.Value = Settings.Default.RollTotalWidth;

            TitleColorInput.Click += ColorClickHandler;
            RollerColorInput.Click += ColorClickHandler;
            EntriesColorInput.Click += ColorClickHandler;
            ChromaKeyInput.Click += ColorClickHandler;

            TitleTextInput.DoubleClick += FontClickHandler;
            RollerTextInput.DoubleClick += FontClickHandler;
            EntriesTextInput.DoubleClick += FontClickHandler;

            EntriesTextInput.GotFocus += EntriesTextInput_GotFocus;

            SaveButton.Click += SaveButton_Click;
        }

        void FontClickHandler(object sender, EventArgs e)
        {
            TextBox T = (TextBox)sender;
            using (FontDialog FD = new FontDialog())
            {
                FD.Font = T.Font;
                if (FD.ShowDialog() == DialogResult.OK)
                {
                    T.Font = FD.Font;
                }
            }
        }

        void EntriesTextInput_GotFocus(object sender, EventArgs e)
        {
            SaveButton.Focus();
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

        void SaveButton_Click(object sender, EventArgs e)
        {
            Settings.Default.RollTopMargin = (int)TopMarginInput.Value;
            Settings.Default.RollLeftMargin = (int)LeftMarginInput.Value;
            Settings.Default.RollRightMargin = (int)RightMarginInput.Value;
            Settings.Default.RollBottomMargin = (int)BottomMarginInput.Value;

            Settings.Default.RollTitleColor = TitleColorInput.BackColor;
            Settings.Default.RollRollerColor = RollerColorInput.BackColor;
            Settings.Default.RollEntriesColor = EntriesColorInput.BackColor;

            Settings.Default.RollTitleFont = TitleTextInput.Font;
            Settings.Default.RollRollerFont = RollerTextInput.Font;
            Settings.Default.RollEntriesFont = EntriesTextInput.Font;

            Settings.Default.RollEntriesTop = (int)EntriesTopInput.Value;
            Settings.Default.RollRollerTop = (int)RollerTopInput.Value;

            Settings.Default.RollTitleText = TitleTextInput.Text;
            Settings.Default.RollRollerText = RollerTextInput.Text;

            Settings.Default.RollChromaKey = ChromaKeyInput.BackColor;
            Settings.Default.RollTotalWidth = (int)TotalWidthInput.Value;

            Settings.Default.Save();
            if (OnSave != null)
            {
                OnSave();
            }
        }
    }
}
