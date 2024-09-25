using Lombiq.HelpfulExtensions.Extensions.Security.Models;
using Lombiq.HelpfulExtensions.Extensions.Security.ViewModels;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Security.Driver;

public sealed class StrictSecuritySettingsDisplayDriver : ContentTypeDefinitionDisplayDriver
{
    public override IDisplayResult Edit(ContentTypeDefinition model, BuildEditorContext context) =>
        Initialize<StrictSecuritySettingsViewModel>("StrictSecuritySetting_Edit", viewModel =>
        {
            var settings = model.GetSettings<StrictSecuritySettings>();

            viewModel.Enabled = settings?.Enabled == true;
        }).PlaceInContent(5);

    public override async Task<IDisplayResult> UpdateAsync(ContentTypeDefinition model, UpdateTypeEditorContext context)
    {
        var viewModel = await context.CreateModelAsync<StrictSecuritySettingsViewModel>(Prefix);

        // Securable must be enabled for Strict Securable to make sense. Also checked on the client side too.
        if (model.GetSettings<ContentTypeSettings>()?.Securable != true) viewModel.Enabled = false;

        context.Builder.MergeSettings<StrictSecuritySettings>(settings => settings.Enabled = viewModel.Enabled);

        return await EditAsync(model, context);
    }
}
