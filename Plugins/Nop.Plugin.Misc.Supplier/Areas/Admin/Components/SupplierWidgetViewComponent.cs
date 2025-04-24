using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Plugin.Misc.Supplier.Areas.Admin.Models;
using Nop.Plugin.Misc.Supplier.Areas.Admin.Services;

namespace Nop.Plugin.Misc.Supplier.Areas.Admin.Components;
public class SupplierWidgetViewComponent : NopViewComponent
{
    private readonly ISupplierService _supplierService;
    public SupplierWidgetViewComponent(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var productModel = additionalData as ProductModel;
        if (productModel == null || productModel.Id == 0)
            return View("~/Plugins/Misc.Supplier/Areas/Admin/Views/Components/Create.cshtml");

        var suppliers = await _supplierService.GetAllSuppliersAsync();
        var supplierId = await _supplierService.GetProductSupplierIdAsync(productModel.Id);

        var model = new SupplierProductModel
        {
            ProductId = productModel.Id,
            SelectedSupplierId = supplierId,
            SelectedSupplierName = suppliers?.FirstOrDefault(s => s.Id == supplierId)?.Name,
            Suppliers = suppliers
        };

        return View("~/Plugins/Misc.Supplier/Areas/Admin/Views/Components/Edit.cshtml", model);

    }
}