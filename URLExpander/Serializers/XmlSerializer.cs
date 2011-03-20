using System.IO;
using LegacyXmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace URLExpander.Serializers
{
    public class XmlSerializer : ISerializer
    {
        public T ReadObject<T>(Stream stream) where T : class
        {
            var serializer = new LegacyXmlSerializer(typeof(T));
            return serializer.Deserialize(stream) as T;
        }
    }
}
