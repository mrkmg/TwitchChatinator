using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Serialization;

namespace TwitchChatinator.Libs
{
    public class SerializableFont
    {
        public SerializableFont()
        {
            FontValue = null;
        }

        public SerializableFont(Font font)
        {
            FontValue = font;
        }

        [XmlIgnore]
        public Font FontValue { get; set; }

        [XmlElement("FontValue")]
        public string SerializeFontAttribute
        {
            get { return FontXmlConverter.ConvertToString(FontValue); }
            set { FontValue = FontXmlConverter.ConvertToFont(value); }
        }

        public static implicit operator Font(SerializableFont serializeableFont)
        {
            return serializeableFont?.FontValue;
        }

        public static implicit operator SerializableFont(Font font)
        {
            return new SerializableFont(font);
        }
    }

    public static class FontXmlConverter
    {
        public static string ConvertToString(Font font)
        {
            try
            {
                if (font != null)
                {
                    var converter = TypeDescriptor.GetConverter(typeof (Font));
                    return converter.ConvertToString(font);
                }
                return null;
            }
            catch
            {
                Debug.WriteLine("Unable to convert");
            }
            return null;
        }

        public static Font ConvertToFont(string fontString)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof (Font));
                return (Font) converter.ConvertFromString(fontString);
            }
            catch
            {
                Debug.WriteLine("Unable to convert");
            }
            return null;
        }
    }
}