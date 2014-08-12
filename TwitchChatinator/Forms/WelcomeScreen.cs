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

        private void ShowPollConfig_Click(object sender, EventArgs e)
        {
            PollSetup PS = new PollSetup();

            PS.ShowDialog();
        }

        private void StartPollButton_Click(object sender, EventArgs e)
        {
            if (RP == null || RP.IsDisposed)
            {
                RP = new RunPoll();
                RP.Show();
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            Settings.Default.Reset();
            Close();
        }
    }
}
