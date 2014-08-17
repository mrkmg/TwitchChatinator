//Much Code Boworroed/Stolen from https://github.com/Phoinx/PhoinxBot/blob/master/PhoinxBot/IRC.cs
//Thank you to Phoinx for making this avaliable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Data.SQLite;
using System.Text.RegularExpressions;


namespace TwitchChatinator
{
    public class TwitchIRC : IDisposable
    {
        private TcpClient Client;
        private NetworkStream NwStream;
        private StreamReader Reader;
        private StreamWriter Writer;
        private Thread Listen;

        public delegate void ReceiveMessage(TwitchMessageObject Message);
        public event ReceiveMessage OnReceiveMessage;

        public delegate void Connected();
        public event Connected OnConnected;

        public delegate void Disconnected();
        public event Disconnected OnDisconnected;

        private bool isConnected = false;

        private bool _shouldStop = false;

        private string Channel;

        public TwitchIRC()
        {
            CreateListenThread();
        }

        private void CreateListenThread()
        {
            Listen = new Thread(new ThreadStart(ListenThread));
        }

        private void Login()
        {
            string Username;
            string Password;

            Username = Settings.Default.TwitchUsername;
            Password = Settings.Default.TwitchPassword;

            Write("USER " + Username + "tmi " + Password);
            Write("PASS " + Password);
            Write("NICK " + Username);
        }

        private void JoinChannel()
        {
            Channel = Settings.Default.TwithChannel;

            Write("JOIN #" + Channel + "\n");
        }

        private void setConnected()
        {
            isConnected = true;
            if (OnConnected != null)
            {
                OnConnected();
            }
        }

        public void Start()
        {
            _shouldStop = false;
            switch (Listen.ThreadState)
            {
                case ThreadState.Unstarted:
                    Listen.Start();
                    break;
                case ThreadState.Running:
                    //SOMETHING
                    break;
                case ThreadState.AbortRequested:
                    OnDisconnected();
                    break;
                default:
                    CreateListenThread();
                    Listen.Start();
                    break;
            }
        }

        public void Stop()
        {
            _shouldStop = true;
            Write("QUIT");
        }

        public void DoReceiveMessage(TwitchMessageObject Message)
        {
            DataStore.InsertMessage(Message.channel, Message.username, Message.message);

            OnReceiveMessage(Message);
        }

        public void ListenThread()
        {
            isConnected = false;
            try
            {
                Log.LogInfo("Pre - Client");
                using (Client = new TcpClient("irc.twitch.tv", 6667))
                {
                    NwStream = Client.GetStream();
                    Reader = new StreamReader(NwStream, Encoding.GetEncoding("iso8859-1"));
                    Writer = new StreamWriter(NwStream, Encoding.GetEncoding("iso8859-1"));

                    Login();
                    JoinChannel();

                    string Data = "";
                    string msgStringIdent = "PRIVMSG #" + Settings.Default.TwithChannel;
                    string[] ex;
                    while ((Data = Reader.ReadLine()) != null && !_shouldStop)
                    {
                        Log.LogInfo("TwitchMessageReceived\t" + Data);
                        //TODO: Look for a better way to do this.
                        //Not sure if this is reliable
                        //IDEA: Check if properly joined channel
                        if (!isConnected && Data.Contains(":Welcome, GLHF!"))
                        {
                            setConnected();
                        }

                        //Check for Pings, and Pong them back
                        ex = Data.Split(new char[] { ' ' }, 5);
                        if (ex[0] == "PING")
                        {
                            Write("PONG " + ex[1]);
                        }

                        //Check for and Handle Message Object
                        if (OnReceiveMessage != null)
                        {
                            if (Data.Contains(msgStringIdent))
                            {
                                int usrStart = Data.IndexOf("!");
                                string Username = Data.Substring(1, usrStart - 1);
                                int msgStart = Data.IndexOf(msgStringIdent);
                                string Message = Data.Substring(msgStart + msgStringIdent.Length + 2);
                                DoReceiveMessage(new TwitchMessageObject(Channel, Username, Message));
                            }
                        }
                    }
                    OnDisconnected();

                    Writer.Dispose();
                    Reader.Dispose();
                    NwStream.Dispose();
                }
            }
            catch (Exception e)
            {
                Log.LogException(e);
                if (Listen.ThreadState == ThreadState.Running)
                {
                    Listen.Abort();
                }
                if (OnDisconnected != null)
                {
                    OnDisconnected();
                    isConnected = false;
                }
            }
        }

        private void Write(string s)
        {
            try
            {
                if (Writer != null && Listen.ThreadState == ThreadState.Running)
                {
                    Log.LogInfo("TwitchMessageSent\t" + s);
                    Writer.WriteLine(s);
                    Writer.Flush();
                }
            }
            catch (Exception e)
            {
                //We do not throw here because this bot is a listen only. May look into this later
                Log.LogException(e);
            }
        }

        public void Dispose()
        {
            try
            {
                Writer.Dispose();
                Reader.Dispose();
                NwStream.Dispose();
            }
            catch(Exception e)
            {
                Log.LogException(e);
            }
        }
    }

    public class TwitchMessageObject
    {
        public string username;
        public string message;
        public string channel;

        public TwitchMessageObject(string c, string u, string m)
        {
            channel = c;
            username = u;
            message = m;
        }
    }
}
