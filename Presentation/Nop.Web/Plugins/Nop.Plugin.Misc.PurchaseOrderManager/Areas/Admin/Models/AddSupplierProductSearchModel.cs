using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Models
{
    public record class AddSupplierProductSearchModel : BaseSearchModel
    {
        public int SupplierId { get; set; }
        public string SearchProductName { get; set; }
    }
}

