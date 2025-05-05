using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Models
{
    public record class AddSupplierProductSearchModel : BaseSearchModel
    {
        public int SupplierId { get; set; }
        public string SearchProductName { get; set; }
    }
}

