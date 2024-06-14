using Lombiq.HelpfulExtensions.Extensions.Security.Models;
using Microsoft.AspNetCore.Authorization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Contents.Security;
using OrchardCore.Modules;
using OrchardCore.Security;
using OrchardCore.Security.Permissions;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Security.Services;

[RequireFeatures(FeatureIds.Security)]
public class StrictSecurityPermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private static readonly Dictionary<string, IList<string>> _permissionTemplates = ContentTypePermissionsHelper
        .PermissionTemplates
        .ToDictionary(
            pair => pair.Key,
            pair => GetPermissionTemplates(pair.Value, []));

    private readonly IContentDefinitionManager _contentDefinitionManager;

    public StrictSecurityPermissionAuthorizationHandler(IContentDefinitionManager contentDefinitionManager) =>
        _contentDefinitionManager = contentDefinitionManager;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if ((context.Resource as IContent)?.ContentItem is not { } contentItem ||
            !_permissionTemplates.TryGetValue(requirement.Permission.Name, out var claims) ||
            await _contentDefinitionManager.GetTypeDefinitionAsync(contentItem.ContentType) is not { } definition ||
            definition.GetSettings<StrictSecuritySettings>()?.Enabled != true)
        {
            return;
        }

        if (!context.User.Identity.IsAuthenticated)
        {
            context.Fail();
            return;
        }

        var contentType = contentItem.ContentType;
        claims = claims
            .Select(template => string.Format(CultureInfo.InvariantCulture, template, contentType))
            .ToList();
        var permissionNames = context
            .User
            .Claims
            .Where(claim => claim.Type == nameof(Permission))
            .Select(claim => claim.Value);

        if (!permissionNames.Any(claims.Contains)) context.Fail();
    }

    private static IList<string> GetPermissionTemplates(Permission permission, IList<string> templates)
    {
        templates.Add(permission.Name);

        if (permission.ImpliedBy is { } impliedBy)
        {
            foreach (var impliedPermission in impliedBy.Where(impliedPermission => impliedPermission.Name.Contains("{0}")))
            {
                GetPermissionTemplates(impliedPermission, templates);
            }
        }

        return templates;
    }
}
