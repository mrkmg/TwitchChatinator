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

        private void ShowPollConfig_Click(object sender, EventArgs e)
        {
            var Manager = new ManagePolls();
            Manager.Show();
        }

        private void StartPollButton_Click(object sender, EventArgs e)
        {
        }

        private void StartRollButton_Click(object sender, EventArgs e)
        {
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
