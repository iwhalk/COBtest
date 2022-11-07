using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components
{
    public partial class ReceptionCertificatesPedingOrHistorical : ComponentBase
    {
        [Parameter]
        public string TypeTableReception { get; set; } = "";
    }
}
