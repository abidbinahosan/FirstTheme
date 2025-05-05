using Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrderManager.Domain;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Factories
{
    public interface IPurchaseOrderModelFactory
    {
        Task<PurchaseOrderSearchModel> PreparePurchaseOrderSearchModelAsync(PurchaseOrderSearchModel searchModel);
        Task<PurchaseOrderListModel> PreparePurchaseOrderListModelAsync(PurchaseOrderSearchModel searchModel);
        Task<PurchaseOrderModel> PreparePurchaseOrderModelAsync(PurchaseOrderModel model);
    }
}
