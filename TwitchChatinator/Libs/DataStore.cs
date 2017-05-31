using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TwitchChatinator.Libs
{
    public sealed class DataStore
    {
        private const string DatetimeFormat = "yyyyMMddHHmmssffff";

        private static readonly Lazy<DataStore> Lazy =
            new Lazy<DataStore>(() => new DataStore());

        private DataStore()
        {
        }

        public static DataStore Instance => Lazy.Value;

        private readonly List<string> _onlineUsers = new List<string>();
        private readonly List<DataStoreMessage> _messages = new List<DataStoreMessage>();

        private List<DataStoreMessage> ApplySelectionCriteria(DataSetSelection selection)
        {
            return _messages.FindAll(delegate(DataStoreMessage message)
            {
                if (selection.Start != DateTime.MinValue && message.Datetime <= selection.Start) return false;
                if (selection.End != DateTime.MinValue && message.Datetime >= selection.End) return false;
                if (!String.IsNullOrEmpty(selection.MessagePartial) &&
                    !message.Message.ToLower().Contains(selection.MessagePartial.ToLower())) return false;

                return true;
            });
        }

        public static List<DataStoreMessage> GetDataSet(DataSetSelection selection)
        {
            return Instance.ApplySelectionCriteria(selection);
        }

        public static bool InsertMessage(string channel, string user, string message)
        {
            Instance._messages.Add(new DataStoreMessage {Datetime = DateTime.Now, Message = message, Username = user});
            return true;
        }

        public static int GetUniqueUsersCount(DataSetSelection selection)
        {
            return Instance.ApplySelectionCriteria(selection).Count;
        }

        public static List<string> GetUniqueUsersString(DataSetSelection selection)
        {
            var users = new List<string>();

            Instance.ApplySelectionCriteria(selection).ForEach((message) =>
            {
                if (!users.Contains(message.Username)) 
                users.Add(message.Username);
            });

            return users;
        }

        public static void ResetUsers()
        {
            Instance._onlineUsers.Clear();
        }

        public static void AddUser(string username)
        {
            if (!Instance._onlineUsers.Contains(username))
            {
                Instance._onlineUsers.Add(username);
            }
        }

        public static void RemoveUser(string username)
        {
            Instance._onlineUsers.Remove(username);
        }

        public static List<string> GetOnlineUsers()
        {
            return new List<string>(Instance._onlineUsers);
        }

        public static void ExportToCsv(string filename)
        {

            using (var fileWriter = new StreamWriter(filename))
            {
                var delimiter = "\"";
                var separator = ",";
                fileWriter.Write(delimiter + "DateTime" + delimiter + separator);
                fileWriter.Write(delimiter + "User" + delimiter + separator);
                fileWriter.Write(delimiter + "Message" + delimiter);
                fileWriter.WriteLine(string.Empty);

                foreach (var message in Instance._messages)
                {
                    fileWriter.Write(delimiter + message.Datetime + delimiter + separator);
                    fileWriter.Write(delimiter + message.Username.Replace('"', '\"') + delimiter + separator);
                    fileWriter.Write(delimiter + message.Message.Replace('"', '\"') + delimiter);
                    fileWriter.WriteLine(string.Empty);
                }

                fileWriter.Flush();
            }
        }
    }

    public class DataSetSelection
    {
        public DateTime End { get; set; }
        public DateTime Start { get; set; }
        public string MessagePartial { get; set; }
    }

    [Serializable]
    public class DataStoreMessage
    {
        public DateTime Datetime { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
    }
}