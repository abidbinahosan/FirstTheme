using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Factories;
using Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Models;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Controllers;
[AuthorizeAdmin]
[Area("admin")]
public class PurchaseOrderManagerController : BasePluginController
{
    private readonly IPurchaseOrderModelFactory _purchaseOrderModelFactory;
    private readonly IPermissionService _permissionService;
    private readonly INotificationService _notificationService;
    private readonly ILocalizationService _localizationService;

    public PurchaseOrderManagerController(
        IPurchaseOrderModelFactory purchaseOrderModelFactory,
        IPermissionService permissionService,
        INotificationService notificationService,
        ILocalizationService localizationService)
    {
        _purchaseOrderModelFactory = purchaseOrderModelFactory;
        _permissionService = permissionService;
        _notificationService = notificationService;
        _localizationService = localizationService;
    }

    public async Task<IActionResult> Index()
    {
        var model = await _purchaseOrderModelFactory.PreparePurchaseOrderSearchModelAsync(new PurchaseOrderSearchModel());
        return View("~/Plugins/Nop.Plugin.Misc.PurchaseOrderManager/Areas/Admin/Views/PurchaseOrder/Index.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> List(PurchaseOrderSearchModel searchModel)
    {
        var model = await _purchaseOrderModelFactory.PreparePurchaseOrderListModelAsync(searchModel);
        return Json(model);
    }
}
