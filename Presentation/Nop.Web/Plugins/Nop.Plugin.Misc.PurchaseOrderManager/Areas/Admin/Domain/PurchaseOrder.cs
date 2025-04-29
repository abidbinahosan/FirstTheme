using Nop.Core;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Domain;
public class PurchaseOrder : BaseEntity
{
    public int SupplierId { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public decimal TotalAmount { get; set; }
    public int CreatedById { get; set; }
}