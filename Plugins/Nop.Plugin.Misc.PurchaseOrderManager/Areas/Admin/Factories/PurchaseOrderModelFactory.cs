using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Caching;
using Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Models;
using Nop.Plugin.Misc.Supplier.Areas.Admin.Services;
using Nop.Services.Customers;
using Nop.Web.Framework.Models.Extensions;
using Nop.Plugin.Misc.PurchaseOrderManager.Services;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Factories
{
    public class PurchaseOrderModelFactory : IPurchaseOrderModelFactory
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly ISupplierService _supplierService;
        private readonly ICustomerService _customerService;
        private readonly IStaticCacheManager _staticCacheManager;

        public PurchaseOrderModelFactory(
            IPurchaseOrderService purchaseOrderService,
            ISupplierService supplierService,
            ICustomerService customerService,
            IStaticCacheManager staticCacheManager)
        {
            _purchaseOrderService = purchaseOrderService;
            _supplierService = supplierService;
            _customerService = customerService;
            _staticCacheManager = staticCacheManager;
        }
        public async Task<PurchaseOrderSearchModel> PreparePurchaseOrderSearchModelAsync(PurchaseOrderSearchModel searchModel)
        {
            if (searchModel == null)
                searchModel = new PurchaseOrderSearchModel();

            var suppliers = await _supplierService.GetAllSuppliersAsync();
            foreach (var supplier in suppliers)
            {
                searchModel.AvailableSuppliers.Add(new SelectListItem
                {
                    Text = supplier.Name,
                    Value = supplier.Id.ToString()
                });
            }

            searchModel.SetGridPageSize();
            return searchModel;
        }

        public async Task<PurchaseOrderListModel> PreparePurchaseOrderListModelAsync(PurchaseOrderSearchModel searchModel)
        {
            var purchaseOrders = await _purchaseOrderService.SearchPurchaseOrdersAsync(
                supplierId: searchModel.SupplierId,
                startDate: searchModel.StartDate,
                endDate: searchModel.EndDate,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            var model = await new PurchaseOrderListModel().PrepareToGridAsync(searchModel, purchaseOrders, () =>
            {
                return purchaseOrders.SelectAwait(async po =>
                {
                    var supplier = await _supplierService.GetByIdAsync(po.SupplierId);
                    var createdBy = await _customerService.GetCustomerByIdAsync(po.CreatedById);

                    return new PurchaseOrderModel
                    {
                        Id = po.Id,
                        SupplierId = po.SupplierId,
                        SupplierName = supplier?.Name ?? "N/A",
                        CreatedOnUtc = po.CreatedOnUtc,
                        CreatedBy = createdBy?.Email ?? "System",
                        TotalAmount = po.TotalAmount
                    };
                });
            });

            return model;
        }

        public async Task<PurchaseOrderModel> PreparePurchaseOrderModelAsync(PurchaseOrderModel model )
        {
            if(model == null ) model = new PurchaseOrderModel();

            var suppliers = await _supplierService.GetAllSuppliersAsync();
            model.AvailableSuppliers = suppliers.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToList();

            return model;
        }
    }
}
