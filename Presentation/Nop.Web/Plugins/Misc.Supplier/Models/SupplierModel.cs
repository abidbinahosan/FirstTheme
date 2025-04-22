using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Text.RegularExpressions;

namespace Nop.Plugin.Misc.Supplier.Model;
// Change SupplierModel to a record to match the inheritance requirement of BaseNopModel  
public record SupplierModel : BaseNopEntityModel, ILocalizedModel<SupplierLocalizedModel>
{
    [NopResourceDisplayName("Admin.Suppliers.Fields.Name")]
    public string Name { get; set; }
    [NopResourceDisplayName("Admin.Suppliers.Fields.ContactPerson")]
    public string ContactPerson { get; set; }
    [NopResourceDisplayName("Admin.Suppliers.Fields.Phone")]
    public string Phone { get; set; }
    [NopResourceDisplayName("Admin.Suppliers.Fields.Email")]
    public string Email { get; set; }
    [NopResourceDisplayName("Admin.Suppliers.Fields.Address")]
    public string Address { get; set; }
    [NopResourceDisplayName("Admin.Suppliers.Fields.Description")]
    public string Description { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.IsActive")]
    public bool IsActive { get; set; }
    public IList<SupplierLocalizedModel> Locales { get; set; }

    public SupplierModel()
    {
        Locales = new List<SupplierLocalizedModel>();
    }
    // ✅ Computed property to clean HTML from Description
    public string CleanDescription
    {
        get
        {
            if (string.IsNullOrEmpty(Description))
                return string.Empty;

            // Step 1: Remove HTML tags
            var noHtml = Regex.Replace(Description, "<.*?>", string.Empty);

            // Step 2: Remove non-breaking spaces (&nbsp;)
            var cleanText = noHtml.Replace("&nbsp;", " ").Trim();

            return cleanText;
        }
    }
}