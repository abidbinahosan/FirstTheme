using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;

namespace Nop.Plugin.Misc.Supplier.Infrastructure
{
    public class SupplierViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context) { }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return new[]
            {
                "/Plugins/Nop.Plugin.Misc.Supplier/Views/{1}/{0}.cshtml", // Controller/View.cshtml
                "/Plugins/Nop.Plugin.Misc.Supplier/Views/Shared/{0}.cshtml"
            }.Concat(viewLocations);
        }
    }
}
