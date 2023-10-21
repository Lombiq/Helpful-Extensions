using Microsoft.Extensions.Logging;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Lombiq.HelpfulLibraries.OrchardCore.Users.PasswordHelper;

namespace Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Services;

public class UserOrchardContentConverter : IOrchardUserConverter
{
    private readonly IUserService _userService;
    private readonly ILogger<UserOrchardContentConverter> _logger;

    public UserOrchardContentConverter(
        IUserService userService,
        ILogger<UserOrchardContentConverter> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public bool IgnoreDefaultConverter => false;

    public async Task ImportAsync(XElement element)
    {
        if (element.Element("UserPart") is not { } userPart)
            return;

        var roles = element.Element("UserRolesPart").Attribute("Roles")?.Value;
        var rolesList = string.IsNullOrEmpty(roles) ? new List<string>() : roles.Split(',').ToList();

        await _userService.CreateUserAsync(
            new User
            {
                UserName = userPart.Attribute("UserName")?.Value,
                Email = userPart.Attribute("Email")?.Value,
                EmailConfirmed = true,
                IsEnabled = true,
                RoleNames = rolesList,
            },
            GenerateRandomPassword(32),
            (_, message) => _logger.LogError("User creation failed: \"{Message}\"", message));

        return;
    }
}
