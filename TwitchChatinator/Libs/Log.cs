using System;

namespace TwitchChatinator.Libs
{
    public static class Log
    {
        public static void LogException(Exception e)
        {
            var li = new LogItem
            {
                Level = "EXCEPTION",
                Message = e.Message
            };


            Write(li);
        }


        public static void LogInfo(string message)
        {
            var li = new LogItem
            {
                Level = "INFO",
                Message = message
            };


            Write(li);
        }

        private static void Write(LogItem li)
        {
            System.Diagnostics.Debug.WriteLine(li.Time.ToString("U") + "\t" + li.Level + "\t" + li.Message);
        }
    }

    internal class LogItem
    {
        public string Level;
        public string Message;
        public DateTime Time;

        public LogItem()
        {
            Time = DateTime.Now;
            Level = "Unknown Log Call";
            Message = "Why did no one set me?";
        }
    }
}