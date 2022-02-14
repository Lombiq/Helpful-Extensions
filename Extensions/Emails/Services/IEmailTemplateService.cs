using System.Threading.Tasks;
using Lombiq.HelpfulLibraries.Libraries.Shapes;
using Lombiq.HelpfulLibraries.Libraries.Utilities;
using OrchardCore.DisplayManagement;

namespace Lombiq.HelpfulExtensions.Extensions.Emails.Services
{
    public interface IEmailTemplateService
    {
        Task<string> RenderEmailTemplateAsync(string emailTemplateId, object model = null);
    }

    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IShapeFactory _shapeFactory;
        private readonly IShapeRenderer _shapeRenderer;

        public EmailTemplateService(IShapeFactory shapeFactory, IShapeRenderer shapeRenderer)
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

            var renderedShape = await _shapeRenderer.RenderAsync(shape);

            return renderedShape;
        }
    }
}
