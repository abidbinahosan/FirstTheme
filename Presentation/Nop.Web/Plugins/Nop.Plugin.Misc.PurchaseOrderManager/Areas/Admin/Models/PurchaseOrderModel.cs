using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Models
{
    public record PurchaseOrderModel : BaseNopEntityModel
    {
        public PurchaseOrderModel()
        {
            AvailableSuppliers = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.PurchaseOrders.Fields.Supplier")]
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }

        [NopResourceDisplayName("Admin.PurchaseOrders.Fields.CreatedOnUtc")]
        public DateTime CreatedOnUtc { get; set; }

        [NopResourceDisplayName("Admin.PurchaseOrders.Fields.TotalAmount")]
        public decimal TotalAmount { get; set; }

        [NopResourceDisplayName("Admin.PurchaseOrders.Fields.CreatedBy")]
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }

        public IList<SelectListItem> AvailableSuppliers { get; set; } = new List<SelectListItem>();
    }
}
