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
        object Giveaway;
        Type GiveawayType;
        DateTime StartTime;

        public LaunchGiveaway()
        {
            InitializeComponent();

            PopulateList();
            StartTime = DateTime.Now;
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
                switch (Options[List.SelectedIndex].type)
                {
                    case "Giveaway":
                        RunGiveaway RPB = new RunGiveaway(StartTime, Options[List.SelectedIndex].name, PollTitle.Text, Labels.ToArray());
                        RPB.Show();
                        RPB.FormClosed += Poll_FormClosed;
                        Giveaway = RPB;
                        GiveawayType = typeof(RunGiveaway);
                        StartButton.Text = "Stop Giveaway";
                        break;
                }
            }
            else
            {
                switch (GiveawayType.Name)
                {
                    case "RunGiveaway":
                        ((RunGiveaway)Giveaway).Close();
                        break;
                }
                StartButton.Text = "Start Poll";
            }
        }

        void Poll_FormClosed(object sender, FormClosedEventArgs e)
        {
            Giveaway = null;
            GiveawayType = null;
            StartButton.Text = "Start Poll";
        }
    }
}
