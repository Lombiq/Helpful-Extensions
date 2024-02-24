using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Entities;
using OrchardCore.Recipes.Models;
using OrchardCore.Users.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Services;

public class OrchardExportToRecipeConverter : IOrchardExportToRecipeConverter
{
    private readonly IContentManager _contentManager;
    private readonly IIdGenerator _idGenerator;
    private readonly IEnumerable<IOrchardExportConverter> _exportConverters;
    private readonly IEnumerable<IOrchardContentConverter> _contentConverters;
    private readonly IEnumerable<IOrchardUserConverter> _userConverters;
    private readonly IContentDefinitionManager _contentDefinitionManager;

    public OrchardExportToRecipeConverter(
        IContentDefinitionManager contentDefinitionManager,
        IContentManager contentManager,
        IIdGenerator idGenerator,
        IEnumerable<IOrchardExportConverter> exportConverters,
        IEnumerable<IOrchardContentConverter> contentConverters,
        IEnumerable<IOrchardUserConverter> userConverters)
    {
        _contentManager = contentManager;
        _idGenerator = idGenerator;
        _exportConverters = exportConverters;
        _contentConverters = contentConverters;
        _userConverters = userConverters;
        _contentDefinitionManager = contentDefinitionManager;
    }

    public async Task<string> ConvertAsync(XDocument export)
    {
        var contentItems = new List<ContentItem>();
        var contents = export.XPathSelectElement("//Content")?.Elements() ?? Enumerable.Empty<XElement>();
        var contentTypes = (await _contentDefinitionManager.ListTypeDefinitionsAsync())
            .Select(definition => definition.Name)
            .ToList();

        foreach (var content in contents)
        {
            if (await CreateContentItemAsync(content, contentTypes) is { } contentItem)
            {
                contentItem.ContentItemId ??= _idGenerator.GenerateUniqueId();
                contentItem.ContentItemVersionId ??= _idGenerator.GenerateUniqueId();

                await _contentConverters
                    .Where(converter => converter.IsApplicable(content))
                    .OrderBy(converter => converter.Order)
                    .AwaitEachAsync(converter => converter.ImportAsync(content, contentItem));

                contentItems.Add(contentItem);
            }
            else if (content.Name == nameof(User))
            {
                var customUserConverter = _userConverters.FirstOrDefault(converter => converter.IgnoreDefaultConverter);
                var userConverter = customUserConverter ?? _userConverters.First();
                await userConverter.ImportAsync(content);
            }
        }

        foreach (var converter in _exportConverters)
        {
            await converter.UpdateContentItemsAsync(export, contentItems);
        }

        var recipe = JsonSerializer.SerializeToNode(new RecipeDescriptor())!;
        recipe["steps"] = JsonSerializer.SerializeToNode(new[] { new { name = "content", data = contentItems } });

        return recipe.ToString();
    }

    private async Task<ContentItem> CreateContentItemAsync(XElement content, List<string> contentTypes)
    {
        foreach (var converter in _contentConverters.OrderBy(converter => converter.Order))
        {
            if (converter.IsApplicable(content) && await converter.CreateContentItemAsync(content) is { } contentItem)
            {
                return contentItem;
            }
        }

        return contentTypes.Contains(content.Name.LocalName)
            ? await _contentManager.NewAsync(content.Name.LocalName)
            : null;
    }
}
