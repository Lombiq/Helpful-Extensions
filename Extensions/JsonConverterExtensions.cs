using Newtonsoft.Json;
using Orchard.Services;

namespace Orchard.Services
{
    public static class JsonConverterExtensions
    {
        public static bool TryDeserialize<T>(this IJsonConverter jsonConverter, string jsonString, out T result)
        {
            result = default(T);

            try
            {
                result = jsonConverter.Deserialize<T>(jsonString);
            }
            catch (JsonException)
            {
                return false;
            }

            return true;
        }
    }
}