using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace TwitchChatinator
{

    //Thank you [Rubens Farias](http://stackoverflow.com/users/113794/rubens-farias)
    //http://stackoverflow.com/questions/1907077/serialize-a-bitmap-in-c-net-to-xml


    public class StorableImage  : object, IXmlSerializable
    {
        public string Name { get; set; }
        public Image Image { get; set; }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader.IsEmptyElement) return;
            Name = reader.GetAttribute("Name");
            reader.MoveToContent();
            reader.ReadStartElement();
            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("Image");
                MemoryStream ms = null;
                byte[] buffer = new byte[256];
                int bytesRead;
                while ((bytesRead = reader.ReadContentAsBase64(
                    buffer,0,buffer.Length)) > 0 )
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
                    catch { }
                    ms.Dispose();
                }
                reader.ReadEndElement();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if(Image != null)
            {
                writer.WriteAttributeString("Name", Name);

                using (MemoryStream ms = new MemoryStream())
                {
                    this.Image.Save(ms, ImageFormat.Png);
                    byte[] bitmapData = ms.ToArray();
                    writer.WriteStartElement("Image");
                    writer.WriteBase64(bitmapData, 0, bitmapData.Length);
                    writer.WriteEndElement();
                }
            }
        }
    }
}
