using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace TwitchChatinator
{
    public class DataStoreMemory : DataStore
    {
        public static List<string[]> Messages;
        string datetimeFormat = "yyyyMMddHHmmssffff";

        public DataStoreMemory()
        {
            if (Messages == null)
                Messages = new List<string[]>();
        }

        public DataSet getDataSet(DataSetSelection selection)
        {
            DataSet DS = new DataSet();

            DataTable table = new DataTable("messages");
            table.Columns.Add("datetime");
            table.Columns.Add("user");
            table.Columns.Add("message");

            foreach (string[] data in Messages)
            {
                if (data.Length == 3)
                {
                    if (selection.Start != DateTime.MinValue && selection.End != DateTime.MinValue)
                    {
                        if(long.Parse(data[0]) >= long.Parse(selection.Start.ToString(datetimeFormat)) &&
                           long.Parse(data[0]) <= long.Parse(selection.End.ToString(datetimeFormat)))
                        {
                            table.Rows.Add(data);
                        }
                    }
                    else
                    {
                        table.Rows.Add(data);
                    }
                }
            }

            DS.Tables.Add(table);

            return DS;
        }

        public bool InsertMessage(string User, string Message)
        {
            Messages.Add(new string[3] { DateTime.Now.ToString(datetimeFormat), User, Message });
            return true;
        }

        public void Dispose()
        {
            //
        }
    }
}
