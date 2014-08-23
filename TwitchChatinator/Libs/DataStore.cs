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
    public sealed class DataStore
    {
        private static readonly Lazy<DataStore> lazy =
            new Lazy<DataStore>(() => new DataStore());

        public static DataStore Instance { get { return lazy.Value; } }

        private SQLiteConnection Connection;
        string LastError;
        private const string datetimeFormat = "yyyyMMddHHmmssffff";

        private DataStore()
        {
            string saveLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Connection = new SQLiteConnection("Data Source=" + saveLocation + @"\Database.sqlite;Version=3");
            Connection.Open();
            InitDB();
        }

        public static DataSet GetDataSet(DataSetSelection Selection)
        {
            StringBuilder Command = new StringBuilder("SELECT datetime, channel, user, message FROM messages ");
            Command.Append(getSqlFromSelections(Selection));

            var ds = new DataSet();
            using (var da = new SQLiteDataAdapter(Command.ToString(), Instance.Connection))
            {
                da.Fill(ds);
            }
            return ds;
        }

        static public bool InsertMessage(string Channel, string User, string Message)
        {
            string Command = "INSERT INTO messages (datetime,channel,user,message) VALUES (@datetime,@channel,@user,@message)";
            bool good = false;
            using (SQLiteCommand SCommand = new SQLiteCommand(Command, Instance.Connection))
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
                    Instance.LastError = e.Message;
                    good = InsertMessage(Channel, User, Message);
                }
            }


            return good;

        }

        static public int GetUniqueUsersCount(DataSetSelection Selection)
        {
            int count = 0;
            StringBuilder Command = new StringBuilder();
            Command.Append("SELECT COUNT(DISTINCT user) as count FROM messages ");
            Command.Append(getSqlFromSelections(Selection));

            using (SQLiteCommand SCommand = new SQLiteCommand(Command.ToString(), Instance.Connection))
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

        static public List<string> GetUniqueUsersString(DataSetSelection Selection)
        {
            List<string> users = new List<string>();
            StringBuilder Command = new StringBuilder();
            Command.Append("SELECT DISTINCT user as count FROM messages ");
            Command.Append(getSqlFromSelections(Selection));

            using (SQLiteCommand SCommand = new SQLiteCommand(Command.ToString(), Instance.Connection))
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

        static public void ExportToCsv(string Filename)
        {
            string Delimiter = "\"";
            string Separator = ",";

            SQLiteCommand Command = new SQLiteCommand("SELECT * FROM messages ORDER BY datetime DESC",Instance.Connection);
            SQLiteDataReader SQLReader = Command.ExecuteReader();
            StreamWriter FileWriter = new StreamWriter(Filename);

            // write header row
            for (int columnCounter = 0; columnCounter < SQLReader.FieldCount; columnCounter++)
            {
                if (columnCounter > 0)
                {
                    FileWriter.Write(Separator);
                }
                FileWriter.Write(Delimiter + SQLReader.GetName(columnCounter) + Delimiter);
            }
            FileWriter.WriteLine(string.Empty);

            // data loop
            while (SQLReader.Read())
            {
                // column loop
                for (int columnCounter = 0; columnCounter < SQLReader.FieldCount; columnCounter++)
                {
                    if (columnCounter > 0)
                    {
                        FileWriter.Write(Separator);
                    }
                    FileWriter.Write(Delimiter + SQLReader.GetValue(columnCounter).ToString().Replace('"', '\'') + Delimiter);
                }   // end of column loop
                FileWriter.WriteLine(string.Empty);
            }   // data loop

            FileWriter.Flush();

            Command.Dispose();
            SQLReader.Dispose();
            FileWriter.Dispose();
        }

        static private string getSqlFromSelections(DataSetSelection Selection)
        {
            List<string> Wheres = new List<string>();
            List<string> Orders = new List<string>();
            StringBuilder ResultSqlPart = new StringBuilder();

            if (Selection.Start != DateTime.MinValue)
            {
                Wheres.Add("datetime >= " + Selection.Start.ToString(datetimeFormat));
            }
            if (Selection.End != DateTime.MinValue)
            {
                Wheres.Add("datetime <= " + Selection.End.ToString(datetimeFormat));
            }

            Orders.Add("datetime DESC"); //May add this in the selection object

            if (Wheres.Count > 0)
            {
                ResultSqlPart.Append("WHERE ");
                for (int i = 0; i < Wheres.Count; i++)
                {
                    ResultSqlPart.Append(Wheres[i]);
                    if (i != Wheres.Count - 1)
                    {
                        ResultSqlPart.Append(" AND ");
                    }
                }
                ResultSqlPart.Append(" ");
            }

            if (Orders.Count > 0)
            {
                ResultSqlPart.Append("ORDER BY ");
                for (int i = 0; i < Orders.Count; i++)
                {
                    ResultSqlPart.Append(Orders[i]);
                    if (i != Orders.Count - 1)
                    {
                        ResultSqlPart.Append(", ");
                    }
                }
                ResultSqlPart.Append(" ");
            }

            return ResultSqlPart.ToString();
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

    public class DataSetSelection
    {
        public DateTime Start;
        public DateTime End;
    }
}