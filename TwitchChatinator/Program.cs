using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Console.WriteLine("TwitchChatinator.DataStore" + Settings.Default.StorageEngine);
            return (DataStore) Activator.CreateInstance(Type.GetType("TwitchChatinator.DataStore" + Settings.Default.StorageEngine));
        }
    }
}
