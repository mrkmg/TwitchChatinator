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
    public partial class ManageGiveaway : Form
    {
        List<SelectListObject> Options;

        public ManageGiveaway()
        {
            InitializeComponent();

            List.SelectionMode = SelectionMode.One;
            PopulateList();
        }

        void PopulateList()
        {
            List.Items.Clear();

            //Get Bar Graphs
            List<string> Giveaways = GiveawayOptions.GetAvaliable();

            //Get Pie Graphs - TODO
            List<string> PieGraphs = new List<string>();

            Options = new List<SelectListObject>();

            foreach (string n in Giveaways)
            {
                Options.Add(new SelectListObject(n, "Giveaway"));
                List.Items.Add(n);
            }


            if (List.Items.Count > 0)
            {
                List.SelectedIndex = 0;
                EditButton.Enabled = true;
                DeleteButton.Enabled = true;
                CopyButton.Enabled = true;
                RenameButton.Enabled = true;
            }
            else
            {
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
                CopyButton.Enabled = false;
                RenameButton.Enabled = false;
            }
        }

        private void NewGiveawayButton_Click(object sender, EventArgs e)
        {
            InputBoxResult result = InputBox.Show("Name:", "New Giveaway Tempalate", "", GiveawayOptions.ValidateNameHandler);
            if (result.OK)
            {
                //TODO: Add Exception Control
                GiveawayOptions.CreateNew(result.Text);
            }
            PopulateList();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            switch (Options[List.SelectedIndex].type)
            {
                case "Giveaway":
                    var sp = new SetupGiveaway(Options[List.SelectedIndex].name);
                    sp.Show();
                    break;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            switch (Options[List.SelectedIndex].type)
            {
                case "Giveaway":
                    GiveawayOptions.Remove(Options[List.SelectedIndex].name);
                    break;
            }
            PopulateList();
        }

        private void RenameButton_Click(object sender, EventArgs e)
        {
            switch (Options[List.SelectedIndex].type)
            {
                case "Giveaway":
                    InputBoxResult result = InputBox.Show("New Name:", "Rename Giveaway Template", "", GiveawayOptions.ValidateNameHandler);
                    if (result.OK)
                    {
                        GiveawayOptions.Rename(Options[List.SelectedIndex].name, result.Text);
                    }
                    break;
            }
            PopulateList();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            switch (Options[List.SelectedIndex].type)
            {
                case "Giveaway":
                    InputBoxResult result = InputBox.Show("Copy To:", "Copy Bar Graph", "", GiveawayOptions.ValidateNameHandler);
                    if (result.OK)
                    {
                        var o = GiveawayOptions.Load(Options[List.SelectedIndex].name);
                        o.Save(result.Text);
                    }
                    break;
            }
            PopulateList();
        }
    }
}
