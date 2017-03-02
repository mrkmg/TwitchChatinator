//Much Code Boworroed/Stolen from https://github.com/Phoinx/PhoinxBot/blob/master/PhoinxBot/IRC.cs
//Thank you to Phoinx for making this avaliable

using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TwitchChatinator.Libs
{
    public class TwitchIrc : IDisposable
    {
        public delegate void Connected(string channel);

        public delegate void Disconnected();

        public delegate void ReceiveMessage(TwitchMessageObject message);

        private bool _shouldStop;

        private string _channel;
        private TcpClient _client;

        private bool _isConnected;
        private Thread _listen;
        private NetworkStream _nwStream;
        private StreamReader _reader;
        private StreamWriter _writer;

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

        private void CreateListenThread()
        {
            _listen = new Thread(ListenThread);
        }

        private void Login()
        {
            string username;
            string password;

            username = Settings.Default.TwitchUsername;
            password = Settings.Default.TwitchPassword;

            Write("USER " + username + "tmi " + password);
            Write("PASS " + password);
            Write("NICK " + username);
        }

        private void JoinChannel()
        {
            _channel = Settings.Default.TwithChannel;

            Write("JOIN #" + _channel + "\n");
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
                    //SOMETHING
                    break;
                case ThreadState.AbortRequested:
                    OnDisconnected();
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
            DataStore.InsertMessage(message.Channel, message.Username, message.Message);

            OnReceiveMessage(message);
        }

        public void ListenThread()
        {
            _isConnected = false;
            try
            {
                Log.LogInfo("Pre - Client");
                using (_client = new TcpClient("irc.twitch.tv", 6667))
                {
                    _nwStream = _client.GetStream();
                    _reader = new StreamReader(_nwStream, Encoding.GetEncoding("iso8859-1"));
                    _writer = new StreamWriter(_nwStream, Encoding.GetEncoding("iso8859-1"));

                    Login();
                    JoinChannel();

                    var data = "";
                    var msgStringIdent = "PRIVMSG #" + Settings.Default.TwithChannel;
                    string[] ex;
                    while ((data = _reader.ReadLine()) != null && !_shouldStop)
                    {
                        Log.LogInfo("TwitchMessageReceived\t" + data);
                        //TODO: Look for a better way to do this.
                        //Not sure if this is reliable
                        //IDEA: Check if properly joined channel
                        if (!_isConnected && data.Contains(":Welcome, GLHF!"))
                        {
                            SetConnected();
                        }

                        //Check for Pings, and Pong them back
                        ex = data.Split(new[] {' '}, 5);
                        if (ex[0] == "PING")
                        {
                            Write("PONG " + ex[1]);
                        }

                        //Check for and Handle Message Object
                        if (OnReceiveMessage != null)
                        {
                            if (data.Contains(msgStringIdent))
                            {
                                var usrStart = data.IndexOf("!");
                                var username = data.Substring(1, usrStart - 1);
                                var msgStart = data.IndexOf(msgStringIdent);
                                var message = data.Substring(msgStart + msgStringIdent.Length + 2);
                                DoReceiveMessage(new TwitchMessageObject(_channel, username, message));
                            }
                        }
                    }
                    OnDisconnected();

                    _writer.Dispose();
                    _reader.Dispose();
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
                if (OnDisconnected != null)
                {
                    OnDisconnected();
                    _isConnected = false;
                }
            }
        }

        private void Write(string s)
        {
            try
            {
                if (_writer != null && _listen.ThreadState == ThreadState.Running)
                {
                    Log.LogInfo("TwitchMessageSent\t" + s);
                    _writer.WriteLine(s);
                    _writer.Flush();
                }
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