using Orchard.ContentManagement.MetaData.Models;
using Piedone.HelpfulExtensions.Tokens.Models;
using System;
using System.Collections.Generic;

namespace Orchard.ContentManagement
{
    public static class TokenContentExtensions
    {
        public static void SetCustomTokenData(this IContent content, string key, object tokenData)
        {
            var customTokenDataPart = content.WeldCustomTokenDataPart();

            if (customTokenDataPart.TokenData.ContainsKey(key)) customTokenDataPart.TokenData[key] = tokenData;
            else customTokenDataPart.TokenData.Add(key, tokenData);
        }

        public static void SetCustomTokenData(this IContent content, IDictionary<string, object> tokenData, bool clearExistingTokens = false)
        {
            if (clearExistingTokens) content.ClearCustomTokens();

            foreach (var data in tokenData)
            {
                content.SetCustomTokenData(data.Key, data.Value);
            }
        }

        public static TData GetCustomTokenData<TData>(this IContent content, string key, Func<TData> defaultValue = null)
        {
            if (defaultValue == null) defaultValue = new Func<TData>(() => default(TData));

            var customTokenDataPart = content.As<CustomTokenDataPart>();
            if (customTokenDataPart == null) return defaultValue();

            if (!customTokenDataPart.TokenData.ContainsKey(key)) return defaultValue();

            var tokenData = customTokenDataPart.TokenData[key];
            if (!(tokenData is TData)) return defaultValue();

            return (TData)tokenData;
        }

        public static void ClearCustomTokens(this IContent content)
        {
            var customTokenDataPart = content.WeldCustomTokenDataPart();

            customTokenDataPart.TokenData.Clear();
        }

        public static CustomTokenDataPart WeldCustomTokenDataPart(this IContent content)
        {
            var customTokenDataPart = content.As<CustomTokenDataPart>();
            if (customTokenDataPart == null)
            {
                customTokenDataPart = new CustomTokenDataPart
                {
                    TypePartDefinition = new ContentTypePartDefinition(
                        new ContentPartDefinition(nameof(CustomTokenDataPart)),
                        new SettingsDictionary())
                };

                content.ContentItem.Weld(customTokenDataPart);
            }

            return customTokenDataPart;
        }
    }
}