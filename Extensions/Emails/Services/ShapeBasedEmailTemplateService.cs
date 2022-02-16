using System.Threading.Tasks;
using Lombiq.HelpfulLibraries.Libraries.Shapes;
using Lombiq.HelpfulLibraries.Libraries.Utilities;
using OrchardCore.DisplayManagement;

namespace Lombiq.HelpfulExtensions.Extensions.Emails.Services
{
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

            var shape = model != null
                ? await _shapeFactory.CreateAsync($"EmailTemplate__{emailTemplateId}", Arguments.From(model))
                : await _shapeFactory.CreateAsync($"EmailTemplate__{emailTemplateId}");

            return await _shapeRenderer.RenderAsync(shape);
        }
    }
}
