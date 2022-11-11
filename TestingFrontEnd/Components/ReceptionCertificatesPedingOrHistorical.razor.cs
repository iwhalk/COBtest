using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace FrontEnd.Components
{
    public partial class ReceptionCertificatesPedingOrHistorical : ComponentBase
    {
        [Parameter]
        public string TypeTableReception { get; set; } = "";
        [Parameter]
        public List<ActasRecepcion> Actas { get; set; }
    }
}
