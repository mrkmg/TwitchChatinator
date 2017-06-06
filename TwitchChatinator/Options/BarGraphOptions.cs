using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using TwitchChatinator.Forms.Components;
using TwitchChatinator.Libs;

namespace TwitchChatinator.Options
{
    public class BarGraphOptions
    {
        public static BarGraphOptions PreviewOptions;

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
            TransparentBackground = false;
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

        [XmlElement]
        public bool TransparentBackground { get; set; }

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


        //TODO - Add in exception
        public static BarGraphOptions Load(string name)
        {
            if (name == "_preview")
            {
                if (PreviewOptions != null)
                {
                    return PreviewOptions;
                }
                return new BarGraphOptions();
            }

            var reader = new XmlSerializer(typeof (BarGraphOptions));
            var stream = new StreamReader(GetPathFromName(name));
            var obj = (BarGraphOptions) reader.Deserialize(stream);

            stream.Close();
            return obj;
        }

        //TODO - Add in exception
        public void Save(string name)
        {
            var writer = new XmlSerializer(typeof (BarGraphOptions));
            var stream = new StreamWriter(GetPathFromName(name));

            writer.Serialize(stream, this);

            stream.Close();
        }

        public static void CreateNew(string name)
        {
            var obj = new BarGraphOptions();
            obj.Save(name);
        }

        public static void Remove(string name)
        {
            File.Delete(GetPathFromName(name));
        }

        public static void Rename(string fromName, string toName)
        {
            File.Move(GetPathFromName(fromName), GetPathFromName(toName));
        }

        public static List<string> GetAvaliable()
        {
            var names = new List<string>();
            var directory = GetPath();

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var files = Directory.GetFiles(directory);

            foreach (var file in files)
            {
                if (file.EndsWith(".xbar"))
                    names.Add(Path.GetFileNameWithoutExtension(file));
            }

            names.Sort();

            return names;
        }

        public static string GetPath()
        {
            return Program.AppDataFolder() + @"\Polls\BarGraphs";
        }

        public static string GetPathFromName(string name)
        {
            return GetPath() + @"\" + name + ".xbar";
        }

        public static void ValidateNameHandler(object sender, InputBoxValidatingArgs e)
        {
            if (e.Text.Length == 0)
            {
                e.Cancel = true;
                e.Message = "Name is Required";
            }

            try
            {
                new FileInfo(GetPathFromName(e.Text));
            }
            catch (Exception)
            {
                e.Cancel = true;
                e.Message = "Invalid Characters";
            }
        }

        public static void Import(string filename)
        {
            var fileInfo = new FileInfo(filename);

            var name = fileInfo.Name;

            var i = 0;
            while (File.Exists(GetPath() + @"\" + name))
            {
                name = fileInfo.Name.Substring(0, fileInfo.Name.Length - 5) + " - " + (++i) + ".xbar";
            }
                
            File.Copy(filename, GetPath() + @"\" + name);
        }

        public static void Export(string name)
        {
            var dialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "XBar Files (*.xbar)|*.xbar",
                Title = "Save Pie Graph",
                OverwritePrompt = true
            };


            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.Copy(GetPathFromName(name), dialog.FileName, true);
            }
        }
    }
}