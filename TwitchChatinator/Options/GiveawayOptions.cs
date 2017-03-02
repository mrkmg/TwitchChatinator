using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using TwitchChatinator.Forms.Components;
using TwitchChatinator.Libs;

namespace TwitchChatinator.Options
{
    public class GiveawayOptions
    {
        public static GiveawayOptions PreviewOptions;

        public GiveawayOptions()
        {
            Width = 400;

            MarginTop = 5;
            MarginBottom = 5;
            MarginLeft = 5;
            MarginRight = 5;

            Spacing = 3;

            ChromaKey = Color.Black;

            TitleFontColor = Color.White;
            TitleFont = new Font("Segoe UI", 15.75f, FontStyle.Bold);

            RollerFontColor = Color.White;
            RollerFont = new Font("Segoe UI", 20.25f, FontStyle.Italic);

            EntriesFontColor = Color.White;
            EntriesFont = new Font("Segoe UI", 12f, FontStyle.Bold);

            BackgroundImage = new StorableImage();
            ForegroundImage = new StorableImage();
        }

        [XmlElement]
        public int Width { get; set; }

        [XmlElement]
        public int MarginTop { get; set; }

        [XmlElement]
        public int MarginBottom { get; set; }

        [XmlElement]
        public int MarginLeft { get; set; }

        [XmlElement]
        public int MarginRight { get; set; }

        [XmlElement]
        public int Spacing { get; set; }

        [XmlIgnore]
        public Color ChromaKey { get; set; }

        [XmlIgnore]
        public Color TitleFontColor { get; set; }

        [XmlIgnore]
        public Font TitleFont { get; set; }

        [XmlIgnore]
        public Color RollerFontColor { get; set; }

        [XmlIgnore]
        public Font RollerFont { get; set; }

        [XmlIgnore]
        public Color EntriesFontColor { get; set; }

        [XmlIgnore]
        public Font EntriesFont { get; set; }

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
        public string TitleFontColorCode
        {
            get { return ColorTranslator.ToHtml(TitleFontColor); }
            set { TitleFontColor = ColorTranslator.FromHtml(value); }
        }

        [XmlElement]
        public string RollerFontColorCode
        {
            get { return ColorTranslator.ToHtml(RollerFontColor); }
            set { RollerFontColor = ColorTranslator.FromHtml(value); }
        }

        [XmlElement]
        public string EntriesFontColorCode
        {
            get { return ColorTranslator.ToHtml(EntriesFontColor); }
            set { EntriesFontColor = ColorTranslator.FromHtml(value); }
        }


        [XmlElement]
        public string TitleFontName
        {
            get { return FontXmlConverter.ConvertToString(TitleFont); }
            set { TitleFont = FontXmlConverter.ConvertToFont(value); }
        }

        [XmlElement]
        public string RollerFontName
        {
            get { return FontXmlConverter.ConvertToString(RollerFont); }
            set { RollerFont = FontXmlConverter.ConvertToFont(value); }
        }

        [XmlElement]
        public string EntriesFontName
        {
            get { return FontXmlConverter.ConvertToString(EntriesFont); }
            set { EntriesFont = FontXmlConverter.ConvertToFont(value); }
        }


        //TODO - Add in exception
        public static GiveawayOptions Load(string name)
        {
            if (name == "_preview")
            {
                if (PreviewOptions != null)
                {
                    return PreviewOptions;
                }
                return new GiveawayOptions();
            }

            var reader = new XmlSerializer(typeof (GiveawayOptions));
            var stream = new StreamReader(GetPathFromName(name));
            var obj = new GiveawayOptions();
            obj = (GiveawayOptions) reader.Deserialize(stream);

            stream.Close();
            return obj;
        }

        //TODO - Add in exception
        public void Save(string name)
        {
            var writer = new XmlSerializer(typeof (GiveawayOptions));
            var stream = new StreamWriter(GetPathFromName(name));

            writer.Serialize(stream, this);

            stream.Close();
        }

        public static void CreateNew(string name)
        {
            var obj = new GiveawayOptions();
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
            var directory = Program.AppDataFolder() + @"\Giveaways";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var files = Directory.GetFiles(directory);

            foreach (var file in files)
            {
                names.Add(Path.GetFileNameWithoutExtension(file));
            }

            names.Sort();

            return names;
        }

        public static string GetPathFromName(string name)
        {
            return Program.AppDataFolder() + @"\Giveaways\" + name + ".xml";
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
            catch (Exception ex)
            {
                e.Cancel = true;
                e.Message = "Invalid Characters";
            }
        }
    }
}