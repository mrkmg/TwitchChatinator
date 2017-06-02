using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TwitchChatinator.Forms.Components;
using TwitchChatinator.Forms.Runners;
using TwitchChatinator.Forms.Setups;
using TwitchChatinator.Options;

namespace TwitchChatinator.Forms.Launchers
{
    public partial class LaunchPoll : Form
    {
        private readonly List<TextBox> _inputs;
        private List<SelectListObject> _options;
        private Form _poll;
        private DateTime _startTime;

        public LaunchPoll()
        {
            InitializeComponent();

            PopulateList();

            _inputs = new List<TextBox>();

            AddInput("Yes");
            AddInput("No");
            AddInput();

            InfoLabel.Text = @"Stopped";

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

        private void LaunchPoll_FormClosed(object sender, FormClosedEventArgs e)
        {
            _poll?.Close();
        }

        private void AddInput(string option = "")
        {
            var input = new TextBox
            {
                Width = Width - SystemInformation.BorderSize.Width*2 - 36,
                Left = 12,
                Text = option
            };

            Controls.Add(input);

            input.GotFocus += Input_GotFocus;
            input.LostFocus += Input_LostFocus;
            input.KeyUp += Input_KeyUp;

            _inputs.Add(input);

            SetWindowSize();
            PositionInputs();
        }

        private static void Input_GotFocus(object sender, EventArgs e)
        {
            var self = (TextBox) sender;

            self.SelectAll();
        }

        private void Input_KeyUp(object sender, KeyEventArgs e)
        {
            var self = (TextBox) sender;

            if (self.Text == string.Empty || !self.Equals(_inputs.Last())) return;

            AddInput();
            SetWindowSize();
            PositionInputs();
        }

        private void Input_LostFocus(object sender, EventArgs e)
        {
            var self = (TextBox) sender;

            if (self.Text != string.Empty || self.Equals(_inputs.Last())) return;

            _inputs.Remove(self);
            self.Dispose();
            SetWindowSize();
            PositionInputs();
        }

        private void PositionInputs()
        {
            for (var i = 0; i < _inputs.Count; i++)
            {
                _inputs[i].Top = i*30 + 100;
            }
        }

        private void SetWindowSize()
        {
            Height = _inputs.Count*30 + 160;
            StartButton.Top = Height - 65;
            InfoLabel.Top = Height - 63;
        }

        private void PopulateList()
        {
            List.Items.Clear();

            //Get Bar Graphs
            var barGraphs = BarGraphOptions.GetAvaliable();

            //Get Pie Graphs - TODO
            var pieGraphs = PieGraphOptions.GetAvaliable();

            _options = new List<SelectListObject>();

            foreach (var n in barGraphs)
            {
                _options.Add(new SelectListObject(n, "Bar"));
                List.Items.Add("Bar - " + n);
            }
            foreach (var n in pieGraphs)
            {
                _options.Add(new SelectListObject(n, "Pie"));
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
                ExportButton.Enabled = true;
            }
            else
            {
                StartButton.Enabled = false;
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
                CopyButton.Enabled = false;
                RenameButton.Enabled = false;
                ExportButton.Enabled = false;
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            _startTime = DateTime.Now;
            if (_poll == null)
            {
                var labels = new List<string>();

                foreach (var input in _inputs)
                {
                    if (input.Text.Trim() != string.Empty)
                    {
                        labels.Add(input.Text.Trim());
                    }
                }

                switch (_options[List.SelectedIndex].Type)
                {
                    case "Bar":
                        var rpb = new RunPollBar(_startTime, _options[List.SelectedIndex].Name, PollTitle.Text,
                            labels.ToArray());
                        rpb.Show();
                        rpb.FormClosed += Poll_FormClosed;
                        _poll = rpb;
                        StartButton.Text = @"Stop Poll";
                        break;
                    case "Pie":
                        var rpp = new RunPollPie(_startTime, _options[List.SelectedIndex].Name, PollTitle.Text,
                            labels.ToArray());
                        rpp.Show();
                        rpp.FormClosed += Poll_FormClosed;
                        _poll = rpp;
                        StartButton.Text = @"Stop Poll";
                        break;
                }
                InfoLabel.Text = @"Started @ " + _startTime.ToString("h:mm t");
            }
            else
            {
                _poll.Close();
            }
        }

        private void Poll_FormClosed(object sender, FormClosedEventArgs e)
        {
            _poll = null;
            StartButton.Text = @"Start Poll";
            InfoLabel.Text = @"Stopped";
        }

        private void EditBarGraph(string name)
        {
            var sb = new SetupBarGraph(name);
            sb.Show();
            Hide();
            sb.Closed += (o, args) => Show();
        }

        private void EditPieGraph(string name)
        {
            var sp = new SetupPieGraph(name);
            sp.Show();
            Hide();
            sp.Closed += (o, args) => Show();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            switch (_options[List.SelectedIndex].Type)
            {
                case "Bar":
                    EditBarGraph(_options[List.SelectedIndex].Name);
                    break;
                case "Pie":
                    EditPieGraph(_options[List.SelectedIndex].Name);
                    break;
                default:
                    throw new Exception("Unknown Type: " + _options[List.SelectedIndex].Type);
            }
        }

        private void RenameButton_Click(object sender, EventArgs e)
        {
            InputBoxResult result;
            switch (_options[List.SelectedIndex].Type)
            {
                case "Bar":
                    result = InputBox.Show("New Name:", "Rename Bar Graph", "", BarGraphOptions.ValidateNameHandler);
                    if (result.Ok)
                    {
                        BarGraphOptions.Rename(_options[List.SelectedIndex].Name, result.Text);
                    }
                    break;
                case "Pie":
                    result = InputBox.Show("New Name:", "Rename Pie Graph", "", PieGraphOptions.ValidateNameHandler);
                    if (result.Ok)
                    {
                        PieGraphOptions.Rename(_options[List.SelectedIndex].Name, result.Text);
                    }
                    break;
                default:
                    throw new Exception("Unknown Type: " + _options[List.SelectedIndex].Type);
            }
            PopulateList();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            switch (_options[List.SelectedIndex].Type)
            {
                case "Bar":
                    BarGraphOptions.Remove(_options[List.SelectedIndex].Name);
                    break;
                case "Pie":
                    PieGraphOptions.Remove(_options[List.SelectedIndex].Name);
                    break;
                default:
                    throw new Exception("Unknown Type: " + _options[List.SelectedIndex].Type);
            }
            PopulateList();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            InputBoxResult result;
            switch (_options[List.SelectedIndex].Type)
            {
                case "Bar":
                    result = InputBox.Show("Copy To:", "Copy Bar Graph", "", BarGraphOptions.ValidateNameHandler);
                    if (result.Ok)
                    {
                        var o = BarGraphOptions.Load(_options[List.SelectedIndex].Name);
                        o.Save(result.Text);
                    }
                    break;
                case "Pie":
                    result = InputBox.Show("Copy To:", "Copy Pie Graph", "", BarGraphOptions.ValidateNameHandler);
                    if (result.Ok)
                    {
                        var o = PieGraphOptions.Load(_options[List.SelectedIndex].Name);
                        o.Save(result.Text);
                    }
                    break;
                default:
                    throw new Exception("Unknown Type: " + _options[List.SelectedIndex].Type);
            }
            PopulateList();
        }

        private void NewBarButton_Click(object sender, EventArgs e)
        {
            var result = InputBox.Show("Name:", "New Bar Graph", "", BarGraphOptions.ValidateNameHandler);
            if (result.Ok)
            {
                //TODO: Add Exception Control
                BarGraphOptions.CreateNew(result.Text);
            }
            PopulateList();
            EditBarGraph(result.Text);
        }

        private void NewPieButton_Click(object sender, EventArgs e)
        {
            var result = InputBox.Show("Name:", "New Pie Graph", "", PieGraphOptions.ValidateNameHandler);
            if (result.Ok)
            {
                //TODO: Add Exception Control
                PieGraphOptions.CreateNew(result.Text);
            }
            PopulateList();
            EditPieGraph(result.Text);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            switch (_options[List.SelectedIndex].Type)
            {
                case "Bar":
                    BarGraphOptions.Export(_options[List.SelectedIndex].Name);
                    break;
                case "Pie":
                    PieGraphOptions.Export(_options[List.SelectedIndex].Name);
                    break;
                default:
                    throw new Exception("Unknown Type: " + _options[List.SelectedIndex].Type);
            }
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "XBar Files (*.xbar)|*.xbar|XPie Files (*.xpie)|*.xpie",
                Title = "Import Pie Graph"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.FileName.EndsWith(".xbar"))
                {
                    BarGraphOptions.Import(dialog.FileName);
                    PopulateList();
                }
                else if (dialog.FileName.EndsWith(".xpie"))
                {
                    PieGraphOptions.Import(dialog.FileName);
                    PopulateList();
                }
                else
                {
                    MessageBox.Show(this, "Invalid File Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    public class PollData
    {
        public int[] Amounts;
        public string[] Options;
        public int TotalVotes = 0;

        public PollData(int count)
        {
            Options = new string[count];
            Amounts = new int[count];
        }
    }
}