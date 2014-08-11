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
    public class DataStoreSQLite : IDisposable
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

        public DataSet getDataSet(DataSetSelection selection)
        {
            StringBuilder sql = new StringBuilder("SELECT datetime, user, message FROM messages");

            if (selection.Start != DateTime.MinValue && selection.End != DateTime.MinValue)
            {
                sql.Append(" WHERE datetime BETWEEN " + selection.Start.ToString(datetimeFormat) + " AND " + selection.End.ToString(datetimeFormat));
            }
            sql.Append(" ORDER BY datetime DESC LIMIT 5000");
            Log.LogInfo("SQL\t" + sql.ToString());
            var ds = new DataSet();
            var da = new SQLiteDataAdapter(sql.ToString(),Connection);
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

        public void Dispose()
        {
            Connection.Close();
        }
    }

    public class DataSetSelection
    {
        public DateTime Start;
        public DateTime End;
    }
}
