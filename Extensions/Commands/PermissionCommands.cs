using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Commands;
using Orchard.Environment.Extensions;
using Orchard.Roles.Services;
using Orchard.Security;

namespace Piedone.HelpfulExtensions.Commands
{
    [OrchardFeature("Piedone.HelpfulExtensions.Commands")]
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
                Context.Output.WriteLine(T("Unauthenticated access to the frontend is now disabled.")); 
            }
        }

        [CommandName("unlock frontend")]
        [CommandHelp("unlock frontend\r\n\t" + "Anonymous users will be able to access the site's frontend too.")]
        public void UnlockFrontend()
        {
            if (SetAccess(true))
            {
                Context.Output.WriteLine(T("Anonymous access to the frontend is now enabled.")); 
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