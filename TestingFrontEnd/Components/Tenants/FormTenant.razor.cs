using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace TestingFrontEnd.Components.Tenants
{
    public partial class FormTenant : ComponentBase
    {
        [Parameter]
        public EventCallback OpenModalTenant { get; set; }
        [Parameter]
        public Tenant? tenant { get; set; }
    }
}
