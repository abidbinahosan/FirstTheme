﻿using Nop.Core;
using Nop.Plugin.Misc.Supplier.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.Supplier.Areas.Admin.Services
{
    public interface ISupplierService
    {
        Task InsertAsync(SupplierEntity supplier);
        Task UpdateAsync(SupplierEntity supplier);
        Task DeleteAsync(SupplierEntity supplier);
        Task<SupplierEntity> GetByIdAsync(int id);
        Task<IList<SupplierEntity>> GetAllSuppliersAsync();
        Task<IPagedList<SupplierEntity>> GetAllAsync(string searchName, string searchEmail, int pageIndex, int pageSize);
        Task InsertOrUpdateProductSupplierMappingAsync(int productId, int supplierId);
        Task<int> GetProductSupplierIdAsync(int productId);
    }
}