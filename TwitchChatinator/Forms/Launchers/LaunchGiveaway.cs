using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TwitchChatinator.Forms.Components;
using TwitchChatinator.Forms.Runners;
using TwitchChatinator.Forms.Setups;
using TwitchChatinator.Options;

namespace TwitchChatinator.Forms.Launchers
{
    public partial class LaunchGiveaway : Form
    {
        private RunGiveaway _giveaway;
        private List<SelectListObject> _options;
        private DateTime _startTime;

        public LaunchGiveaway()
        {
            InitializeComponent();

            PopulateList();
            RollButton.Enabled = false;

            InfoLabel.Text = "Stopped";

            FormClosed += LaunchGiveaway_FormClosed;

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

        private void LaunchGiveaway_FormClosed(object sender, FormClosedEventArgs e)
        {
            _giveaway?.Close();
        }

        private void PopulateList()
        {
            var originalIndex = List.SelectedIndex;

            List.Items.Clear();

            //Get Giveaways
            var giveaways = GiveawayOptions.GetAvaliable();

            _options = new List<SelectListObject>();

            foreach (var n in giveaways)
            {
                _options.Add(new SelectListObject(n, "Giveaway"));
                List.Items.Add(n);
            }


            if (List.Items.Count > 0)
            {
                List.SelectedIndex = originalIndex < List.Items.Count ? originalIndex : 0;
                EditButton.Enabled = true;
                DeleteButton.Enabled = true;
                CopyButton.Enabled = true;
                RenameButton.Enabled = true;
                StartButton.Enabled = true;
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

            if (_giveaway == null)
            {
                var gv = new RunGiveaway(_startTime, _options[List.SelectedIndex].Name, GiveawayTitle.Text, GiveawayKeyword.Text);
                gv.Show();
                gv.FormClosed += Giveaway_FormClosed;
                _giveaway = gv;
                StartButton.Text = @"Stop Giveaway";
                RollButton.Enabled = true;
                InfoLabel.Text = @"Started @ " + _startTime.ToString("h:mm t");
            }
            else
            {
                _giveaway.Close();
                RollButton.Enabled = false;
            }
        }

        private void Giveaway_FormClosed(object sender, FormClosedEventArgs e)
        {
            _giveaway = null;
            StartButton.Text = @"Start Giveaway";
            InfoLabel.Text = @"Stopped";
        }

        private void RollButton_Click(object sender, EventArgs e)
        {
            _giveaway?.Roll();
        }

        private void NewGiveawayButton_Click(object sender, EventArgs e)
        {
            var result = InputBox.Show("Name:", "New Giveaway Template", "", GiveawayOptions.ValidateNameHandler);
            if (result.Ok)
            {
                //TODO: Add Exception Control
                GiveawayOptions.CreateNew(result.Text);
            }
            PopulateList();
            EditGiveaway(result.Text);
        }

        private void EditGiveaway(string name)
        {
            var sp = new SetupGiveaway(name);
            sp.Show();
            Hide();
            sp.Closed += (o, args) => Show();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            switch (_options[List.SelectedIndex].Type)
            {
                case "Giveaway":
                    EditGiveaway(_options[List.SelectedIndex].Name);
                    break;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            switch (_options[List.SelectedIndex].Type)
            {
                case "Giveaway":
                    GiveawayOptions.Remove(_options[List.SelectedIndex].Name);
                    break;
            }
            PopulateList();
        }

        private void RenameButton_Click(object sender, EventArgs e)
        {
            switch (_options[List.SelectedIndex].Type)
            {
                case "Giveaway":
                    var result = InputBox.Show("New Name:", "Rename Giveaway Template", "",
                        GiveawayOptions.ValidateNameHandler);
                    if (result.Ok)
                    {
                        GiveawayOptions.Rename(_options[List.SelectedIndex].Name, result.Text);
                    }
                    break;
            }
            PopulateList();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            switch (_options[List.SelectedIndex].Type)
            {
                case "Giveaway":
                    var result = InputBox.Show("Copy To:", "Copy Bar Graph", "", GiveawayOptions.ValidateNameHandler);
                    if (result.Ok)
                    {
                        var o = GiveawayOptions.Load(_options[List.SelectedIndex].Name);
                        o.Save(result.Text);
                    }
                    break;
            }
            PopulateList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            GiveawayOptions.Export(_options[List.SelectedIndex].Name);
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "XBar Files (*.xroll)|*.xroll",
                Title = "Import Pie Graph"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.FileName.EndsWith(".xroll"))
                {
                    GiveawayOptions.Import(dialog.FileName);
                    PopulateList();
                }
                else
                {
                    MessageBox.Show(this, "Invalid File Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}