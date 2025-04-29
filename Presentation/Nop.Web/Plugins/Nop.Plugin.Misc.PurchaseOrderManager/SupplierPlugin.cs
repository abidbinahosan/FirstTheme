using Nop.Services.Events;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;
using Nop.Services.Localization;
using Nop.Data;
using Nop.Services.Cms;
using Nop.Web.Framework.Infrastructure;
using Nop.Plugin.Misc.Supplier.Areas.Admin.Components;
using Nop.Plugin.Misc.Supplier.Areas.Admin.Infrastructure;

namespace Nop.Plugin.Misc.Supplier;
public class SupplierPlugin : BasePlugin, IWidgetPlugin
{
    private readonly IPermissionService _permissionService;
    private readonly ILocalizationService _localizationService;
    private readonly INopDataProvider _dataProvider;
    public SupplierPlugin(IPermissionService permissionService, ILocalizationService localizationService, INopDataProvider dataProvider)
    {
        _permissionService = permissionService;
        _localizationService = localizationService;
        _dataProvider = dataProvider;
    }
    public override async Task InstallAsync()
    {
        var resources = SupplierLocaleResources.GetAll();
        await _localizationService.AddOrUpdateLocaleResourceAsync(resources);
        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        try
        {
            var resourceKeys = SupplierLocaleResources.GetAll().Keys.ToArray();
            await _localizationService.DeleteLocaleResourcesAsync(resourceKeys);
            await _dataProvider.ExecuteNonQueryAsync("DROP TABLE IF EXISTS [Supplier]");
            await base.UninstallAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public override async Task UpdateAsync(string currentVersion, string targetVersion)
    {
        var current = Version.Parse(currentVersion);
        var target = Version.Parse(targetVersion);

        if (current < target)
        {
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Admin.Suppliers.Fields.ContactPerson.Required"] = "Contact Person is Required!"
            });
        }
    }

    //Widget
    /// <summary>
    /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
    /// </summary>
    public bool HideInWidgetList => false;

    /// <summary>
    /// Gets a type of a view component for displaying widget
    /// </summary>

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(SupplierWidgetViewComponent);
    }

    /// <summary>
    /// Gets widget zones where this widget should be rendered
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the widget zones
    /// </returns>
    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string>
         {
             AdminWidgetZones.ProductDetailsBlock

         });

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
                    SystemName = "Misc.Supplier", 
                    Title = "Supplier", 
                    Url = eventMessage.GetMenuItemUrl("Supplier", "Index"), 
                    IconClass = "far fa-dot-circle", 
                    Visible = true, 
                });
        }
    }
}