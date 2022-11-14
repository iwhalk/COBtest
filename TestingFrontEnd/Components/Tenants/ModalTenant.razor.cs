using Microsoft.AspNetCore.Components;
using Shared.Models;
using SharedLibrary.Models;

namespace FrontEnd.Components.Tenants
{
    public partial class ModalTenant : ComponentBase
    {
        [Parameter]
        public bool ShowModal { get; set; }
        [Parameter]
        public List<Tenant> Tenants { get; set; }
        [Parameter]
        public EventCallback CloseModalTenant { get; set; }
        [Parameter]
        public EventCallback<int> SendIdTenant { get; set; }

        public int IdTenant { get; set; }
        public bool DisableCheckBox { get; set; } = false;

        public void CheckboxTenantSelect(int idTenant, object checkedValue)
        {
            IdTenant = idTenant;
            DisableCheckBox = (bool)checkedValue
            ? DisableCheckBox = true
            : DisableCheckBox = false;
        }
    }
}
