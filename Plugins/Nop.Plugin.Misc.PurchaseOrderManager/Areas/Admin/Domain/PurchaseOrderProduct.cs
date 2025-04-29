using Nop.Core;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Domain;
public class PurchaseOrderProduct : BaseEntity
{
    public int PurchaseOrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal LineTotal { get; set; }
}