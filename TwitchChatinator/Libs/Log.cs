using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchChatinator
{
    public static class Log
    {
        public static void LogException(Exception e)
        {
            LogItem LI = new LogItem();

            LI.Level = "EXCEPTION";
            LI.Message = e.Message;

            Write(LI);   
        }


        public static void LogInfo(string Message)
        {
            LogItem LI = new LogItem();

            LI.Level = "INFO";
            LI.Message = Message;

            Write(LI);  
        }

        private static void Write(LogItem LI){
            Console.WriteLine(LI.Time.ToString("U") + "\t" + LI.Level + "\t" + LI.Message);
        }
    }

    class LogItem
    {
        public DateTime Time;
        public string Level;
        public string Message;

        public LogItem()
        {
            Time = DateTime.Now;
            Level = "Unknown Log Call";
            Message = "Why did no one set me?";
        }
    }
}
