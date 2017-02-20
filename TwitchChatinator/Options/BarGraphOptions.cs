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
        public static BarGraphOptions PreviewOptions;

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
        [XmlIgnore]
        public Font OptionFont { get; set; }

        [XmlIgnore]
        public Color CountFontColor { get; set; }
        [XmlIgnore]
        public Font CountFont { get; set; }

        [XmlIgnore]
        public Color TitleFontColor { get; set; }
        [XmlIgnore]
        public Font TitleFont { get; set; }

        [XmlElement]
        public bool AllowMulti { get; set; }

        [XmlElement]
        public string TotalPosition { get; set; }

        [XmlElement]
        public StorableImage BackgroundImage { get; set; }
        [XmlElement]
        public StorableImage ForegroundImage { get; set; }


        //Special for XMLSerialization
        //Thank you [BenAlabaster] (https://stackoverflow.com/users/40650/benalabaster)
        //https://stackoverflow.com/questions/376234/best-solution-for-xmlserializer-and-system-drawing-color/376254#376254
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
        public string TitleFontColorCode
        {
            get { return ColorTranslator.ToHtml(TitleFontColor); }
            set { TitleFontColor = ColorTranslator.FromHtml(value); }
        }


        [XmlElement]
        public string OptionFontName
        {
            get { return FontXmlConverter.ConvertToString(OptionFont); }
            set { OptionFont = FontXmlConverter.ConvertToFont(value); }
        }
        [XmlElement]
        public string CountFontName
        {
            get { return FontXmlConverter.ConvertToString(CountFont); }
            set { CountFont = FontXmlConverter.ConvertToFont(value); }
        }
        [XmlElement]
        public string TitleFontName
        {
            get { return FontXmlConverter.ConvertToString(TitleFont); }
            set { TitleFont = FontXmlConverter.ConvertToFont(value); }
        }

        public BarGraphOptions()
        {
            Width = 400;
            Height = 200;

            MarginTop = 5;
            MarginBottom = 5;
            MarginLeft = 5;
            MarginRight = 5;

            BarSpacing = 3;

            ChromaKey = Color.Black;
            Option1Color = Color.Maroon;
            Option2Color = Color.RoyalBlue;
            Option3Color = Color.Orange;
            Option4Color = Color.ForestGreen;

            OptionFontColor = Color.White;
            OptionFont = new Font("Segoe UI", 15.75f, FontStyle.Bold);

            CountFontColor = Color.White;
            CountFont = new Font("Segoe UI", 10.25f, FontStyle.Italic);

            TitleFontColor = Color.White;
            TitleFont = new Font("Segoe UI", 15.75f, FontStyle.Bold);
            
            AllowMulti = false;

            TotalPosition = "Bottom";

            BackgroundImage = new StorableImage();
            ForegroundImage = new StorableImage();
        }


        //TODO - Add in exception
        static public BarGraphOptions Load(string name)
        {
            if (name == "_preview")
            {
                if (PreviewOptions != null)
                {
                    return PreviewOptions;
                }
                else
                {
                    return new BarGraphOptions();
                }
            }

            var reader = new XmlSerializer(typeof(BarGraphOptions));
            var stream = new StreamReader(getPathFromName(name));
            var obj = new BarGraphOptions();
            obj = (BarGraphOptions)reader.Deserialize(stream);

            stream.Close();
            return obj;
        }

        //TODO - Add in exception
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
            string directory = getPath();

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

        static public string getPath()
        {
            return Program.AppDataFolder() + @"\Polls\BarGraphs";
        }

        static public string getPathFromName(string name)
        {
            return getPath() + @"\" + name + ".xbar";
        }

        static public void ValidateNameHandler(object sender, InputBoxValidatingArgs e)
        {
            if (e.Text.Length == 0)
            {
                e.Cancel = true;
                e.Message = "Name is Required";
            }

            try
            {
                new FileInfo(getPathFromName(e.Text));
            }
            catch(Exception ex)
            {
                e.Cancel = true;
                e.Message = "Invalid Characters";
            }
        }

        static public void Export(string name)
        {
            System.Windows.Forms.SaveFileDialog Dialog = new System.Windows.Forms.SaveFileDialog();

            Dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Dialog.Filter = "XBar Files (*.xbar)|*.xbar";
            Dialog.Title = "Save Pie Graph";
            Dialog.OverwritePrompt = true;

            if(Dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.Copy(getPathFromName(name), Dialog.FileName, true);
            }
        }
    }
}
