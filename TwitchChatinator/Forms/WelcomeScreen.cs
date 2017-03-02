using System;
using System.Windows.Forms;
using TwitchChatinator.Forms.Launchers;
using TwitchChatinator.Libs;
using TwitchChatinator.Properties;

namespace TwitchChatinator.Forms
{
    public partial class WelcomeScreen : Form
    {
        private bool _isConnected;
        public TwitchIrc Ti;


        public WelcomeScreen()
        {
            InitializeComponent();

            VersionLabel.Text = @"V" + Application.ProductVersion;

            FormClosed += WelcomeScreen_FormClosed;

            Ti = new TwitchIrc();
            Ti.OnReceiveMessage += ReceiveMessage;
            Ti.OnConnected += TI_OnConnected;
            Ti.OnDisconnected += TI_OnDisconnected;

            if (Settings.Default.TwitchUsername != "" && Settings.Default.TwitchPassword != "")
            {
                ShowListenButton();
            }
        }

        private void TI_OnDisconnected()
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

        private void OnListenDisconnected()
        {
            ListeningStatus.Image = Resources.Red;
            StartListenButton.Text = @"Start Listening";
            StartListenButton.Enabled = true;
            _isConnected = false;
            ConnectedLabel.Text = @"Not Connected";
        }

        private void TI_OnConnected(string channel)
        {
            Invoke(new Action<string>(OnListenConnected), channel);
        }

        private void OnListenConnected(string channel)
        {
            ListeningStatus.Image = Resources.Green;
            StartListenButton.Text = @"Stop Listening";
            StartListenButton.Enabled = true;
            _isConnected = true;
            ConnectedLabel.Text = channel;
        }

        private void WelcomeScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopListen();
            Console.WriteLine(@"Good Bye");
        }

        private void ReceiveMessage(TwitchMessageObject message)
        {
        }

        public void ShowListenButton()
        {
            StartListenButton.Visible = true;
            ListeningStatus.Visible = true;
        }

        public void StartListen()
        {
            Ti.Start();
            ConnectedLabel.Text = @"Connecting";
        }

        public void StopListen()
        {
            Ti.Stop();
            ConnectedLabel.Text = @"Disconnecting";
        }

        private void SetCredentialsButton_Click(object sender, EventArgs e)
        {
            var ls = new SetCredentialsScreen(ShowListenButton);
            ls.ShowDialog();
        }

        private void StartListenButton_Click(object sender, EventArgs e)
        {
            if (_isConnected)
            {
                StopListen();
                StartListenButton.Text = @"Please Wait";
            }
            else
            {
                StartListen();
                StartListenButton.Text = @"Please Wait";
            }
            StartListenButton.Enabled = false;
        }

        private void ShowMessageBrowser_Click(object sender, EventArgs e)
        {
            var mb = new MessageBrowser();
            mb.Show();
        }

        private void StartPollButton_Click(object sender, EventArgs e)
        {
            var launcher = new LaunchPoll();
            launcher.Show();
        }

        private void StartRollButton_Click(object sender, EventArgs e)
        {
            var launcher = new LaunchGiveaway();
            launcher.Show();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }

            Ti.Dispose();

            base.Dispose(disposing);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            var d = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = "csv",
                Filter = @"CSV files (*.csv)|*.csv|All files (*.*)|*.*"
            };

            if (d.ShowDialog() == DialogResult.OK)
            {
                DataStore.ExportToCsv(d.FileName);
            }
        }

        private void CopyRandomButton_Click(object sender, EventArgs e)
        {
            var dss = new DataSetSelection {Start = DateTime.Now.AddMinutes(-5)};

            var users = DataStore.GetUniqueUsersString(dss);
            var r = new Random();
            if (users.Count > 0)
            {
                var winner = users[(int) (r.NextDouble()*(users.Count - 1))];
                Clipboard.SetText(winner);
                ((Button) sender).Text = winner;
            }
            else
            {
                ((Button) sender).Text = @"NO ENTRIES";
            }
            var T = new Timer();
            //TODO - Make this timer go away if clicked again.
            T.Tick += delegate
            {
                ((Button) sender).Text = @"Copy Random User";
                T.Stop();
                T.Dispose();
            };
            T.Interval = 1500;
            T.Start();
        }
    }
}