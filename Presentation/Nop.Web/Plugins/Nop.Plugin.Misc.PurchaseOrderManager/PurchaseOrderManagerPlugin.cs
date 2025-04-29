using Nop.Plugin.Misc.PurchaseOrderManager.Utility;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.PurchaseOrderManager
{
    public class PurchaseOrderManagerPlugin : BasePlugin
    {
        private readonly ILocalizationService _localizationService;

        public PurchaseOrderManagerPlugin(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }
        public override async Task InstallAsync()
        {
            var resources = PurchaseOrderLocaleResources.GetAll();

            await _localizationService.AddOrUpdateLocaleResourceAsync(resources);

            await base.InstallAsync();
        }
        public override async Task UninstallAsync()
        {
            var resourceKeys = PurchaseOrderLocaleResources.GetAll().Keys.ToArray();

            await _localizationService.DeleteLocaleResourcesAsync(resourceKeys);

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
