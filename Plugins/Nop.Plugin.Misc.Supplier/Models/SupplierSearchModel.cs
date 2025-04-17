using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.Supplier.Model;
// Change the class to a record to fix CS8865
public record SupplierSearchModel : BaseSearchModel
{
    public string SearchName { get; set; }
    public string SearchEmail { get; set; }
    public bool Active { get; set; }
    public IList<SupplierModel> Suppliers { get; set; } // This will hold the list of suppliers
    public int PageIndex { get; set; } // Current page number
    public int TotalPages { get; set; } // Total number of pages
    public bool HasPreviousPage { get; set; } // Determines if there is a previous page
    public bool HasNextPage { get; set; } // Determines if there is a next page
}