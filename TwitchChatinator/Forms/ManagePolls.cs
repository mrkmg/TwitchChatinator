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
        }

        private void NewBarButton_Click(object sender, EventArgs e)
        {
            InputBoxResult result = InputBox.Show("Name:", "New Bar Graph", "", null);
            if (result.OK)
            {
                //TODO: Add Exception Control
                BarGraphOptions.CreateNew(result.Text);
                PopulateList();
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (Options.Count != 0)
            {
                switch(Options[List.SelectedIndex].type){
                    case "Bar":
                        var sp = new SetupBarGraph(Options[List.SelectedIndex].name);
                        sp.Show();
                        break;
                    case "Pie":

                        break;
                }
            }
        }
    }

    class SelectListObject
    {
        public string name { get; set; }
        public string type { get; set; }

        public SelectListObject()
        {

        }

        public SelectListObject(string n, string t)
        {
            name = n;
            type = t;
        }
    }
}
