namespace Nop.Plugin.Misc.Supplier.Areas.Admin.Models;
public record SupplierProductModel
{
    public int ProductId { get; set; }
    public int SelectedSupplierId { get; set; }
    public string SelectedSupplierName { get; set; }
    public IList<Domain.SupplierEntity> Suppliers { get; set; }
}