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
        List<TextBox> Inputs;
        Form Poll;
        DateTime StartTime;

        public LaunchPoll()
        {
            InitializeComponent();

            PopulateList();

            Inputs = new List<TextBox>();

            AddInput("Yes");
            AddInput("No");
            AddInput("");
            StartTime = DateTime.Now;

            InfoLabel.Text = "Stopped | " + StartTime.ToString("h:mm t");

            FormClosed += LaunchPoll_FormClosed;

            if (List.Items.Count > 0)
            {
                List.SelectedIndex = 0;
                EditButton.Enabled = true;
                DeleteButton.Enabled = true;
                CopyButton.Enabled = true;
                RenameButton.Enabled = true;
                StartButton.Enabled = true;
            }
            else
            {
                StartButton.Enabled = false;
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
                CopyButton.Enabled = false;
                RenameButton.Enabled = false;
            }
        }

        void LaunchPoll_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(Poll != null)
            {
                Poll.Close();
            }
        }

        void AddInput()
        {
            AddInput("");
        }

        void AddInput(string option)
        {
            var Input = new TextBox();
            Input.Width = Width - (SystemInformation.BorderSize.Width * 2) - 36;
            Input.Left = 12;
            Input.Text = option;

            Controls.Add(Input);

            Input.GotFocus += Input_GotFocus;
            Input.LostFocus += Input_LostFocus;
            Input.KeyUp += Input_KeyUp;

            Inputs.Add(Input);

            SetWindowSize();
            PositionInputs();
        }

        void Input_GotFocus(object sender, EventArgs e)
        {
            var self = (TextBox)sender;

            self.SelectAll();
        }

        void Input_KeyUp(object sender, KeyEventArgs e)
        {
            var self = (TextBox)sender;
            
            if (self.Text != String.Empty && self.Equals(Inputs.Last()))
            {
                AddInput("");
                SetWindowSize();
                PositionInputs();
            }
        }

        private void Input_LostFocus(object sender, EventArgs e)
        {
            var self = (TextBox)sender;
            
            if (self.Text == String.Empty && !self.Equals(Inputs.Last()))
            {
                Inputs.Remove(self);
                self.Dispose();
                self = null;
                SetWindowSize();
                PositionInputs();
            }
        }

        void PositionInputs()
        {
            for (int i = 0; i < Inputs.Count; i++)
            {
                Inputs[i].Top = i * 30 + 100;
            }
        }

        void SetWindowSize()
        {
            Height = Inputs.Count * 30 + 160;
            StartButton.Top = Height - 65;
            InfoLabel.Top = Height - 63;
        }        

        void PopulateList()
        {
            List.Items.Clear();

            //Get Bar Graphs
            List<string> BarGraphs = BarGraphOptions.GetAvaliable();

            //Get Pie Graphs - TODO
            List<string> PieGraphs = PieGraphOptions.GetAvaliable();

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
                EditButton.Enabled = true;
                DeleteButton.Enabled = true;
                CopyButton.Enabled = true;
                RenameButton.Enabled = true;
            }
            else
            {
                StartButton.Enabled = false;
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
                CopyButton.Enabled = false;
                RenameButton.Enabled = false;
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (Poll == null)
            {
                List<string> Labels = new List<string>();

                foreach (var Input in Inputs)
                {
                    if (Input.Text.Trim() != String.Empty)
                    {
                        Labels.Add(Input.Text.Trim());
                    }
                }

                switch (Options[List.SelectedIndex].type)
                {
                    case "Bar":
                        RunPollBar RPB = new RunPollBar(StartTime, Options[List.SelectedIndex].name, PollTitle.Text, Labels.ToArray());
                        RPB.Show();
                        RPB.FormClosed += Poll_FormClosed;
                        Poll = RPB;
                        StartButton.Text = "Stop Poll";
                        break;
                    case "Pie":
                        RunPollPie RPP = new RunPollPie(StartTime, Options[List.SelectedIndex].name, PollTitle.Text, Labels.ToArray());
                        RPP.Show();
                        RPP.FormClosed += Poll_FormClosed;
                        Poll = RPP;
                        StartButton.Text = "Stop Poll";
                        break;
                }
                InfoLabel.Text = "Started | " + StartTime.ToString("h:mm t");
            }
            else
            {
                Poll.Close();
                StartButton.Text = "Start Poll";
                InfoLabel.Text = "Stopped | " + StartTime.ToString("h:mm t");
            }
        }

        void Poll_FormClosed(object sender, FormClosedEventArgs e)
        {
            Poll = null;
            StartButton.Text = "Start Poll";
        }

        void editBarGraph(string name)
        {
            var sb = new SetupBarGraph(name);
            sb.Show();
            Hide();
            sb.Closed += (o, args) => Show();
        }

        void editPieGraph(string name)
        {
            var sp = new SetupPieGraph(name);
            sp.Show();
            Hide();
            sp.Closed += (o, args) => Show();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            switch (Options[List.SelectedIndex].type)
            {
                case "Bar":
                    editBarGraph(Options[List.SelectedIndex].name);
                    break;
                case "Pie":
                    editPieGraph(Options[List.SelectedIndex].name);
                    break;
            }
        }

        private void RenameButton_Click(object sender, EventArgs e)
        {
            InputBoxResult result;
            switch (Options[List.SelectedIndex].type)
            {
                case "Bar":
                    result = InputBox.Show("New Name:", "Rename Bar Graph", "", BarGraphOptions.ValidateNameHandler);
                    if (result.OK)
                    {
                        BarGraphOptions.Rename(Options[List.SelectedIndex].name, result.Text);
                    }
                    break;
                case "Pie":
                    result = InputBox.Show("New Name:", "Rename Pie Graph", "", PieGraphOptions.ValidateNameHandler);
                    if (result.OK)
                    {
                        PieGraphOptions.Rename(Options[List.SelectedIndex].name, result.Text);
                    }
                    break;
            }
            PopulateList();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            switch (Options[List.SelectedIndex].type)
            {
                case "Bar":
                    BarGraphOptions.Remove(Options[List.SelectedIndex].name);
                    break;
                case "Pie":
                    PieGraphOptions.Remove(Options[List.SelectedIndex].name);
                    break;
            }
            PopulateList();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            InputBoxResult result;
            switch (Options[List.SelectedIndex].type)
            {
                case "Bar":
                    result = InputBox.Show("Copy To:", "Copy Bar Graph", "", BarGraphOptions.ValidateNameHandler);
                    if (result.OK)
                    {

                        var o = BarGraphOptions.Load(Options[List.SelectedIndex].name);
                        o.Save(result.Text);
                    }
                    break;
                case "Pie":
                    result = InputBox.Show("Copy To:", "Copy Pie Graph", "", BarGraphOptions.ValidateNameHandler);
                    if (result.OK)
                    {

                        var o = PieGraphOptions.Load(Options[List.SelectedIndex].name);
                        o.Save(result.Text);
                    }
                    break;
            }
            PopulateList();
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
            editBarGraph(result.Text);
        }

        private void NewPieButton_Click(object sender, EventArgs e)
        {
            InputBoxResult result = InputBox.Show("Name:", "New Pie Graph", "", PieGraphOptions.ValidateNameHandler);
            if (result.OK)
            {
                //TODO: Add Exception Control
                PieGraphOptions.CreateNew(result.Text);
            }
            PopulateList();        
            editPieGraph(result.Text);  
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            switch (Options[List.SelectedIndex].type)
            {
                case "Bar":
                    BarGraphOptions.Export(Options[List.SelectedIndex].name);
                    break;
                case "Pie":
                    PieGraphOptions.Export(Options[List.SelectedIndex].name);
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
