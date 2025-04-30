using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Factories;
using Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Models;
using Nop.Plugin.Misc.Supplier.Areas.Admin.Services;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    [Area("admin")]
    public class PurchaseOrderManagerController : BasePluginController
    {
        private readonly IPurchaseOrderModelFactory _purchaseOrderModelFactory;
        private readonly IPermissionService _permissionService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        private readonly ISupplierService _supplierService;

        public PurchaseOrderManagerController(
            IPurchaseOrderModelFactory purchaseOrderModelFactory,
            IPermissionService permissionService,
            INotificationService notificationService,
            ILocalizationService localizationService,
            ISupplierService supplierService)
        {
            _purchaseOrderModelFactory = purchaseOrderModelFactory;
            _permissionService = permissionService;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _supplierService = supplierService;
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

        public async Task<IActionResult> Create()
        {
            var model = await _purchaseOrderModelFactory.PreparePurchaseOrderModelAsync(new PurchaseOrderModel());
            return View("~/Plugins/Nop.Plugin.Misc.PurchaseOrderManager/Areas/Admin/Views/PurchaseOrder/Create.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PurchaseOrderModel model)
        {
            if (ModelState.IsValid)
            {
                // Handle saving the purchase order (you should add actual saving logic here)
                // E.g., create purchase order in the database, send notifications, etc.

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.PurchaseOrders.CreatedSuccessfully"));
                return RedirectToAction("Index");
            }

            // If validation fails, re-populate available suppliers and return the model
           
            return View("~/Plugins/Nop.Plugin.Misc.PurchaseOrderManager/Areas/Admin/Views/PurchaseOrder/Create.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsBySupplier(int supplierId)
        {
            // Ensure that the supplier ID is valid
            if (supplierId == 0)
                return Json(new List<SelectListItem>());

            // Get products for the selected supplier
            var products = await _supplierService.GetProductsBySupplierAsync(supplierId); // Make sure to create this method in your service

            // Prepare the product list for the dropdown
            var productList = products.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).ToList();

            return Json(productList);
        }

        public async Task<IActionResult> GetAvailableSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            var items = suppliers.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToList();

            return Json(items);
        }
    }
}
