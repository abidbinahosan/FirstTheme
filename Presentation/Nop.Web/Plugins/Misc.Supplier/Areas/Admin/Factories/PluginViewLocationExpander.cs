using Microsoft.AspNetCore.Mvc.Razor;

namespace Nop.Plugin.Misc.Supplier.Areas.Admin.Factories;
public class PluginViewLocationExpander : IViewLocationExpander
{
    public void PopulateValues(ViewLocationExpanderContext context)
    {
    }

    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        var pluginViewLocations = new[]
        {
             "/Plugins/Misc.Supplier/Views/{1}/{0}.cshtml",
             "/Plugins/Misc.Supplier/Views/Shared/{0}.cshtml"
         };

        return pluginViewLocations.Concat(viewLocations);
    }
}
