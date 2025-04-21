using System.Threading.Tasks;
using Nop.Plugin.Misc.Supplier.Model;
using Nop.Plugin.Misc.Supplier.Domain;

namespace Nop.Plugin.Misc.Supplier.Factories
{
    public interface ISupplierModelFactory
    {
        public SupplierEntity PrepareEntity(SupplierModel model);
        public SupplierModel PrepareModel(SupplierEntity entity);
        Task<SupplierListModel> PrepareSupplierListModelAsync(SupplierSearchModel searchModel);
    }
}