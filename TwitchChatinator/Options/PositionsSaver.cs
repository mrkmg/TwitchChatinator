using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TwitchChatinator.Options
{
    public sealed class PositionsSaver
    {
        private static readonly Lazy<PositionsSaver> Lazy =
            new Lazy<PositionsSaver>(() => new PositionsSaver());

        public SerializableDictionary<string, Point> Positions;

        public PositionsSaver()
        {
            Positions = new SerializableDictionary<string, Point>();
            Load();
        }

        public static PositionsSaver Instance => Lazy.Value;

        public static Point Get(string name)
        {
            return Instance.Positions.ContainsKey(name) ? Instance.Positions[name] : Point.Empty;
        }

        public static void Put(string name, Point point)
        {
            if (Instance.Positions.ContainsKey(name))
            {
                Instance.Positions[name] = point;
            }
            else
            {
                Instance.Positions.Add(name, point);
            }
            Instance.Save();
        }

        private void Load()
        {
            if (File.Exists(GetPath()))
            {
                var reader = new XmlSerializer(typeof (SerializableDictionary<string, Point>));
                var stream = new StreamReader(GetPath());
                Positions = (SerializableDictionary<string, Point>) reader.Deserialize(stream);
                stream.Close();
            }
            else
            {
                Save();
            }
        }

        private void Save()
        {
            var writer = new XmlSerializer(typeof (SerializableDictionary<string, Point>));
            var stream = new StreamWriter(GetPath());

            writer.Serialize(stream, Positions);

            stream.Close();
        }

        public static string GetPath()
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

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            var keySerializer = new XmlSerializer(typeof (TKey));
            var valueSerializer = new XmlSerializer(typeof (TValue));

            var wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                var key = (TKey) keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                var value = (TValue) valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            var keySerializer = new XmlSerializer(typeof (TKey));
            var valueSerializer = new XmlSerializer(typeof (TValue));

            foreach (var key in Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                var value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }

        #endregion
    }
}