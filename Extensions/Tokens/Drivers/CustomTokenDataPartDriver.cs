using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Piedone.HelpfulExtensions.Tokens.Models;

namespace Piedone.HelpfulExtensions.Tokens.Drivers
{
    [OrchardFeature(Constants.FeatureNames.Tokens)]
    public class CustomTokenDataPartDriver : ContentPartDriver<CustomTokenDataPart> { }
}