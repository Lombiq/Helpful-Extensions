#nullable enable

using Fluid;
using Fluid.Ast;
using Lombiq.HelpfulLibraries.OrchardCore.Liquid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OrchardCore.ContentManagement;
using OrchardCore.Security.Permissions;
using OrchardCore.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Liquid.Services;

public class IfAuthorizedParserBlock : ILiquidParserBlock
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IContentManager _contentManager;
    private readonly IHttpContextAccessor _hca;
    private readonly IEnumerable<IPermissionProvider> _permissionProviders;
    private readonly UserManager<IUser> _userManager;

    public IfAuthorizedParserBlock(
        IAuthorizationService authorizationService,
        IContentManager contentManager,
        IHttpContextAccessor hca,
        IEnumerable<IPermissionProvider> permissionProviders,
        UserManager<IUser> userManager)
    {
        _authorizationService = authorizationService;
        _contentManager = contentManager;
        _hca = hca;
        _permissionProviders = permissionProviders;
        _userManager = userManager;
    }

    public async ValueTask<Completion> WriteToAsync(
        IReadOnlyList<FilterArgument> argumentsList,
        IReadOnlyList<Statement> statements,
        TextWriter writer,
        TextEncoder encoder,
        TemplateContext context)
    {
        FilterArgument? GetArgument(string name) =>
            argumentsList.FirstOrDefault(argument => name.EqualsOrdinalIgnoreCase(argument.Name));

        async Task<string?> EvaluateAsync(FilterArgument? argument) =>
            argument?.Expression is { } expression
                ? (await expression.EvaluateAsync(context)).ToStringValue()
                : null;

        var permission = await _permissionProviders.GetPermissionAsync(
            await EvaluateAsync(GetArgument("permission")),
            _hca.GetCancellation());

        if (permission == null) return Completion.Normal;

        var expected = GetArgument("invert") is not { } invertArgument ||
            !(await invertArgument.Expression.EvaluateAsync(context)).ToBooleanValue();

        var user = _hca.HttpContext?.User;

        if (await EvaluateAsync(GetArgument("user")) is { } userName)
        {
            var claims = await _userManager.FindByNameAsync(userName) is { } foundUser
                ? await _userManager.GetClaimsAsync(foundUser)
                : [];
            user = new(new ClaimsIdentity(claims));
        }

        if (await EvaluateAsync(GetArgument("email")) is { } email)
        {
            var claims = await _userManager.FindByEmailAsync(email) is { } foundUser
                ? await _userManager.GetClaimsAsync(foundUser)
                : [];
            user = new(new ClaimsIdentity(claims));
        }

        var resource = await EvaluateAsync(GetArgument("contentItem")) is { } contentItemId
            ? await _contentManager.GetAsync(contentItemId)
            : (object?)null;

        if (await _authorizationService.AuthorizeAsync(user, permission, resource) != expected) return Completion.Normal;

        foreach (var statement in statements)
        {
            var completion = await statement.WriteToAsync(writer, encoder, context);

            if (completion != Completion.Normal) return completion;
        }

        return Completion.Normal;
    }
}
