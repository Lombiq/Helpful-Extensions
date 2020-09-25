using Orchard.Services;
using System;

namespace Orchard.ContentManagement.Utilities
{
    public static class LazyFieldExtensions
    {
        public static void SetJsonGetter<T>(this LazyField<T> lazyField, IJsonConverter jsonConverter, Func<string> retrieve)
        {
            lazyField.Loader(() =>
            {
                var serializedValue = retrieve();

                return string.IsNullOrEmpty(serializedValue) ? default : jsonConverter.Deserialize<T>(serializedValue);
            });
        }

        public static void SetJsonSetter<T>(this LazyField<T> lazyField, IJsonConverter jsonConverter, Action<string> store)
        {
            lazyField.Setter(value =>
            {
                store(jsonConverter.Serialize(value));

                return value;
            });
        }

        public static void SetJsonGetterAndSetter<T>(this LazyField<T> lazyField, IJsonConverter jsonConverter, Func<string> retrieve, Action<string> store)
        {
            lazyField.SetJsonGetter(jsonConverter, retrieve);
            lazyField.SetJsonSetter(jsonConverter, store);
        }
    }
}