using Lombiq.HelpfulExtensions.Extensions.Flows.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Flows.Drivers
{
    public class AdditionalStylingPartDisplay : ContentDisplayDriver
    {
        public override IDisplayResult Edit(ContentItem contentItem, IUpdateModel updater)
        {
            var additionalStylingPart = contentItem.As<AdditionalStylingPart>();

            if (additionalStylingPart == null)
            {
                return null;
            }

            return Initialize<AdditionalStylingPart>($"{nameof(AdditionalStylingPart)}_Edit", m =>
            {
                m.CustomClasses = additionalStylingPart.CustomClasses;
                m.RemoveGridExtensionClasses = additionalStylingPart.RemoveGridExtensionClasses;
            }).Location("Footer:3");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentItem contentItem, IUpdateModel updater)
        {
            var additionalStylingPart = contentItem.As<AdditionalStylingPart>();

            if (additionalStylingPart == null)
            {
                return null;
            }

            await contentItem.AlterAsync<AdditionalStylingPart>(model => updater.TryUpdateModelAsync(model, Prefix));

            return Edit(contentItem, updater);
        }
    }
}
