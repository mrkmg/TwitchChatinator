using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml.Serialization;
using System.IO;

namespace TwitchChatinator
{
    class BarGraphOptions
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public int MarginTop { get; set; }
        public int MarginBottom { get; set; }
        public int MarginLeft { get; set; }
        public int MarginRight { get; set; }

        public int BarSpacing { get; set; }

        public Color ChromaKey { get; set; }
        public Color Option1Color { get; set; }
        public Color Option2Color { get; set; }
        public Color Option3Color { get; set; }
        public Color Option4Color { get; set; }

        public Color OptionFontColor { get; set; }
        public Font OptionFont { get; set; }

        public Color CountFontColor { get; set; }
        public Font CountFont { get; set; }

        public Color TotalFontColor { get; set; }
        public Font TotalFont { get; set; }

        public bool AllowMulti { get; set; }

        public string TotalPosition { get; set; }

        //TODO - Add in exception login
        static public BarGraphOptions Load(string name)
        {
            var reader = new XmlSerializer(typeof(BarGraphOptions));
            var stream = new StreamReader(getPathFromName(name));
            var obj = new BarGraphOptions();
            obj = (BarGraphOptions)reader.Deserialize(stream);

            stream.Close();
            return obj;
        }

        //TODO - Add in exception login
        public void Save(string name)
        {
            var writer = new XmlSerializer(typeof(BarGraphOptions));
            var stream = new StreamWriter(getPathFromName(name),false);

            writer.Serialize(stream, this);

            stream.Close();
        }

        static public void CreateNew(string name)
        {
            var obj = new BarGraphOptions();

            obj.Width = 300;
            obj.Height = 300;

            obj.MarginTop = 5;
            obj.MarginBottom = 5;
            obj.MarginLeft = 5;
            obj.MarginRight = 5;

            obj.BarSpacing = 3;

            obj.ChromaKey = Color.Black;
            obj.Option1Color = Color.Maroon;
            obj.Option2Color = Color.RoyalBlue;
            obj.Option3Color = Color.Orange;
            obj.Option4Color = Color.ForestGreen;

            obj.OptionFontColor = Color.White;
            obj.OptionFont = new Font("Segoe UI", 15.75f, FontStyle.Bold);

            obj.CountFontColor = Color.White;
            obj.CountFont =  new Font("Segoe UI", 10.25f, FontStyle.Italic);

            obj.TotalFontColor = Color.White;
            obj.TotalFont = new Font("Segoe UI", 15.75f, FontStyle.Bold);

            obj.AllowMulti = false;

            obj.TotalPosition = "BottomMiddle";

            obj.Save(name);
        }

        static public void Remove(string name)
        {
            File.Delete(getPathFromName(name));
        }

        static public void Rename(string fromName, string toName)
        {
            File.Move(getPathFromName(fromName),getPathFromName(toName));
        }

        static public string getPathFromName(string name)
        {
            return Environment.SpecialFolder.ApplicationData + @"\BarGraph_" + name + ".xml";
        }
    }
}
