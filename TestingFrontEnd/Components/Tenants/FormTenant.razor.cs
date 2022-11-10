using Microsoft.AspNetCore.Components;
using Shared.Models;
using SharedLibrary.Models;

namespace TestingFrontEnd.Components.Tenants
{
    public partial class FormTenant : ComponentBase
    {
        [Parameter]
        public EventCallback OpenModalTenant { get; set; }
        [Parameter]
        public Tenant? CurrentTenant { get; set; }
        [Parameter]
        public bool IsFormTenantExit { get; set; }

        public void HandlePost()
        {

            var l = new Tenant();
            l.Street = "'";
        }
    }
}
