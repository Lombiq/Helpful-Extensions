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
            Func<TPart, object> tokenValue,
            string chainTarget = null) where TPart : ContentPart
        {
            var token = $"{typeof(TPart).Name}.{tokenName}";

            evaluateFor.Token(token, content => content.Has<TPart>() ? tokenValue(content.As<TPart>()) : new object());

            if (!string.IsNullOrWhiteSpace(chainTarget))
                evaluateFor.Chain(token, chainTarget, content => content.Has<TPart>() ? tokenValue(content.As<TPart>()) : new object());

            return evaluateFor;
        }
    }
}