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
    public partial class LaunchGiveaway : Form
    {
        List<SelectListObject> Options;
        RunGiveaway Giveaway;
        DateTime StartTime;

        public LaunchGiveaway()
        {
            InitializeComponent();

            PopulateList();
            StartTime = DateTime.Now;
            RollButton.Enabled = false;

            FormClosed += LaunchGiveaway_FormClosed;
        }

        void LaunchGiveaway_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Giveaway != null)
            {
                Giveaway.Close();
            }
        }

        void PopulateList()
        {
            List.Items.Clear();

            //Get Giveaways
            List<string> Giveaways = GiveawayOptions.GetAvaliable();

            Options = new List<SelectListObject>();

            foreach (string n in Giveaways)
            {
                Options.Add(new SelectListObject(n, "Giveaway"));
                List.Items.Add(n);
            }


            if (List.Items.Count > 0)
            {
                List.SelectedIndex = 0;
                StartButton.Enabled = true;
            }
            else
            {
                StartButton.Enabled = false;
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (Giveaway == null)
            {
                RunGiveaway GV = new RunGiveaway(StartTime, Options[List.SelectedIndex].name, GiveawayTitle.Text);
                GV.Show();
                GV.FormClosed += Giveaway_FormClosed;
                Giveaway = GV;
                StartButton.Text = "Stop Giveaway";
                RollButton.Enabled = true;
            }
            else
            {
                Giveaway.Close();
                RollButton.Enabled = false;
                StartButton.Text = "Start Poll";
            }
        }

        void Giveaway_FormClosed(object sender, FormClosedEventArgs e)
        {
            Giveaway = null;
            StartButton.Text = "Start Poll";
        }

        private void RollButton_Click(object sender, EventArgs e)
        {
            if (Giveaway != null)
            {
                Giveaway.Roll();
            }
        }
    }
}
