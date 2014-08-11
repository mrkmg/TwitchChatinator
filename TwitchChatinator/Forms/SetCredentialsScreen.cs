using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitchChatinator
{
    public partial class SetCredentialsScreen : Form
    {

        public delegate void Login();

        Login LoginHandler;

        public SetCredentialsScreen(Login l)
        {
            LoginHandler = l;
            InitializeComponent();

            UsernameInput.KeyUp += checkInputs;
            PasswordInput.KeyUp += checkInputs;
            ChannelInput.KeyUp += checkInputs;
            UsernameInput.KeyUp += copyUserToChannel;

            if (Settings.Default.TwitchUsername != "" && Settings.Default.TwitchPassword != "" && Settings.Default.TwithChannel != ""){
                LoginButton.Enabled = true;
                UsernameInput.Text = Settings.Default.TwitchUsername;
                PasswordInput.Text = Settings.Default.TwitchPassword;
                ChannelInput.Text = Settings.Default.TwithChannel;
            }
            else LoginButton.Enabled = false;
        }

        public void copyUserToChannel(object sender, KeyEventArgs e)
        {
            ChannelInput.Text = UsernameInput.Text.ToLower();
        }

        private void checkInputs(object sender, KeyEventArgs e)
        {
            if (UsernameInput.Text.Length > 1 && PasswordInput.Text.Length > 1 && ChannelInput.Text.Length > 1) LoginButton.Enabled = true;
            else LoginButton.Enabled = false;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            Settings.Default.TwitchUsername = this.UsernameInput.Text;
            Settings.Default.TwitchPassword = this.PasswordInput.Text;
            Settings.Default.TwithChannel = this.ChannelInput.Text;
            Settings.Default.Save();

            LoginHandler();
            this.Close();
        }

        private void CancelButtonC_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
