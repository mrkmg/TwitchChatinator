using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TwitchChatinator.Options
{
    //Thank you [Rubens Farias](http://stackoverflow.com/users/113794/rubens-farias)
    //http://stackoverflow.com/questions/1907077/serialize-a-bitmap-in-c-net-to-xml


    public class StorableImage : object, IXmlSerializable
    {
        public string Name { get; set; }
        public Image Image { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement) return;
            Name = reader.GetAttribute("Name");
            reader.MoveToContent();
            reader.ReadStartElement();
            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("Image");
                MemoryStream ms = null;
                var buffer = new byte[256];
                int bytesRead;
                while ((bytesRead = reader.ReadContentAsBase64(
                    buffer, 0, buffer.Length)) > 0)
                {
                    if (ms == null) ms = new MemoryStream(bytesRead);
                    ms.Write(buffer, 0, bytesRead);
                }
                if (ms != null)
                {
                    try
                    {
                        Image = Image.FromStream(ms);
                    }
                    catch
                    {
                    }
                    ms.Dispose();
                }
                reader.ReadEndElement();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            if (Image != null)
            {
                writer.WriteAttributeString("Name", Name);

                using (var ms = new MemoryStream())
                {
                    Image.Save(ms, ImageFormat.Png);
                    var bitmapData = ms.ToArray();
                    writer.WriteStartElement("Image");
                    writer.WriteBase64(bitmapData, 0, bitmapData.Length);
                    writer.WriteEndElement();
                }
            }
        }
    }
}