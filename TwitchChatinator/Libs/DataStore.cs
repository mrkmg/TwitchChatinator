using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace TwitchChatinator
{
    public class DataStoreSQLite
    {
        private SQLiteConnection Connection;
        string LastError;
        string datetimeFormat = "yyyyMMddHHmmssffff";

        public DataStoreSQLite()
        {
            Connection = new SQLiteConnection("Data Source=Database.sqlite;Version=3");
            Connection.Open();
            InitDB();
        }

        public DataSet getDataSet()
        {
            var ds = new DataSet();
            var da = new SQLiteDataAdapter("SELECT * FROM messages ORDER BY datetime DESC LIMIT 5000",Connection);
            da.Fill(ds);
            return ds;
        }

        public bool InsertMessage(string User, string Message){
            string Command = "INSERT INTO messages (datetime,user,message) VALUES (@datetime,@user,@message)";

            SQLiteCommand SCommand = new SQLiteCommand(Command,Connection);
            SCommand.Parameters.AddWithValue("@user", User);
            SCommand.Parameters.AddWithValue("@message", Message);
            SCommand.Parameters.AddWithValue("@datetime", DateTime.Now.ToString(datetimeFormat));

            int results = 0;

            try
            {
                results = SCommand.ExecuteNonQuery();
            } catch(Exception e)
            {
                LastError = e.Message;
            }
            

            return results > 0;

        }

        private void InitDB(){
            string Command = "CREATE TABLE IF NOT EXISTS messages (" +
                                "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                                "datetime INTEGER NOT NULL," +
                                "user CHAR(100) NOT NULL," +
                                "message TEXT);";
            SQLiteCommand SCommand = new SQLiteCommand(Command,Connection);
            SCommand.ExecuteNonQuery();
        }
    }
}
