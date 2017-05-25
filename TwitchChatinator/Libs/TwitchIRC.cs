//Much Code Boworroed/Stolen from https://github.com/Phoinx/PhoinxBot/blob/master/PhoinxBot/IRC.cs
// ReSharper disable once CommentTypo
//Thank you to Phoinx for making this avaliable

using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TwitchChatinator.Libs
{
    public class TwitchIrc : IDisposable
    {
        private const string TwitchServerUrl = "irc.chat.twitch.tv";
        private const int TwitchServerPort = 443;

        public delegate void Connected(string channel);
        public delegate void Disconnected();
        public delegate void ReceiveMessage(TwitchMessageObject message);
        public delegate void UserJoin(string user);
        public delegate void UserLeave(string user);

        private bool _shouldStop;

        private string _channel;
        private TcpClient _client;

        private bool _isConnected;
        private Thread _listen;
        private NetworkStream _nwStream;
        private StreamReader _reader;
        private StreamWriter _writer;
        private SslStream _sslStream;

        public TwitchIrc()
        {
            CreateListenThread();
        }

        public void Dispose()
        {
            try
            {
                _writer.Dispose();
                _reader.Dispose();
                _nwStream.Dispose();
            }
            catch (Exception e)
            {
                Log.LogException(e);
            }
        }

        public event ReceiveMessage OnReceiveMessage;
        public event Connected OnConnected;
        public event Disconnected OnDisconnected;
        public event UserJoin OnUserJoin;
        public event UserLeave OnUserLeave;

        private void CreateListenThread()
        {
            _listen = new Thread(ListenThread);
        }

        private void Login()
        {
            var username = Settings.Default.TwitchUsername.ToLower();
            var password = Settings.Default.TwitchPassword;

//            Write("USER " + username + "tmi " + password);
            Write("PASS oauth:" + password);
            Write("NICK " + username);
        }

        private void SetMembershipCaps()
        {
            Write("CAP REQ :twitch.tv/membership");
        }

        private void JoinChannel()
        {
            Write("JOIN #" + _channel);
        }

        private void SetConnected()
        {
            _isConnected = true;
            OnConnected?.Invoke(_channel);
        }

        public void Start()
        {
            _shouldStop = false;
            switch (_listen.ThreadState)
            {
                case ThreadState.Unstarted:
                    _listen.Start();
                    break;
                case ThreadState.Running:
                    //TODO SOMETHING
                    break;
                case ThreadState.AbortRequested:
                    OnDisconnected?.Invoke();
                    break;
                default:
                    CreateListenThread();
                    _listen.Start();
                    break;
            }
        }

        public void Stop()
        {
            _shouldStop = true;
            Write("QUIT");
        }

        public void DoReceiveMessage(TwitchMessageObject message)
        {
            OnReceiveMessage?.Invoke(message);
        }

        public void ListenThread()
        {
            _isConnected = false;
            try
            {
                Log.LogInfo("Pre - Client");
                using (_client = new TcpClient(TwitchServerUrl, TwitchServerPort))
                { 
                    string data;
                    _nwStream = _client.GetStream();
                    _sslStream = new SslStream(_nwStream);
                    
                    _sslStream.AuthenticateAsClient(TwitchServerUrl);

                    _reader = new StreamReader(_sslStream, Encoding.GetEncoding("iso8859-1"));
                    _writer = new StreamWriter(_sslStream, Encoding.GetEncoding("iso8859-1"));
                    _channel = Settings.Default.TwitchChannel;

                    Login();

                    while (!_shouldStop && (data = _reader.ReadLine()) != null)
                    {
                        HandleMessage(data);
                    }
                    OnDisconnected?.Invoke();

                    _writer.Dispose();
                    _reader.Dispose();
                    _sslStream.Dispose();
                    _nwStream.Dispose();
                }
            }
            catch (Exception e)
            {
                Log.LogException(e);
                if (_listen.ThreadState == ThreadState.Running)
                {
                    _listen.Abort();
                }

                OnDisconnected?.Invoke();
                _isConnected = false;
            }
        }

        private void HandleMessage(string data)
        {
            var username = Settings.Default.TwitchUsername.ToLower();
            var channel = _channel.ToLower();
            Log.LogInfo("TwitchMessageReceived\t" + data);

            //Check for Pings, and Pong them back
            var ex = data.Split(new[] { ' ' }, 5);
            if (ex[0] == "PING")
            {
                Write("PONG " + ex[1]);
                return;
            }

            //Check for and Handle Message Object
            var msgStringIdentifier = "PRIVMSG #" + Settings.Default.TwitchChannel;
            if (OnReceiveMessage != null && data.Contains(msgStringIdentifier))
            {
                var usrStart = data.IndexOf("!", StringComparison.Ordinal);
                var user = data.Substring(1, usrStart - 1);
                var msgStart = data.IndexOf(msgStringIdentifier, StringComparison.Ordinal);
                var message = data.Substring(msgStart + msgStringIdentifier.Length + 2);
                DoReceiveMessage(new TwitchMessageObject(_channel, user, message));
                return;
            }

            if (OnUserJoin != null && data.Contains(":"+username+".tmi.twitch.tv 353 "+username+" = #" + channel))
            {
                var namesListStringStart = data.IndexOf(':', 3);
                var namesList = data.Substring(namesListStringStart + 1).Split(' ');
                
                foreach (var name in namesList)
                {
                    OnUserJoin(name);
                }
                return;
            }


            // Logged in
            if (data == ":tmi.twitch.tv 001 " + username + " :Welcome, GLHF!")
            {
                SetMembershipCaps();
                return;
            }

            // Got Caps
            if (data == ":tmi.twitch.tv CAP * ACK :twitch.tv/membership")
            {
                JoinChannel();
                // cool, got the caps
                return;
            }

            // Joined Channel
            if (data == ":" + username + "!" + username + "@" + username + ".tmi.twitch.tv JOIN #" + channel)
            {
                SetConnected();
                return;
            }

            if (OnUserLeave != null && data.Contains(".tmi.twitch.tv PART #"))
            {
                var userNameEnd = data.IndexOf('!');
                OnUserLeave(data.Substring(1, userNameEnd -1));
                return;
            }

            if (OnUserJoin != null && data.Contains(".tmi.twitch.tv JOIN #"))
            {
                var userNameEnd = data.IndexOf('!');
                OnUserJoin(data.Substring(1, userNameEnd - 1));
                return;
            }

            

            // Check for everthing else...
            // TODO
        }

        private void Write(string s)
        {
            try
            {
                if (_writer == null || _listen.ThreadState != ThreadState.Running) return;

                Log.LogInfo("TwitchMessageSent\t" + s);
                _writer.WriteLine(s);
                _writer.Flush();
            }
            catch (Exception e)
            {
                //We do not throw here because this bot is a listen only. May look into this later
                Log.LogException(e);
            }
        }
    }

    public class TwitchMessageObject
    {
        public string Channel;
        public string Message;
        public string Username;

        public TwitchMessageObject(string c, string u, string m)
        {
            Channel = c;
            Username = u;
            Message = m;
        }
    }
}