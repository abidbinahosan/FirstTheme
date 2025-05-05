using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Models
{
    public record PurchaseOrderModel : BaseNopEntityModel
    {
        public PurchaseOrderModel()
        {
            AvailableSuppliers = new List<SelectListItem>();
            SelectedProductIds = new List<int>();
            Quantities = new Dictionary<int, int>();
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

        public IList<SelectListItem> AvailableSuppliers { get; set; }

        /// <summary>
        /// Gets or sets the selected product IDs for the purchase order
        /// </summary>
        public IList<int> SelectedProductIds { get; set; }

        /// <summary>
        /// Gets or sets the product quantities (product ID -> quantity)
        /// </summary>
        public IDictionary<int, int> Quantities { get; set; }
    }
}