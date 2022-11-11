using Microsoft.AspNetCore.Components;
using Shared.Models;
using SharedLibrary.Models;

namespace FrontEnd.Components
{
    public partial class ReceptionCertificatesPedingOrHistorical : ComponentBase
    {
        [Parameter]
        public string TypeTableReception { get; set; } = "";
        [Parameter]
        public List<ActasRecepcion> Actas { get; set; }
        [Parameter]
        public List<PropertyType> PropertyTypes { get; set; }
        [Parameter]
        public List<Tenant> Tenants { get; set; }
        [Parameter]
        public List<Lessor> Lessors { get; set; }
    }
}
