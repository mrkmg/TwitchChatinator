using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Security.Cryptography;
using System.Data;
using System.Text;
using System.IO;

namespace TwitchChatinator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WelcomeScreen());
        }

        static public DataStore getSelectedDataStore()
        {
            return (DataStore) Activator.CreateInstance(Type.GetType("TwitchChatinator.DataStore" + Settings.Default.StorageEngine));
        }

        //Thank you [grenade](http://stackoverflow.com/users/68115/grenade)
        //http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp 2014-08-13
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        //Thank you [SuperLucky](http://stackoverflow.com/users/766963/superlucky)
        //http://stackoverflow.com/questions/888181/convert-datatable-to-csv-stream 2014-08-13
        public static bool DataTableToCSV(DataTable dtSource, StreamWriter writer, bool includeHeader)
        {
            if (dtSource == null || writer == null) return false;

            if (includeHeader)
            {
                string[] columnNames = dtSource.Columns.Cast<DataColumn>().Select(column => "\"" + column.ColumnName.Replace("\"", "\"\"") + "\"").ToArray<string>();
                writer.WriteLine(String.Join(",", columnNames));
                writer.Flush();
            }

            foreach (DataRow row in dtSource.Rows)
            {
                string[] fields = row.ItemArray.Select(field => "\"" + field.ToString().Replace("\"", "\"\"") + "\"").ToArray<string>();
                writer.WriteLine(String.Join(",", fields));
                writer.Flush();
            }

            return true;
        }
    }
}
