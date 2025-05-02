using Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Models;
using System.Collections.Generic;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Models
{
    public class ProductSelectionModel
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }

        // Add a list of products
        public List<ProductModel> Products { get; set; } = new List<ProductModel>();
    }

    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
