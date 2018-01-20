using Orchard.Autoroute.Models;
using Orchard.Autoroute.Settings;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Environment.Extensions;
using System.Globalization;

namespace Piedone.HelpfulExtensions.Autoroute
{
    [OrchardFeature(Constants.FeatureNames.Autoroute)]
    public static class ContentTypeDefinitionBuilderExtensions
    {
        private static string Prefix = $"{nameof(AutorouteSettings)}.";


        public static ContentTypeDefinitionBuilder WithAutoroutePart(
            this ContentTypeDefinitionBuilder builder, AutorouteSettings settings) =>
            builder.WithPart(nameof(AutoroutePart), configuration => configuration
                .WithSetting(Prefix + nameof(AutorouteSettings.PerItemConfiguration), settings.PerItemConfiguration.ToString(CultureInfo.InvariantCulture))
                .WithSetting(Prefix + nameof(AutorouteSettings.AllowCustomPattern), settings.AllowCustomPattern.ToString(CultureInfo.InvariantCulture))
                .WithSetting(Prefix + nameof(AutorouteSettings.UseCulturePattern), settings.UseCulturePattern.ToString(CultureInfo.InvariantCulture))
                .WithSetting(Prefix + nameof(AutorouteSettings.AutomaticAdjustmentOnEdit), settings.AutomaticAdjustmentOnEdit.ToString(CultureInfo.InvariantCulture))
                .WithSetting(Prefix + nameof(AutorouteSettings.PatternDefinitions), settings.PatternDefinitions)
                .WithSetting(Prefix + nameof(AutorouteSettings.DefaultPatternDefinitions), settings.DefaultPatternDefinitions)
                .WithSetting(Prefix + nameof(AutorouteSettings.DefaultPatternIndex), settings.DefaultPatternIndex));
    }
}