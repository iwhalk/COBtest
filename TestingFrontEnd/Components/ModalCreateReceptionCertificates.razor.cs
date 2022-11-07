using Microsoft.AspNetCore.Components;

namespace TestingFrontEnd.Components
{
    public partial class ModalCreateReceptionCertificates : ComponentBase
    {
        [Parameter]
        public bool ShowModal { get; set; } = false;

    }
}
