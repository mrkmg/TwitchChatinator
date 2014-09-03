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
    public partial class ManagePolls : Form
    {
        List<SelectListObject> Options;


        public ManagePolls()
        {
            InitializeComponent();

            List.SelectionMode = SelectionMode.One;
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

        private void NewBarButton_Click(object sender, EventArgs e)
        {
            InputBoxResult result = InputBox.Show("Name:", "New Bar Graph", "", BarGraphOptions.ValidateNameHandler);
            if (result.OK)
            {
                //TODO: Add Exception Control
                BarGraphOptions.CreateNew(result.Text);
            }
            PopulateList();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            switch (Options[List.SelectedIndex].type)
            {
                case "Bar":
                    var sp = new SetupBarGraph(Options[List.SelectedIndex].name);
                    sp.Show();
                    break;
                case "Pie":

                    break;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            switch (Options[List.SelectedIndex].type)
            {
                case "Bar":
                    BarGraphOptions.Remove(Options[List.SelectedIndex].name);
                    break;
                case "Pie":

                    break;
            }
            PopulateList();
        }

        private void RenameButton_Click(object sender, EventArgs e)
        {
            switch (Options[List.SelectedIndex].type)
            {
                case "Bar":
                    InputBoxResult result = InputBox.Show("New Name:", "Rename Bar Graph", "", BarGraphOptions.ValidateNameHandler);
                    if (result.OK)
                    {
                        BarGraphOptions.Rename(Options[List.SelectedIndex].name, result.Text);
                    }
                    break;
                case "Pie":

                    break;
            }
            PopulateList();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            switch (Options[List.SelectedIndex].type)
            {
                case "Bar":
                    InputBoxResult result = InputBox.Show("Copy To:", "Copy Bar Graph", "", BarGraphOptions.ValidateNameHandler);
                    if (result.OK)
                    {

                        var o = BarGraphOptions.Load(Options[List.SelectedIndex].name);
                        o.Save(result.Text);
                    }
                    break;
                case "Pie":

                    break;
            }
            PopulateList();
        }
    }
}
