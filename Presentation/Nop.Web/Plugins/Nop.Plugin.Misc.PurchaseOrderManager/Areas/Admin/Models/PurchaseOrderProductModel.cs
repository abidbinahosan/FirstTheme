using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Models
{
    /// <summary>
    /// Represents a product in a purchase order
    /// </summary>
    public record PurchaseOrderProductModel : BaseNopEntityModel
    {
        /// <summary>
        /// Gets or sets the product ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product name
        /// </summary>
        [NopResourceDisplayName("Admin.PurchaseOrders.Products.Fields.Name")]
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the product SKU
        /// </summary>
        [NopResourceDisplayName("Admin.PurchaseOrders.Products.Fields.Sku")]
        public string Sku { get; set; }

        /// <summary>
        /// Gets or sets the product price
        /// </summary>
        [NopResourceDisplayName("Admin.PurchaseOrders.Products.Fields.Price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the current stock quantity
        /// </summary>
        [NopResourceDisplayName("Admin.PurchaseOrders.Products.Fields.StockQuantity")]
        public int StockQuantity { get; set; }

        /// <summary>
        /// Gets or sets the order quantity
        /// </summary>
        [NopResourceDisplayName("Admin.PurchaseOrders.Products.Fields.OrderQuantity")]
        public int OrderQuantity { get; set; }

        /// <summary>
        /// Gets or sets the product subtotal
        /// </summary>
        [NopResourceDisplayName("Admin.PurchaseOrders.Products.Fields.Subtotal")]
        public decimal Subtotal { get; set; }
        public bool Published { get; set; } // <-- Add this
    }
}