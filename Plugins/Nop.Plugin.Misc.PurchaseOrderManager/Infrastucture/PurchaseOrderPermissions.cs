using Nop.Core.Domain.Security;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Infrastucture;
public class PurchaseOrderPermissions
{
    public static readonly PermissionRecord ManagePurchaseOrders = new PermissionRecord
    {
        Name = "Manage Purchase Orders",
        SystemName = "ManagePurchaseOrders",
        Category = "Purchase Order Management"
    };
}
