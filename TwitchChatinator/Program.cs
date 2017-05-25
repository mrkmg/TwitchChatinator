using System;
using System.Collections.Generic;
using System.Data;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TwitchChatinator.Forms;
using TwitchChatinator.Options;

namespace TwitchChatinator
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            CreateDefaultTypes();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WelcomeScreen());
        }

        private static void CreateDefaultTypes()
        {
            if (BarGraphOptions.GetAvaliable().Count == 0)
            {
                BarGraphOptions.CreateNew("Default");
            }

            if (PieGraphOptions.GetAvaliable().Count == 0)
            {
                PieGraphOptions.CreateNew("Default");
            }

            if (GiveawayOptions.GetAvaliable().Count == 0)
            {
                GiveawayOptions.CreateNew("Default");
            }
        }

        // Thank you sylvanaar
        //https://stackoverflow.com/questions/1488918/how-to-synchronise-the-publish-version-to-the-assembly-version-in-a-net-clickon
        public static string GetVersion()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                return ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }

            return "Debug";
        }

        //Thank you [grenade](http://stackoverflow.com/users/68115/grenade)
        //http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp 2014-08-13
        public static void Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        //Thank you [SuperLucky](http://stackoverflow.com/users/766963/superlucky)
        //http://stackoverflow.com/questions/888181/convert-datatable-to-csv-stream 2014-08-13
        public static bool DataTableToCsv(DataTable dtSource, StreamWriter writer, bool includeHeader)
        {
            if (dtSource == null || writer == null) return false;

            if (includeHeader)
            {
                var columnNames =
                    dtSource.Columns.Cast<DataColumn>()
                        .Select(column => "\"" + column.ColumnName.Replace("\"", "\"\"") + "\"")
                        .ToArray();
                writer.WriteLine(string.Join(",", columnNames));
                writer.Flush();
            }

            foreach (DataRow row in dtSource.Rows)
            {
                var fields =
                    row.ItemArray.Select(field => "\"" + field.ToString().Replace("\"", "\"\"") + "\"").ToArray();
                writer.WriteLine(string.Join(",", fields));
                writer.Flush();
            }

            return true;
        }

        public static string AppDataFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Chatinator";
        }
    }

    internal class SelectListObject
    {
        public SelectListObject()
        {
        }

        public SelectListObject(string n, string t)
        {
            Name = n;
            Type = t;
        }

        public string Name { get; set; }
        public string Type { get; set; }
    }
}