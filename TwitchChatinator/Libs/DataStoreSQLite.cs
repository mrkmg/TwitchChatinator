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
    public class DataStoreSQLite : DataStore
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

        public DataSet GetDataSet(DataSetSelection selection)
        {
            StringBuilder sql = new StringBuilder("SELECT datetime, channel, user, message FROM messages");



            if (selection.Start != DateTime.MinValue && selection.End != DateTime.MinValue)
            {
                sql.Append(" WHERE datetime BETWEEN " + selection.Start.ToString(datetimeFormat) + " AND " + selection.End.ToString(datetimeFormat));
            }
            else if (selection.Start != DateTime.MinValue)
            {
                sql.Append(" WHERE datetime >= " + selection.Start.ToString(datetimeFormat));
            }
            else if (selection.End != DateTime.MinValue)
            {
                sql.Append(" WHERE datetime <= " + selection.End.ToString(datetimeFormat));
            }
            sql.Append(" ORDER BY datetime DESC");
            var ds = new DataSet();
            using (var da = new SQLiteDataAdapter(sql.ToString(), Connection))
            {
                da.Fill(ds);
            }
            return ds;
        }

        public bool InsertMessage(string Channel, string User, string Message)
        {
            string Command = "INSERT INTO messages (datetime,channel,user,message) VALUES (@datetime,@channel,@user,@message)";
            bool good = false;
            using (SQLiteCommand SCommand = new SQLiteCommand(Command, Connection))
            {
                SCommand.Parameters.AddWithValue("@user", User);
                SCommand.Parameters.AddWithValue("@channel", Channel);
                SCommand.Parameters.AddWithValue("@message", Message);
                SCommand.Parameters.AddWithValue("@datetime", DateTime.Now.ToString(datetimeFormat));


                try
                {
                    good = SCommand.ExecuteNonQuery() > 0;
                }
                catch (Exception e)
                {
                    LastError = e.Message;
                    good = InsertMessage(Channel, User, Message);
                }
            }


            return good;

        }

        public int GetUniqueUsersCount(DataSetSelection Selection)
        {
            int count = 0;
            StringBuilder Command = new StringBuilder();
            Command.Append("SELECT COUNT(DISTINCT user) as count FROM messages");
            if (Selection.Start != DateTime.MinValue && Selection.End != DateTime.MinValue)
            {
                Command.Append(" WHERE datetime BETWEEN " + Selection.Start.ToString(datetimeFormat) + " AND " + Selection.End.ToString(datetimeFormat));
            }
            else if (Selection.Start != DateTime.MinValue)
            {
                Command.Append(" WHERE datetime >= " + Selection.Start.ToString(datetimeFormat));
            }
            else if (Selection.End != DateTime.MinValue)
            {
                Command.Append(" WHERE datetime <= " + Selection.End.ToString(datetimeFormat));
            }

            using (SQLiteCommand SCommand = new SQLiteCommand(Command.ToString(),Connection))
            {
                try
                {
                    object a = SCommand.ExecuteScalar();
                    count = int.Parse(a.ToString());
                }
                catch (Exception e)
                {
                    Log.LogException(e);
                }
            }

            return (int)count;
        }

        public List<string> GetUniqueUsersString(DataSetSelection Selection)
        {
            List<string> users = new List<string>();
            StringBuilder Command = new StringBuilder();
            Command.Append("SELECT DISTINCT user as count FROM messages");
            if (Selection.Start != DateTime.MinValue && Selection.End != DateTime.MinValue)
            {
                Command.Append(" WHERE datetime BETWEEN " + Selection.Start.ToString(datetimeFormat) + " AND " + Selection.End.ToString(datetimeFormat));
            }
            else if (Selection.Start != DateTime.MinValue)
            {
                Command.Append(" WHERE datetime >= " + Selection.Start.ToString(datetimeFormat));
            }
            else if (Selection.End != DateTime.MinValue)
            {
                Command.Append(" WHERE datetime <= " + Selection.End.ToString(datetimeFormat));
            }

            using (SQLiteCommand SCommand = new SQLiteCommand(Command.ToString(),Connection))
            {
                try
                {
                    SQLiteDataReader Reader = SCommand.ExecuteReader();
                    while (Reader.Read())
                    {
                        users.Add(Reader.GetValue(0).ToString());
                    }
                }
                catch (Exception e)
                {
                    Log.LogException(e);
                }
            }

            return users;
        }

        private void InitDB()
        {
            string Command = "CREATE TABLE IF NOT EXISTS messages (" +
                                "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                                "datetime INTEGER NOT NULL," +
                                "channel char(100) NOT NULL," +
                                "user CHAR(100) NOT NULL," +
                                "message TEXT);";
            SQLiteCommand SCommand = new SQLiteCommand(Command, Connection);
            SCommand.ExecuteNonQuery();
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}
