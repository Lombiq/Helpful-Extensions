using Newtonsoft.Json;

namespace Orchard.Services
{
    public static class JsonConverterExtensions
    {
        public static bool TryDeserialize<T>(this IJsonConverter jsonConverter, string jsonString, out T result)
        {
            result = default(T);

            if (string.IsNullOrEmpty(jsonString)) return false;

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