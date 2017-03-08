using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace TwitchChatinator.Libs
{
    public sealed class DataStore : IDisposable
    {
        private const string DatetimeFormat = "yyyyMMddHHmmssffff";

        private static readonly Lazy<DataStore> Lazy =
            new Lazy<DataStore>(() => new DataStore());

        private readonly SQLiteConnection _connection;

        private DataStore()
        {
            var saveLocation = Program.AppDataFolder();
            _connection = new SQLiteConnection("Data Source=" + saveLocation + @"\Database.sqlite;Version=3");
            _connection.Open();
            InitDb();
        }

        public static DataStore Instance => Lazy.Value;

        public void Dispose()
        {
            _connection.Close();
        }

        public static DataSet GetDataSet(DataSetSelection selection)
        {
            var command = new StringBuilder("SELECT datetime, channel, user, message FROM messages ");
            command.Append(GetSqlFromSelections(selection));

            var ds = new DataSet();
            using (var da = new SQLiteDataAdapter(command.ToString(), Instance._connection))
            {
                da.Fill(ds);
            }
            return ds;
        }

        public static bool InsertMessage(string channel, string user, string message)
        {
            const string command = "INSERT INTO messages (datetime,channel,user,message) VALUES (@datetime,@channel,@user,@message)";
            bool good;
            using (var sCommand = new SQLiteCommand(command, Instance._connection))
            {
                sCommand.Parameters.AddWithValue("@user", user);
                sCommand.Parameters.AddWithValue("@channel", channel);
                sCommand.Parameters.AddWithValue("@message", message);
                sCommand.Parameters.AddWithValue("@datetime", DateTime.Now.ToString(DatetimeFormat));

                try
                {
                    good = sCommand.ExecuteNonQuery() > 0;
                }
                catch (Exception)
                {
                    good = InsertMessage(channel, user, message);
                }
            }


            return good;
        }

        public static int GetUniqueUsersCount(DataSetSelection selection)
        {
            var count = 0;
            var command = new StringBuilder();
            command.Append("SELECT COUNT(DISTINCT user) as count FROM messages ");
            command.Append(GetSqlFromSelections(selection));

            using (var sCommand = new SQLiteCommand(command.ToString(), Instance._connection))
            {
                try
                {
                    var a = sCommand.ExecuteScalar();
                    count = int.Parse(a.ToString());
                }
                catch (Exception e)
                {
                    Log.LogException(e);
                }
            }

            return count;
        }

        public static List<string> GetUniqueUsersString(DataSetSelection selection)
        {
            var users = new List<string>();
            var command = new StringBuilder();
            command.Append("SELECT DISTINCT user as count FROM messages ");
            command.Append(GetSqlFromSelections(selection));

            using (var sCommand = new SQLiteCommand(command.ToString(), Instance._connection))
            {
                try
                {
                    var reader = sCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        users.Add(reader.GetValue(0).ToString());
                    }
                }
                catch (Exception e)
                {
                    Log.LogException(e);
                }
            }

            return users;
        }

        public static void ExportToCsv(string filename)
        {
            var delimiter = "\"";
            var separator = ",";

            var command = new SQLiteCommand("SELECT * FROM messages ORDER BY datetime DESC", Instance._connection);
            var sqlReader = command.ExecuteReader();
            var fileWriter = new StreamWriter(filename);

            // write header row
            for (var columnCounter = 0; columnCounter < sqlReader.FieldCount; columnCounter++)
            {
                if (columnCounter > 0)
                {
                    fileWriter.Write(separator);
                }
                fileWriter.Write(delimiter + sqlReader.GetName(columnCounter) + delimiter);
            }
            fileWriter.WriteLine(string.Empty);

            // data loop
            while (sqlReader.Read())
            {
                // column loop
                for (var columnCounter = 0; columnCounter < sqlReader.FieldCount; columnCounter++)
                {
                    if (columnCounter > 0)
                    {
                        fileWriter.Write(separator);
                    }
                    fileWriter.Write(delimiter + sqlReader.GetValue(columnCounter).ToString().Replace('"', '\'') +
                                     delimiter);
                } // end of column loop
                fileWriter.WriteLine(string.Empty);
            } // data loop

            fileWriter.Flush();

            command.Dispose();
            sqlReader.Dispose();
            fileWriter.Dispose();
        }

        private static string GetSqlFromSelections(DataSetSelection selection)
        {
            var wheres = new List<string>();
            var orders = new List<string>();
            var resultSqlPart = new StringBuilder();

            if (selection.Start != DateTime.MinValue)
            {
                wheres.Add("datetime >= " + selection.Start.ToString(DatetimeFormat));
            }
            if (selection.End != DateTime.MinValue)
            {
                wheres.Add("datetime <= " + selection.End.ToString(DatetimeFormat));
            }

            orders.Add("datetime DESC"); //May add this in the selection object

            if (wheres.Count > 0)
            {
                resultSqlPart.Append("WHERE ");
                for (var i = 0; i < wheres.Count; i++)
                {
                    resultSqlPart.Append(wheres[i]);
                    if (i != wheres.Count - 1)
                    {
                        resultSqlPart.Append(" AND ");
                    }
                }
                resultSqlPart.Append(" ");
            }

            if (orders.Count > 0)
            {
                resultSqlPart.Append("ORDER BY ");
                for (var i = 0; i < orders.Count; i++)
                {
                    resultSqlPart.Append(orders[i]);
                    if (i != orders.Count - 1)
                    {
                        resultSqlPart.Append(", ");
                    }
                }
                resultSqlPart.Append(" ");
            }

            return resultSqlPart.ToString();
        }

        private void InitDb()
        {
            var command = "CREATE TABLE IF NOT EXISTS messages (" +
                          "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                          "datetime INTEGER NOT NULL," +
                          "channel char(100) NOT NULL," +
                          "user CHAR(100) NOT NULL," +
                          "message TEXT);";
            var sCommand = new SQLiteCommand(command, _connection);
            sCommand.ExecuteNonQuery();
        }
    }

    public class DataSetSelection
    {
        public DateTime End;
        public DateTime Start;
    }
}