using OrchardCore.ContentManagement;

namespace Lombiq.HelpfulExtensions.Flows.Models
{
    public class AdditionalStylingPart : ContentPart
    {
        public string CustomClasses { get; set; }
        public bool RemoveGridExtensionClasses { get; set; }
    }
}
