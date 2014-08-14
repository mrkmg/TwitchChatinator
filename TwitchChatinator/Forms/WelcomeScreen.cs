using System;
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
            this.ShowIcon = false;
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
        }

        void TI_OnConnected()
        {
            Invoke(new Action(OnListenConnected));
        }

        void OnListenConnected()
        {
            ListeningStatus.Image = Properties.Resources.Green;
            StartListenButton.Text = "Stop Listening";
            StartListenButton.Enabled = true;
            isConnected = true;
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
        }

        public void StopListen()
        {
            TI.Stop();
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
                StartListenButton.Text = "Disconnecting";
            }
            else
            {
                StartListen();
                StartListenButton.Text = "Connecting";
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
    }
}
