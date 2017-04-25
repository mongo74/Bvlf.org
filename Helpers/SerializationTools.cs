using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace bvlf_v2.Helpers
{
    public static class SerializationTools
    {
        public static void SerialialiazeObjet(string path, object objectToSerialize)
        {
            var objectToSerializeType = objectToSerialize.GetType();
            var Serialiser = new XmlSerializer(objectToSerializeType);
            using (TextWriter txt = new StreamWriter(path))
            {
                Serialiser.Serialize(txt, objectToSerialize);
            }
        }

        public static T DeSerializeFile<T>(string path)
        {
            var exist = Directory.Exists(path);
            using (Stream stream = File.OpenRead(path))
            {
                var serializer = new XmlSerializer(typeof (T));
                return (T) serializer.Deserialize(stream);
            }
        }

        public static T DeSerializeString<T>(string xmlstring)
        {
            using (var reader = XmlReader.Create(new StringReader(xmlstring)))
            {
                reader.MoveToContent();
                var serializer = new XmlSerializer(typeof (T));
                return (T) serializer.Deserialize(reader);
            }
        }
    }
}