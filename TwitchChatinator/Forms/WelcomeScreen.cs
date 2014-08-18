﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitchChatinator
{
    public partial class WelcomeScreen : Form
    {
        public TwitchIRC TI;
        public RunPoll RP;
        public RunRoll RR;
        DateTime PollStart;
        DateTime RollStart;

        private bool isConnected = false;


        public WelcomeScreen()
        {
            InitializeComponent();
            this.FormClosed += WelcomeScreen_FormClosed;

            TI = new TwitchIRC();
            TI.OnReceiveMessage += ReceiveMessage;
            TI.OnConnected += TI_OnConnected;
            TI.OnDisconnected += TI_OnDisconnected;

            if (Settings.Default.TwitchUsername != "" && Settings.Default.TwitchPassword != "")
            {
                ShowListenButton();
            }
        }

        void TI_OnDisconnected()
        {
            try
            {
                Invoke(new Action(OnListenDisconnected));
            }
            catch (Exception e)
            {
                Log.LogException(e);
            }
            
        }

        void OnListenDisconnected()
        {
            ListeningStatus.Image = Properties.Resources.Red;
            StartListenButton.Text = "Start Listening";
            StartListenButton.Enabled = true;
            isConnected = false;
            ConnectedLabel.Text = "Not Connected";
        }

        void TI_OnConnected(string channel)
        {
            Invoke(new Action<string>(OnListenConnected), new string[1] { channel });
        }

        void OnListenConnected(string channel)
        {
            ListeningStatus.Image = Properties.Resources.Green;
            StartListenButton.Text = "Stop Listening";
            StartListenButton.Enabled = true;
            isConnected = true;
            ConnectedLabel.Text = "Connected to " + channel;
        }

        void WelcomeScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopListen();
            if (RP != null && !RP.IsDisposed) RP.Close();
            Console.WriteLine("Good Bye");
        }

        private void ReceiveMessage(TwitchMessageObject Message)
        {
        }

        public void ShowListenButton()
        {
            this.StartListenButton.Visible = true;
            this.ListeningStatus.Visible = true;
        }

        public void StartListen()
        {
            TI.Start();
            ConnectedLabel.Text = "Connecting";
        }

        public void StopListen()
        {
            TI.Stop();
            ConnectedLabel.Text = "Disconnecting";
        }

        private void SetCredentialsButton_Click(object sender, EventArgs e)
        {
            SetCredentialsScreen LS = new SetCredentialsScreen(ShowListenButton);
            LS.ShowDialog();
        }

        private void StartListenButton_Click(object sender, EventArgs e)
        {
            if(isConnected)
            {
                StopListen();
                StartListenButton.Text = "Please Wait";
            }
            else
            {
                StartListen();
                StartListenButton.Text = "Please Wait";
            }
            StartListenButton.Enabled = false;
        }

        private void ShowMessageBrowser_Click(object sender, EventArgs e)
        {
            MessageBrowser MB = new MessageBrowser();
            MB.Show();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            string u = Settings.Default.TwitchUsername;
            string p = Settings.Default.TwitchPassword;
            string c = Settings.Default.TwithChannel;
            Settings.Default.Reset();
            Settings.Default.TwitchUsername = u;
            Settings.Default.TwitchPassword = p;
            Settings.Default.TwithChannel = c;
            Settings.Default.Save();

            if (RP != null && !RP.IsDisposed)
                StartRunPoll();

            if (RR != null && !RR.IsDisposed)
                StartRunRoll();
        }

        private void ShowPollConfig_Click(object sender, EventArgs e)
        {
            PollSetup PS = new PollSetup();
            PS.OnSave += PS_OnSave;

            PS.Show();
        }

        void PS_OnSave()
        {
            if(RP != null && !RP.IsDisposed)
                StartRunPoll();
        }

        private void StartPollButton_Click(object sender, EventArgs e)
        {
            PollStart = DateTime.Now;
            StartRunPoll();
        }

        void StartRunPoll()
        {
            if (RP != null && !RP.IsDisposed)
            {
                RP.Close();
                while (!RP.IsDisposed) Thread.Sleep(50);
                RP = new RunPoll(PollStart);
            }
            else
            {
                RP = new RunPoll(PollStart);
            }
            RP.Show();
        }

        private void StartRollButton_Click(object sender, EventArgs e)
        {
            RollStart = DateTime.Now;
            StartRunRoll();
        }

        void StartRunRoll()
        {
            if (RR != null && !RR.IsDisposed)
            {
                RR.Close();
                while (!RR.IsDisposed) Thread.Sleep(50);
                RR = new RunRoll(RollStart);
            }
            else
            {
                RR = new RunRoll(RollStart);
            }
            RR.Show();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            TI.Dispose();

            base.Dispose(disposing);
        }

        private void SetupRollButton_Click(object sender, EventArgs e)
        {
            SetupRoll SR = new SetupRoll();
            SR.OnSave += SR_OnSave;

            SR.Show();
        }

        void SR_OnSave()
        {
            if (RR != null && !RR.IsDisposed)
                StartRunRoll();
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog D = new SaveFileDialog();
            D.AddExtension = true;
            D.DefaultExt = "csv";
            D.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

            if (D.ShowDialog() == DialogResult.OK)
            {
                DataStore.ExportToCsv(D.FileName);
            }
        }

        private void CopyRandomButton_Click(object sender, EventArgs e)
        {
            var DSS = new DataSetSelection();
            DSS.Start = DateTime.Now.AddMinutes(-5);

            List<string> users = DataStore.GetUniqueUsersString(DSS);
            Random r = new Random();
            string winner = users[(int)(r.NextDouble()*(users.Count-1))];
            Clipboard.SetText(winner);
        }
    }
}
