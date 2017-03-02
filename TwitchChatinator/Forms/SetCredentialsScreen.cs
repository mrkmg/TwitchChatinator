using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TwitchChatinator.Forms
{
    public partial class SetCredentialsScreen : Form
    {
        public delegate void Login();

        private readonly Login _loginHandler;

        public SetCredentialsScreen(Login l)
        {
            _loginHandler = l;
            InitializeComponent();

            UsernameInput.KeyUp += CheckInputs;
            PasswordInput.KeyUp += CheckInputs;
            ChannelInput.KeyUp += CheckInputs;
            UsernameInput.KeyUp += CopyUserToChannel;

            if (Settings.Default.TwitchUsername != "" && Settings.Default.TwitchPassword != "" &&
                Settings.Default.TwithChannel != "")
            {
                LoginButton.Enabled = true;
                UsernameInput.Text = Settings.Default.TwitchUsername;
                PasswordInput.Text = Settings.Default.TwitchPassword;
                ChannelInput.Text = Settings.Default.TwithChannel;
            }
            else LoginButton.Enabled = false;
        }

        public void CopyUserToChannel(object sender, KeyEventArgs e)
        {
            ChannelInput.Text = UsernameInput.Text.ToLower();
        }

        private void CheckInputs(object sender, KeyEventArgs e)
        {
            if (UsernameInput.Text.Length > 1 && PasswordInput.Text.Length > 1 && ChannelInput.Text.Length > 1)
                LoginButton.Enabled = true;
            else LoginButton.Enabled = false;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            Settings.Default.TwitchUsername = UsernameInput.Text;
            Settings.Default.TwitchPassword = PasswordInput.Text;
            Settings.Default.TwithChannel = ChannelInput.Text;
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
            Process.Start("http://www.twitchapps.com/tmi/");
        }
    }
}