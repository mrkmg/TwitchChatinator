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
    public sealed class PositionsSaver
    {

        private static readonly Lazy<PositionsSaver> lazy =
            new Lazy<PositionsSaver>(() => new PositionsSaver());

        public static PositionsSaver Instance { get { return lazy.Value; } }

        public SerializableDictionary<string, Point> Positions;

        public PositionsSaver()
        {
            Positions = new SerializableDictionary<string, Point>();
            load();
        }

        static public Point get(string name)
        {
            if(Instance.Positions.ContainsKey(name))
            {
                return Instance.Positions[name];
            }
            else
            {
                return Point.Empty;
            }
        }

        static public void put(string name, Point point)
        {
            if(Instance.Positions.ContainsKey(name))
            {
                Instance.Positions[name] = point;
            }
            else
            {
                Instance.Positions.Add(name, point);
            }
            Instance.save();
        }

        private void load()
        {
            if(File.Exists(getPath()))
            {
                var reader = new XmlSerializer(typeof(SerializableDictionary<string, Point>));
                var stream = new StreamReader(getPath());
                Positions = (SerializableDictionary<string, Point>)reader.Deserialize(stream);
                stream.Close();
            }
            else
            {
                save();
            }
        }

        private void save()
        {
            var writer = new XmlSerializer(typeof(SerializableDictionary<string, Point>));
            var stream = new StreamWriter(getPath());
            
            writer.Serialize(stream, Positions);

            stream.Close();
        }

        static public string getPath()
        {
            return Program.AppDataFolder() + @"\positions.xml";
        }
    }


    //http://weblogs.asp.net/pwelter34/444961
    // Thank you Paul Welter!
    public class SerializableDictionary<TKey, TValue>
        : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion
    }
}
