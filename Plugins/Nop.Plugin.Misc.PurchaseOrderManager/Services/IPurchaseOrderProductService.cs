using Nop.Plugin.Misc.PurchaseOrderManager.Domain;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Services
{
    public interface IPurchaseOrderProductService
    {
        Task<IList<PurchaseOrderProduct>> GetProductsByPurchaseOrderIdAsync(int purchaseOrderId);
    }
}
