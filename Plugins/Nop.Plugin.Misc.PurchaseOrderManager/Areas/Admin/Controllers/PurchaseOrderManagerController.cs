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
using System.Linq;

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
            if (Request.Query.ContainsKey("handler") && Request.Query["handler"] == "ProductSelectionPopup")
            {
                return await ProductSelectionPopup(int.Parse(Request.Query["supplierId"]));
            }

            var model = await _purchaseOrderModelFactory.PreparePurchaseOrderModelAsync(new PurchaseOrderModel());
            return View("~/Plugins/Nop.Plugin.Misc.PurchaseOrderManager/Areas/Admin/Views/PurchaseOrder/Create.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> ProductSelectionPopup(int supplierId)
        {
            if (supplierId <= 0)
                return BadRequest();

            var suppliers = await _supplierService.GetAllSuppliersAsync();
            var supplier = suppliers.FirstOrDefault(s => s.Id == supplierId);
            if (supplier == null)
                return NotFound();

            var model = new AddSupplierProductSearchModel
            {
                SupplierId = supplierId,
                AvailablePageSizes = "10,15,20,50"
            };

            model.SetGridPageSize();

            return View("~/Plugins/Nop.Plugin.Misc.PurchaseOrderManager/Areas/Admin/Views/PurchaseOrder/ProductSelectionPopup.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PurchaseOrderModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _purchaseOrderModelFactory.PreparePurchaseOrderModelAsync(model);
                return View("~/Plugins/Nop.Plugin.Misc.PurchaseOrderManager/Areas/Admin/Views/PurchaseOrder/Create.cshtml", model);
            }

            if (model.SelectedProductIds == null || !model.SelectedProductIds.Any())
            {
                _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.PurchaseOrders.SelectAtLeastOneProduct"));
                model = await _purchaseOrderModelFactory.PreparePurchaseOrderModelAsync(model);
                return View("~/Plugins/Nop.Plugin.Misc.PurchaseOrderManager/Areas/Admin/Views/PurchaseOrder/Create.cshtml", model);
            }

            try
            {
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.PurchaseOrders.CreatedSuccessfully"));

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }

                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
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
            if (supplierId == 0)
                return Json(new List<object>());

            var products = await _supplierService.GetProductsBySupplierAsync(supplierId);

            var productList = products.Select(p => new
            {
                Value = p.Id.ToString(),
                Text = p.Name,
                Sku = p.Sku,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                Published = p.Published,
                MinimumStockQuantity = p.MinStockQuantity
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

        [HttpPost]
        public async Task<IActionResult> SupplierProductAddPopupList(DataTablesParam parameters, int supplierId)
        {
            if (supplierId <= 0)
                return Json(new DataTablesResponse { Data = new List<PurchaseOrderProductModel>() });

            // Fetch all products for the supplier
            var products = await _supplierService.GetProductsBySupplierAsync(supplierId);

            // Apply search filter if exists
            var searchValue = parameters.Search?.Value;
            if (!string.IsNullOrEmpty(searchValue))
            {
                products = products.Where(p =>
                    p.Name.Contains(searchValue, System.StringComparison.InvariantCultureIgnoreCase) ||
                    p.Sku.Contains(searchValue, System.StringComparison.InvariantCultureIgnoreCase)
                ).ToList();
            }

            // Total count of all products (before filtering)
            var totalCount = products.Count(); // Directly using products.Count

            // Paged products for the current page
            var pagedProducts = products
                .Skip(parameters.Start)
                .Take(parameters.Length)
                .Select(p => new PurchaseOrderProductModel
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Sku = p.Sku,
                    Price = p.Price, // Ensure price is included
                    Published = p.Published
                })
                .ToList();

            // Return JSON response with DataTables format
            return Json(new DataTablesResponse
            {
                Draw = parameters.Draw,
                RecordsTotal = totalCount,  // Total records (before filtering)
                RecordsFiltered = products.Count(),  // Filtered records (after filtering)
                Data = pagedProducts
            });
        }





    }
}
