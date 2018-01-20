using Orchard.Commands;
using Orchard.Environment.Extensions;
using Orchard.Roles.Services;
using Orchard.Security;
using System.Collections.Generic;

namespace Piedone.HelpfulExtensions.Commands
{
    [OrchardFeature(Constants.FeatureNames.CodeGeneration)]
    public class PermissionCommands : DefaultOrchardCommandHandler
    {
        private readonly IRoleService _roleService;


        public PermissionCommands(IRoleService roleService)
        {
            _roleService = roleService;
        }



        [CommandName("lock frontend")]
        [CommandHelp("lock frontend\r\n\t" + "Locks access to the frontend of the site, i.e. only authenticated users will be able to access it.")]
        public void LockFrontend()
        {
            if (SetAccess(false))
            {
                Context.Output.WriteLine(T("Unauthenticated access to the frontend is now disabled. Remember to restart the app for the change to take effect if you're running the command from the command line.")); 
            }
        }

        [CommandName("unlock frontend")]
        [CommandHelp("unlock frontend\r\n\t" + "Anonymous users will be able to access the site's frontend too.")]
        public void UnlockFrontend()
        {
            if (SetAccess(true))
            {
                Context.Output.WriteLine(T("Anonymous access to the frontend is now enabled. Remember to restart the app for the change to take effect if you're running the command from the command line.")); 
            }
        }


        private bool SetAccess(bool allowFrontend)
        {
            var anonymousRole = _roleService.GetRoleByName("Anonymous");

            if (anonymousRole == null)
            {
                Context.Output.WriteLine(T("There is no Anonymous role."));
                return false;
            }

            var permissions = new HashSet<string>(_roleService.GetPermissionsForRole(anonymousRole.Id));

            if (allowFrontend) permissions.Add(StandardPermissions.AccessFrontEnd.Name);
            else permissions.Remove(StandardPermissions.AccessFrontEnd.Name);

            _roleService.UpdateRole(anonymousRole.Id, anonymousRole.Name, permissions);

            return true;
        }
    }
}