using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using System.Collections.Generic;

namespace Piedone.HelpfulExtensions.Tokens.Models
{
    [OrchardFeature(Constants.FeatureNames.Tokens)]
    public class CustomTokenDataPart : ContentPart
    {
        public IDictionary<string, object> TokenData { get; set; } = new Dictionary<string, object>();
    }
}