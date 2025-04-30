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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            if (!ModelState.IsValid)
            {
                // If validation fails, re-populate available suppliers
                model = await _purchaseOrderModelFactory.PreparePurchaseOrderModelAsync(model);
                return View("~/Plugins/Nop.Plugin.Misc.PurchaseOrderManager/Areas/Admin/Views/PurchaseOrder/Create.cshtml", model);
            }

            // Verify at least one product is selected
            if (model.SelectedProductIds == null || !model.SelectedProductIds.Any())
            {
                _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.PurchaseOrders.SelectAtLeastOneProduct"));
                model = await _purchaseOrderModelFactory.PreparePurchaseOrderModelAsync(model);
                return View("~/Plugins/Nop.Plugin.Misc.PurchaseOrderManager/Areas/Admin/Views/PurchaseOrder/Create.cshtml", model);
            }

            try
            {
                // Handle saving the purchase order
                // This is where you would implement your purchase order creation logic
                // Example:
                // await _purchaseOrderService.CreatePurchaseOrderAsync(model);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.PurchaseOrders.CreatedSuccessfully"));

                // For AJAX requests, return JSON result
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }

                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                // Log the exception

                // For AJAX requests, return error
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new
                    {
                        success = false,
                        errorMessage = await _localizationService.GetResourceAsync("Admin.PurchaseOrders.Error")
                    });
                }

                _notificationService.ErrorNotification(ex.Message);
                model = await _purchaseOrderModelFactory.PreparePurchaseOrderModelAsync(model);
                return View("~/Plugins/Nop.Plugin.Misc.PurchaseOrderManager/Areas/Admin/Views/PurchaseOrder/Create.cshtml", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsBySupplier(int supplierId)
        {
            // Ensure that the supplier ID is valid
            if (supplierId == 0)
                return Json(new List<object>());

            // Get products for the selected supplier
            var products = await _supplierService.GetProductsBySupplierAsync(supplierId);

            // Map products to an enhanced model that includes additional fields needed for the DataTable
            var productList = products.Select(p => new
            {
                Value = p.Id.ToString(),     // Product ID
                Text = p.Name,               // Product Name
                Sku = p.Sku,                 // Product SKU
                Price = p.Price,             // Product Price
                StockQuantity = p.StockQuantity, // Current Stock
                Published = p.Published,     // Is Published
                MinimumStockQuantity = p.MinStockQuantity // Optional: Minimum stock level
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