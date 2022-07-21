using Lombiq.HelpfulLibraries.Common.Utilities;
using Lombiq.HelpfulLibraries.OrchardCore.Shapes;
using OrchardCore.DisplayManagement;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Emails.Services;

public class ShapeBasedEmailTemplateService : IEmailTemplateService
{
    private readonly IShapeFactory _shapeFactory;
    private readonly IShapeRenderer _shapeRenderer;

    public ShapeBasedEmailTemplateService(IShapeFactory shapeFactory, IShapeRenderer shapeRenderer)
    {
        _shapeFactory = shapeFactory;
        _shapeRenderer = shapeRenderer;
    }

    public async Task<string> RenderEmailTemplateAsync(string emailTemplateId, object model = null)
    {
        ExceptionHelpers.ThrowIfNull(emailTemplateId, nameof(emailTemplateId));

        var shape = await _shapeFactory.CreateAsync($"EmailTemplate__{emailTemplateId}", Arguments.From(model ?? new { }));

        return await _shapeRenderer.RenderAsync(shape);
    }
}
