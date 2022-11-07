using Microsoft.AspNetCore.Components;

namespace TestingFrontEnd.Components
{
    public partial class ReceptionCertificatesPedingOrHistorical : ComponentBase
    {
        [Parameter]
        public string TypeTableReception { get; set; } = "";
    }
}
