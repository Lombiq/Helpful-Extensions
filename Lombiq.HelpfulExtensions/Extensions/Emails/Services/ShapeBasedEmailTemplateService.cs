using Lombiq.HelpfulLibraries.Common.Utilities;
using Lombiq.HelpfulLibraries.OrchardCore.Shapes;
using OrchardCore.DisplayManagement;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Emails.Services;

public class ShapeBasedEmailTemplateService(IShapeFactory shapeFactory, IShapeRenderer shapeRenderer) : IEmailTemplateService
{
    public async Task<string> RenderEmailTemplateAsync(string emailTemplateId, object model = null)
    {
        ExceptionHelpers.ThrowIfNull(emailTemplateId, nameof(emailTemplateId));

        var shape = await shapeFactory.CreateAsync($"EmailTemplate__{emailTemplateId}", Arguments.From(model ?? new { }));

        return await shapeRenderer.RenderAsync(shape);
    }
}
