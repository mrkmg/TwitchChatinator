using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace TwitchChatinator
{
    public class DataStoreMemory
    {
        public static List<string[]> Messages;
        string datetimeFormat = "yyyyMMddHHmmssffff";

        public DataStoreMemory()
        {
            if (Messages == null)
                Messages = new List<string[]>();
        }

        public DataSet GetDataSet(DataSetSelection selection)
        {
            DataSet DS = new DataSet();

            DataTable table = new DataTable("messages");
            table.Columns.Add("datetime");
            table.Columns.Add("channel");
            table.Columns.Add("user");
            table.Columns.Add("message");

            bool addRow = false;
            foreach (string[] data in Messages)
            {
                addRow = true;
                if (data.Length == 3)
                {
                    if((selection.Start != DateTime.MinValue && long.Parse(data[0]) < long.Parse(selection.Start.ToString(datetimeFormat)))){
                        addRow = false;
                    }
                    if (addRow && (selection.End != DateTime.MinValue && long.Parse(data[0]) > long.Parse(selection.End.ToString(datetimeFormat))))
                    {
                        addRow = false;
                    }
                } else {
                    addRow = false;
                }

                if(addRow){
                   table.Rows.Add(data);
                }
            }

            DS.Tables.Add(table);

            return DS;
        }

        public bool InsertMessage(string Channel, string User, string Message)
        {
            Messages.Add(new string[4] { DateTime.Now.ToString(datetimeFormat), Channel, User, Message });
            return true;
        }

        public void Dispose()
        {
            //
        }
    }
}
