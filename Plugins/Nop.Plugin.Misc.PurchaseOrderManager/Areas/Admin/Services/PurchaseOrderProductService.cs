using Nop.Data;
using Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Services
{
    public class PurchaseOrderProductService : IPurchaseOrderProductService
    {
        private readonly IRepository<PurchaseOrderProduct> _purchaseOrderProductRepository;

        public PurchaseOrderProductService(IRepository<PurchaseOrderProduct> purchaseOrderProductRepository)
        {
            _purchaseOrderProductRepository = purchaseOrderProductRepository;
        }

        public async Task<IList<PurchaseOrderProduct>> GetProductsByPurchaseOrderIdAsync(int purchaseOrderId)
        {
            var products = await _purchaseOrderProductRepository.Table
                .Where(p => p.PurchaseOrderId == purchaseOrderId)
                .ToListAsync();

            return products;
        }
    }
}
