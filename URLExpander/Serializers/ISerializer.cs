using System.IO;

namespace URLExpander.Serializers
{
    public interface ISerializer
    {
        T ReadObject<T>(Stream stream) where T : class;
    }
}