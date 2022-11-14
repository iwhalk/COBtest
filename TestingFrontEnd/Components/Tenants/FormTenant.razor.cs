using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Shared.Models;
using SharedLibrary.Models;

namespace FrontEnd.Components.Tenants
{
    public partial class FormTenant : ComponentBase
    {
        [Parameter]
        public EventCallback OpenModalTenant { get; set; }
        [Parameter]
        public Tenant? CurrentTenant { get; set; }
        [Parameter]
        public bool IsFormTenantExit { get; set; }
        public EditContext TenantEditContext;
        protected override void OnInitialized()
        {
            TenantEditContext = new EditContext(CurrentTenant);
        }
    }
}
