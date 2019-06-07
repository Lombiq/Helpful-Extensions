using Orchard.ContentManagement;
using Orchard.Localization;
using System;

namespace Orchard.Tokens
{
    public static class TokenProviderExtensions
    {
        public static DescribeFor ContentPartToken<TPart>(
            this DescribeFor describeFor,
            string tokenName,
            LocalizedString name,
            LocalizedString description) where TPart : ContentPart =>
            describeFor.Token($"{typeof(TPart).Name}.{tokenName}", name, description);

        public static EvaluateFor<IContent> ContentPartToken<TPart>(
            this EvaluateFor<IContent> evaluateFor,
            string tokenName,
            Func<TPart, object> tokenValue) where TPart : ContentPart =>
            evaluateFor.Token($"{typeof(TPart).Name}.{tokenName}", content => content.Has<TPart>() ? tokenValue(content.As<TPart>()) : "");
    }
}