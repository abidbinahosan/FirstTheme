using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Services.Events;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.PurchaseOrderManager
{
    public class PurchaseOrderManagerPlugin : BasePlugin
    {
        public override async Task InstallAsync()
        {
            //Logic during installation goes here... 

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            //Logic during uninstallation goes here... 

            await base.UninstallAsync();
        }
        public class EventConsumer : IConsumer<AdminMenuCreatedEvent>
        {
            private readonly IPermissionService _permissionService;

            public EventConsumer(IPermissionService permissionService)
            {
                _permissionService = permissionService;
            }

            public async Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
            {
                if (!await _permissionService.AuthorizeAsync(StandardPermission.Configuration.MANAGE_PLUGINS))
                    return;

                eventMessage.RootMenuItem.InsertAfter("Help",
                    new AdminMenuItem
                    {
                        SystemName = "Misc.PurchaseOrderManager",
                        Title = "Purchase Order Manager",
                        Url = eventMessage.GetMenuItemUrl("PurchaseOrderManager", "Index"),
                        IconClass = "far fa-dot-circle",
                        Visible = true,
                    });
            }
        }
    }
}
