using System.IO;
using System.Runtime.Serialization.Json;

namespace URLExpander.Serializers
{
    public class JsonSerializer : ISerializer
    {
        public T ReadObject<T>(Stream stream) where T : class
        {
            var deserializer = new DataContractJsonSerializer(typeof(T));
            return deserializer.ReadObject(stream) as T;
        }
    }
}
