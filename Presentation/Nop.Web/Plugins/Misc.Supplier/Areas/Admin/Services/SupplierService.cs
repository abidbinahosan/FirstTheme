using Nop.Core;
using Nop.Data;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog; // For accessing the Product class
using Nop.Plugin.Misc.Supplier.Areas.Admin.Domain;
using Microsoft.EntityFrameworkCore; // Ensure this is included for EF queries

namespace Nop.Plugin.Misc.Supplier.Areas.Admin.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IRepository<SupplierEntity> _repository;
        private readonly IRepository<ProductSupplierMapping> _productSupplierMappingRepository;
        private readonly IRepository<Product> _productRepository;  // Add the product repository
        private readonly IStaticCacheManager _staticCacheManager;

        // Constructor to inject the product repository along with the other dependencies
        public SupplierService(IRepository<SupplierEntity> repository,
                               IRepository<ProductSupplierMapping> productSupplierMappingRepository,
                               IRepository<Product> productRepository, // Inject the Product repository
                               IStaticCacheManager staticCacheManager)
        {
            _repository = repository;
            _productSupplierMappingRepository = productSupplierMappingRepository;
            _productRepository = productRepository;  // Initialize the product repository
            _staticCacheManager = staticCacheManager;
        }

        public async Task InsertAsync(SupplierEntity supplier)
        {
            await _repository.InsertAsync(supplier);
            await _staticCacheManager.RemoveByPrefixAsync(SupplierDefaults.AdminSupplierAllPrefixCacheKey);
        }

        public async Task UpdateAsync(SupplierEntity supplier)
        {
            await _repository.UpdateAsync(supplier);
            await _staticCacheManager.RemoveByPrefixAsync(SupplierDefaults.AdminSupplierAllPrefixCacheKey);
        }

        public async Task DeleteAsync(SupplierEntity supplier)
        {
            await _repository.DeleteAsync(supplier);
            await _staticCacheManager.RemoveByPrefixAsync(SupplierDefaults.AdminSupplierAllPrefixCacheKey);
        }

        public async Task<SupplierEntity> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<IList<SupplierEntity>> GetAllSuppliersAsync()
        {
            return await _repository.Table.ToListAsync();
        }

        public async Task<IPagedList<SupplierEntity>> GetAllAsync(string name, string email, int pageIndex, int pageSize)
        {
            name = string.IsNullOrWhiteSpace(name) ? null : name.Trim();
            email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();

            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(SupplierDefaults.AdminSupplierAllModelKey,
                             name, email, pageIndex, pageSize);

            var allSuppliers = await _staticCacheManager.GetAsync<IList<SupplierEntity>>(cacheKey, async () =>
            {
                var query = _repository.Table;

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(s => s.Name.Contains(name));
                if (!string.IsNullOrEmpty(email))
                    query = query.Where(s => s.Email.Contains(email));

                return await query.OrderBy(s => s.Name).ToListAsync();
            });

            return new PagedList<SupplierEntity>(allSuppliers, pageIndex, pageSize);
        }

        public async Task InsertOrUpdateProductSupplierMappingAsync(int productId, int supplierId)
        {
            var existing = await _productSupplierMappingRepository.Table
                .FirstOrDefaultAsync(x => x.ProductId == productId);

            if (existing != null)
            {
                existing.SupplierId = supplierId;
                await _productSupplierMappingRepository.UpdateAsync(existing);
            }
            else
            {
                var newMapping = new ProductSupplierMapping
                {
                    ProductId = productId,
                    SupplierId = supplierId
                };
                await _productSupplierMappingRepository.InsertAsync(newMapping);
            }
        }

        public async Task<int> GetProductSupplierIdAsync(int productId)
        {
            var existing = await _productSupplierMappingRepository.Table
                .FirstOrDefaultAsync(x => x.ProductId == productId);

            if (existing != null)
            {
                return existing.SupplierId;
            }
            else
                return 0;
        }

        public async Task<IList<Product>> GetProductsBySupplierAsync(int supplierId)
        {
            var productIds = await _productSupplierMappingRepository.Table
                .Where(mapping => mapping.SupplierId == supplierId)
                .Select(mapping => mapping.ProductId)
                .ToListAsync();

            var products = await _productRepository.Table
                .Where(product => productIds.Contains(product.Id))
                .ToListAsync();

            return products;
        }
    }
}
