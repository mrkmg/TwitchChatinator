using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Security.Cryptography;

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

        static public int PtToPxV(float font, Graphics G)
        {
            return (int)((font * G.DpiY) / 72);
        }

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
    }
}
