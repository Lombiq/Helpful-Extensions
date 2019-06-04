using Orchard.ContentManagement;
using Piedone.HelpfulLibraries.Contents;
using System;

namespace Orchard.Tokens
{
    public static class EvaluateForExtensions
    {
        public static EvaluateFor<IContent> ContentPartToken<TPart>(
            this EvaluateFor<IContent> evaluateFor,
            string tokenName,
            Func<TPart, object> tokenValue) where TPart : ContentPart =>
            evaluateFor.Token(tokenName, content => tokenValue(content.AsOrThrow<TPart>()));
    }
}