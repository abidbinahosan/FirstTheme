using Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Services
{
    public interface IPurchaseOrderProductService
    {
        Task<IList<PurchaseOrderProduct>> GetProductsByPurchaseOrderIdAsync(int purchaseOrderId);
    }
}
