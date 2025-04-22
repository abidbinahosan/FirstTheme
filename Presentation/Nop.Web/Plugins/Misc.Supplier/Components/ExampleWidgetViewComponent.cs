using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.Supplier.Components;
public class ExampleWidgetViewComponent : NopViewComponent
{
    public IViewComponentResult Invoke(string widgetZone, object additionalData)
    {
        return View("~/Plugins/Misc.Supplier/Views/Supplier/Default.cshtml");

    }
}