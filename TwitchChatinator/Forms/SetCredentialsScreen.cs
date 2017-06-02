using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using TwitchChatinator.Libs;
using Timer = System.Windows.Forms.Timer;

namespace TwitchChatinator.Forms
{
    public partial class SetCredentialsScreen : Form
    {
        public delegate void Login();

        delegate void StringArgReturningVoidDelegate(string text);

        private readonly Login _loginHandler;
        private Form _authBrowserForm;

        public SetCredentialsScreen(Login l)
        {
            _loginHandler = l;
            InitializeComponent();

            UsernameInput.KeyUp += TriggerCheckInputs;
            PasswordInput.KeyUp += TriggerCheckInputs;
            ChannelInput.KeyUp += TriggerCheckInputs;
            UsernameInput.KeyUp += CopyUserToChannel;

            if (Settings.Default.TwitchUsername != "" && Settings.Default.TwitchPassword != "" &&
                Settings.Default.TwitchChannel != "")
            {
                LoginButton.Enabled = true;
                UsernameInput.Text = Settings.Default.TwitchUsername;
                PasswordInput.Text = Settings.Default.TwitchPassword;
                ChannelInput.Text = Settings.Default.TwitchChannel;
            }
            else LoginButton.Enabled = false;
        }

        public void CopyUserToChannel(object sender, KeyEventArgs e)
        {
            ChannelInput.Text = UsernameInput.Text.ToLower();
            CheckInputs();
        }

        private void CheckInputs()
        {
            if (UsernameInput.Text.Length > 1 && PasswordInput.Text.Length > 1 && ChannelInput.Text.Length > 1)
                LoginButton.Enabled = true;
            else LoginButton.Enabled = false;
        }

        private void TriggerCheckInputs(object send, KeyEventArgs e)
        {
            CheckInputs();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            Settings.Default.TwitchUsername = UsernameInput.Text;
            Settings.Default.TwitchPassword = PasswordInput.Text;
            Settings.Default.TwitchChannel = ChannelInput.Text;
            Settings.Default.Save();

            _loginHandler();
            Close();
        }

        private void CancelButtonC_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GetTokenLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var width = 400;
            var height = 600;
            var authenticationServer = new AuthenticationWebserver();
            authenticationServer.Run();
            authenticationServer.OnReceivedAuthCode += SetPassword;

            _authBrowserForm = new Form()
            {
                Width = width, Height = height, ShowIcon = false
            };
            var webview = new WebBrowser()
            {
                Url = new Uri("http://localhost:8080"), Top = 0, Left = 0, Width = width, Height = height
            };

            _authBrowserForm.Controls.Add(webview);
            _authBrowserForm.Show();
            _authBrowserForm.Closed += (o, args) => authenticationServer.Stop();
        }

        private void SetPassword(string code)
        {
            if (PasswordInput.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = SetPassword;
                Invoke(d, code);
            }
            else
            {
                PasswordInput.Text = code;
                _authBrowserForm?.Close();
            }
        }
    }
}