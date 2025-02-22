using Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Entities;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Services;

public class OrchardExportToRecipeConverter : IOrchardExportToRecipeConverter
{
    private readonly int batchSize = 50;

    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IContentManager _contentManager;
    private readonly IIdGenerator _idGenerator;
    private readonly IEnumerable<IOrchardContentConverter> _contentConverters;
    private readonly IEnumerable<IOrchardUserConverter> _userConverters;
    private readonly IContentDefinitionManager _contentDefinitionManager;
    private readonly IUrlHelperFactory _urlHelperFactory;

    public OrchardExportToRecipeConverter(
        IActionContextAccessor actionContextAccessor,
        IContentDefinitionManager contentDefinitionManager,
        IContentManager contentManager,
        IIdGenerator idGenerator,
        IEnumerable<IOrchardContentConverter> contentConverters,
        IEnumerable<IOrchardUserConverter> userConverters,
        IUrlHelperFactory urlHelperFactory)
    {
        _actionContextAccessor = actionContextAccessor;
        _contentManager = contentManager;
        _idGenerator = idGenerator;
        _contentConverters = contentConverters;
        _userConverters = userConverters;
        _contentDefinitionManager = contentDefinitionManager;
        _urlHelperFactory = urlHelperFactory;
    }

    public async Task<object> ConvertAsync(XDocument export, int page)
    {
        var contents = export.XPathSelectElement("//Content")?.Elements() ?? [];
        var contentList = contents.ToList();
        var contentTypes = (await _contentDefinitionManager.ListTypeDefinitionsAsync())
            .Select(definition => definition.Name)
            .ToList();

        var totalItems = contentList.Count;
        var totalPages = (int)Math.Ceiling((double)totalItems / batchSize);

        var batch = contentList.Skip((page - 1) * batchSize).Take(batchSize);
        var contentItems = new List<ContentItem>();

        foreach (var content in batch)
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

        var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext!);

        var hasNextPage = page < totalPages;
        var nextUrl = hasNextPage ? urlHelper.Action(
            nameof(Convert),
            typeof(OrchardRecipeMigrationAdminController).ControllerName(),
            new { page = page + 1 })
            : null;

        return new
        {
            Processed = page * batchSize > totalItems ? totalItems : page * batchSize,
            Total = totalItems,
            NextPage = nextUrl,
            ContentItems = contentItems,
        };
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
