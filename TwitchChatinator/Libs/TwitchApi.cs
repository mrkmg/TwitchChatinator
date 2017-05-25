using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace TwitchChatinator.Libs
{
    public sealed class TwitchApi
    {
        private static readonly Lazy<TwitchApi> Lazy =
            new Lazy<TwitchApi>(() => new TwitchApi());

        public static TwitchApi Instance => Lazy.Value;

        public static TwitchChatters ChattersInChannel(string channel)
        {
            var request = new RestRequest("/group/user/{channel}/chatters", Method.GET);
            request.AddUrlSegment("channel", channel);

            var response = Instance.Client.Execute<TwitchChatters>(request);

            return response.Data;
        }

        public RestClient Client;

        private TwitchApi()
        {
            Client = new RestClient("http://tmi.twitch.tv");
        }
    }

    public class TwitchChatters
    {
        public int chatter_count { get; set; }
        public TwitchChattersByType chatters { get; set; }
    }

    public class TwitchChattersByType
    {
        public List<string> moderators { get; set; }
        public List<string> staff { get; set; }
        public List<string> admins { get; set; }
        public List<string> global_mods { get; set; }
        public List<string> viewers { get; set; }
    }
}