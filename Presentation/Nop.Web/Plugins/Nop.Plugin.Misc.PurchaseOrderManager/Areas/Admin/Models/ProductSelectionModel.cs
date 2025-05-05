namespace Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Models
{
    public class ProductSelectionModel
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public List<ProductModel> Products { get; set; } = new List<ProductModel>();
    }

    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
