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
    public partial class LaunchPoll : Form
    {
        List<SelectListObject> Options;

        public LaunchPoll()
        {
            InitializeComponent();

            PopulateList();
        }

        void PopulateList()
        {
            List.Items.Clear();

            //Get Bar Graphs
            List<string> BarGraphs = BarGraphOptions.GetAvaliable();

            //Get Pie Graphs - TODO
            List<string> PieGraphs = new List<string>();

            Options = new List<SelectListObject>();

            foreach (string n in BarGraphs)
            {
                Options.Add(new SelectListObject(n, "Bar"));
                List.Items.Add("Bar - " + n);
            }
            foreach (string n in PieGraphs)
            {
                Options.Add(new SelectListObject(n, "Pie"));
                List.Items.Add("Pie - " + n);
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
            List<string> Labels = new List<string>();

            if (Option1.Text.Trim() != "")
            {
                Labels.Add(Option1.Text.Trim());
            }
            if (Option2.Text.Trim() != "")
            {
                Labels.Add(Option2.Text.Trim());
            }
            if (Option3.Text.Trim() != "")
            {
                Labels.Add(Option3.Text.Trim());
            }
            if (Option4.Text.Trim() != "")
            {
                Labels.Add(Option4.Text.Trim());
            }
            switch (Options[List.SelectedIndex].type)
            {
                case "Bar":
                    RunPollBar RPB = new RunPollBar(DateTime.Now, Options[List.SelectedIndex].name, PollTitle.Text, Labels.ToArray());
                    RPB.Show();
                    break;
                case "Pie":

                    break;
            }
        }
    }


    public class PollData
    {
        public string[] options;
        public int[] amounts;
        public int totalVotes = 0;

        public PollData(int count)
        {
            options = new string[count];
            amounts = new int[count];
        }
    }
}
