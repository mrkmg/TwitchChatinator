using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;

namespace TwitchChatinator.Libs
{
    public class AuthenticationWebserver
    {
        private readonly HttpListener _listener = new HttpListener();

        public delegate void ReceivedAuthCode(string code);

        public event ReceivedAuthCode OnReceivedAuthCode;

        private Thread _listenThread;

        private const string TwitchAuthUrl = "https://api.twitch.tv/kraken/oauth2/authorize";
        private const string TwitchClientId = "geybikx6e1oowq816gtln56zhsfkcw";
        private const string TwitchRedirectUrl = "http://localhost:8080/auth/";
        private const string TwitchScope = "chat_login";

        public AuthenticationWebserver()
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            _listener.Prefixes.Add(@"http://localhost:8080/");
            _listener.Prefixes.Add(@"http://localhost:8080/auth/");
            _listener.Start();
        }

        private string BuildUrl()
        {
            return TwitchAuthUrl +
                   "?response_type=code" +
                   "&client_id=" + Uri.EscapeDataString(TwitchClientId) +
                   "&redirect_uri=" + Uri.EscapeDataString(TwitchRedirectUrl) +
                   "&scope=" + Uri.EscapeDataString(TwitchScope) +
                   "&force_verify";
        }

        private string RequestHandler(HttpListenerRequest request)
        {
            switch (request.Url.AbsolutePath)
            {
                case "/":
                    return "Redirecting<script>window.location = '" + BuildUrl() + "';</script>";
                case "/auth/":
                    OnOnReceivedAuthCode(request.QueryString["code"]);
                    return "Checking Auth";
                default:
                    return "Bad URL";
            }
        }

        public void Run()
        {
            _listenThread = new Thread(() =>
            {
                Console.WriteLine("Webserver running...");
                while (_listener.IsListening)
                {
                    Console.WriteLine("While IsListening");
                    try
                    {
                        var ctx = _listener.GetContext();
                        try
                        {
                            string rstr = RequestHandler(ctx.Request);
                            byte[] buf = Encoding.UTF8.GetBytes(rstr);
                            ctx.Response.ContentLength64 = buf.Length;
                            ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                        }
                        catch
                        {
                        }
                        finally
                        {
                            // always close the stream
                            ctx.Response.OutputStream.Close();
                        }
                    }
                    catch
                    {
                    }

                }
            });
            _listenThread.Start();
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }

        protected virtual void OnOnReceivedAuthCode(string code)
        {
            OnReceivedAuthCode?.Invoke(code);
        }
    }
}
