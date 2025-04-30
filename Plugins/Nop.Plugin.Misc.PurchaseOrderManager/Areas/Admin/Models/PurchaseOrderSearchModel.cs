using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.PurchaseOrderManager.Areas.Admin.Models
{
    public record PurchaseOrderSearchModel : BaseSearchModel
    {
        public PurchaseOrderSearchModel()
        {
            AvailableSuppliers = new List<SelectListItem>(){
                 new SelectListItem { Text = "All", Value = "0" }
             };
        }

        [NopResourceDisplayName("Admin.PurchaseOrders.List.SearchSupplier")]
        public int SupplierId { get; set; }

        [NopResourceDisplayName("Admin.PurchaseOrders.List.SearchStartDate")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [NopResourceDisplayName("Admin.PurchaseOrders.List.SearchEndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the list of available suppliers
        /// </summary>
        public IList<SelectListItem> AvailableSuppliers { get; set; }
    }
}