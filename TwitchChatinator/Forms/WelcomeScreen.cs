using System;
using System.Threading;
using System.Windows.Forms;
using TwitchChatinator.Forms.Launchers;
using TwitchChatinator.Libs;
using TwitchChatinator.Properties;
using Timer = System.Windows.Forms.Timer;

namespace TwitchChatinator.Forms
{
    public partial class WelcomeScreen : Form
    {
        private bool _isConnected;
        public TwitchIrc Ti;
        private string _channel;
        private Thread _getUsersThread;


        public WelcomeScreen()
        {
            InitializeComponent();

            VersionLabel.Text = @"v " + Program.GetVersion();

            FormClosed += WelcomeScreen_FormClosed;

            Ti = new TwitchIrc();
            Ti.OnReceiveMessage += ReceiveMessage;
            Ti.OnConnected += TI_OnConnected;
            Ti.OnDisconnected += TI_OnDisconnected;
            Ti.OnUserJoin += Ti_OnUserJoin;
            Ti.OnUserLeave += Ti_OnUserLeave;

            if (Settings.Default.TwitchUsername != "" && Settings.Default.TwitchPassword != "")
            {
                ShowListenButton();
            }
        }

        private void Ti_OnUserLeave(string user)
        {
            Log.LogInfo("User Leave\t" + user);
        }

        private void Ti_OnUserJoin(string user)
        {
            Log.LogInfo("User Join\t" + user);
            DataStore.AddUser(user);
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
            _channel = channel;

            if (_getUsersThread != null && _getUsersThread.IsAlive)
            {
                _getUsersThread.Abort();
            }
            _getUsersThread = new Thread(GetChatters);
            _getUsersThread.Start();
        }

        private void GetChatters()
        {
            Log.LogInfo("Retreiving chatters");
            var chattersFromApi = TwitchApi.ChattersInChannel(_channel);
            double totalToProcess = chattersFromApi.chatters.moderators.Count + chattersFromApi.chatters.viewers.Count;

            Log.LogInfo("Retreived chatters " + totalToProcess);

            foreach (var chatter in chattersFromApi.chatters.moderators)
            {
                DataStore.AddUser(chatter);
            }

            foreach (var chatter in chattersFromApi.chatters.viewers)
            {
                DataStore.AddUser(chatter);
            }
        }

        private void WelcomeScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopListen();
        }

        private void ReceiveMessage(TwitchMessageObject message)
        { 
            DataStore.InsertMessage(message.Channel, message.Username, message.Message);
        }

        public void ShowListenButton()
        {
            StartListenButton.Visible = true;
            ListeningStatus.Visible = true;
        }

        public void StartListen()
        {
            DataStore.ResetUsers();
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

            if (_getUsersThread != null && _getUsersThread.IsAlive)
            {
                _getUsersThread.Abort();
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

        private Timer copyRandomUserTimer;

        private void CopyRandomButton_Click(object sender, EventArgs e)
        {
            var users = DataStore.GetOnlineUsers();
            var r = new Random();
            if (users.Count > 0)
            {
                var winner = users[ (int)Math.Round(r.NextDouble()*(users.Count - 1)) ];
                Clipboard.SetText(winner);
                ((Button) sender).Text = winner;
            }
            else
            {
                ((Button) sender).Text = @"NO ENTRIES";
            }
            copyRandomUserTimer?.Dispose();

            copyRandomUserTimer = new Timer();
            copyRandomUserTimer.Tick += delegate
            {
                ((Button) sender).Text = @"Copy Random User";
                copyRandomUserTimer.Stop();
                copyRandomUserTimer.Dispose();
                copyRandomUserTimer = null;
            };
            copyRandomUserTimer.Interval = 1500;
            copyRandomUserTimer.Start();
        }
    }
}