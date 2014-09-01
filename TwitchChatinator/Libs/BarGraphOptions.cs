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
    public class BarGraphOptions
    {
        [XmlElement]
        public int Width { get; set; }
        [XmlElement]
        public int Height { get; set; }

        [XmlElement]
        public int MarginTop { get; set; }
        [XmlElement]
        public int MarginBottom { get; set; }
        [XmlElement]
        public int MarginLeft { get; set; }
        [XmlElement]
        public int MarginRight { get; set; }

        [XmlElement]
        public int BarSpacing { get; set; }

        [XmlIgnore]
        public Color ChromaKey { get; set; }
        [XmlIgnore]
        public Color Option1Color { get; set; }
        [XmlIgnore]
        public Color Option2Color { get; set; }
        [XmlIgnore]
        public Color Option3Color { get; set; }
        [XmlIgnore]
        public Color Option4Color { get; set; }

        [XmlIgnore]
        public Color OptionFontColor { get; set; }
        [XmlElement]
        public SerializableFont OptionFont { get; set; }

        [XmlIgnore]
        public Color CountFontColor { get; set; }
        [XmlElement]
        public SerializableFont CountFont { get; set; }

        [XmlIgnore]
        public Color TotalFontColor { get; set; }
        [XmlElement]
        public SerializableFont TotalFont { get; set; }

        [XmlElement]
        public bool AllowMulti { get; set; }

        [XmlElement]
        public string TotalPosition { get; set; }


        //Special for XMLSerialization
        [XmlElement]
        public string ChromaKeyCode
        {
            get { return ColorTranslator.ToHtml(ChromaKey); }
            set { ChromaKey = ColorTranslator.FromHtml(value); }
        }
        [XmlElement]
        public string Option1ColorCode
        {
            get { return ColorTranslator.ToHtml(Option1Color); }
            set { Option1Color = ColorTranslator.FromHtml(value); }
        }
        [XmlElement]
        public string Option2ColorCode
        {
            get { return ColorTranslator.ToHtml(Option2Color); }
            set { Option2Color = ColorTranslator.FromHtml(value); }
        }
        [XmlElement]
        public string Option3ColorCode
        {
            get { return ColorTranslator.ToHtml(Option3Color); }
            set { Option3Color = ColorTranslator.FromHtml(value); }
        }
        [XmlElement]
        public string Option4ColorCode
        {
            get { return ColorTranslator.ToHtml(Option4Color); }
            set { Option4Color = ColorTranslator.FromHtml(value); }
        }
        [XmlElement]
        public string OptionFontColorCode
        {
            get { return ColorTranslator.ToHtml(OptionFontColor); }
            set { OptionFontColor = ColorTranslator.FromHtml(value); }
        }
        [XmlElement]
        public string CountFontColorCode
        {
            get { return ColorTranslator.ToHtml(CountFontColor); }
            set { CountFontColor = ColorTranslator.FromHtml(value); }
        }
        [XmlElement]
        public string TotalFontColorCode
        {
            get { return ColorTranslator.ToHtml(TotalFontColor); }
            set { TotalFontColor = ColorTranslator.FromHtml(value); }
        }


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
            var stream = new StreamWriter(getPathFromName(name));

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
            obj.CountFont = new Font("Segoe UI", 10.25f, FontStyle.Italic);

            obj.TotalFontColor = Color.White;
            obj.TotalFont = new Font("Segoe UI", 15.75f, FontStyle.Bold);

            obj.AllowMulti = false;

            obj.TotalPosition = "Bottom Middle";

            obj.Save(name);
        }

        static public void Remove(string name)
        {
            File.Delete(getPathFromName(name));
        }

        static public void Rename(string fromName, string toName)
        {
            File.Move(getPathFromName(fromName), getPathFromName(toName));
        }

        static public List<string> GetAvaliable()
        {
            var names = new List<string>();
            string directory = Program.AppDataFolder() + @"\Polls\BarGraphs";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string[] files = Directory.GetFiles(directory);

            foreach (string file in files)
            {
                names.Add(Path.GetFileNameWithoutExtension(file));
            }

            names.Sort();

            return names;
        }

        static public string getPathFromName(string name)
        {
            return Program.AppDataFolder() + @"\Polls\BarGraphs\" + name + ".xml";
        }
    }
}
