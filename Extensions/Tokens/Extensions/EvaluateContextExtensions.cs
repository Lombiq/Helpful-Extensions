using Orchard.ContentManagement;
using System;

namespace Orchard.Tokens
{
    public static class EvaluateContextExtensions
    {
        public static IContent GetContentItem(this EvaluateContext context) =>
            context.Data.ContainsKey("Content") ? context.Data["Content"] as IContent : null;

        public static EvaluateFor<TData> ForCustomTokenData<TData>(
            this EvaluateContext context, 
            string target, 
            Func<TData> defaultValue = null)
        {
            var content = context.GetContentItem();

            return context.For(target, () => 
                content != null ? 
                    content.GetCustomTokenData(target, defaultValue) : 
                    (defaultValue == null ? default(TData) : defaultValue()));
        }
    }
}