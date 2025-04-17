using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.Supplier.Domain;
using Nop.Plugin.Misc.Supplier.Model;
using Nop.Plugin.Misc.Supplier.Services;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using System.Threading.Tasks;
using Nop.Plugin.Misc.Supplier.Factories;

namespace Nop.Plugin.Misc.Supplier.Controllers
{
    [AuthorizeAdmin]
    [Area("admin")]
    public class SupplierController : BasePluginController
    {
        private readonly ISupplierService _supplierService;
        private readonly IPermissionService _permissionService;
        private readonly ISupplierModelFactory _supplierModelFactory;

        public SupplierController(
            ISupplierService supplierService,
            IPermissionService permissionService,
            ISupplierModelFactory supplierModelFactory)
        {
            _supplierService = supplierService;
            _permissionService = permissionService;
            _supplierModelFactory = supplierModelFactory;
        }

        // GET: Supplier Index Page
        public async Task<IActionResult> Index()
        {
            var model = new SupplierSearchModel();
            model.SetGridPageSize();
            return View("~/Plugins/Misc.Supplier/Views/Supplier/Index.cshtml", model);
        }

        // POST: List API for DataTables
        [HttpPost]
        public async Task<IActionResult> List(SupplierSearchModel searchModel)
        {
            var model = await _supplierModelFactory.PrepareSupplierListModelAsync(searchModel);
            return Json(model);
        }

        // GET: Create View
        public IActionResult Create()
        {
            return View("~/Plugins/Misc.Supplier/Views/Supplier/Create.cshtml");
        }

        // POST: Create Action
        [HttpPost]
        public async Task<IActionResult> Create(SupplierEntity supplier)
        {
            if (ModelState.IsValid)
            {
                await _supplierService.InsertAsync(supplier);
                return RedirectToAction("Index");
            }

            return View("~/Plugins/Misc.Supplier/Views/Supplier/Create.cshtml", supplier);
        }

        // GET: Edit View
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            return View("~/Plugins/Misc.Supplier/Views/Supplier/Edit.cshtml", supplier);
        }

        // POST: Edit Action
        [HttpPost]
        public async Task<IActionResult> Edit(SupplierEntity supplier)
        {
            if (ModelState.IsValid)
            {
                await _supplierService.UpdateAsync(supplier);
                return RedirectToAction("Index");
            }

            return View("~/Plugins/Misc.Supplier/Views/Supplier/Edit.cshtml", supplier);
        }

        // POST: Delete Supplier
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier != null)
                await _supplierService.DeleteAsync(supplier);

            return RedirectToAction("Index");
        }
    }
}
