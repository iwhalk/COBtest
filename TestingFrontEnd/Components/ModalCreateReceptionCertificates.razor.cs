using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components
{
    public partial class ModalCreateReceptionCertificates : ComponentBase
    {
        [Parameter]
        public bool ShowModal { get; set; } = false;

    }
}
