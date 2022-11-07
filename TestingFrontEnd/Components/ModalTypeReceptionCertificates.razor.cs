using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components
{
    public partial class ModalTypeReceptionCertificates : ComponentBase
    {
        [Parameter]
        public bool ShowModal { get; set; }
        [Parameter]
        public EventCallback OnClick { get; set; }
    }
}
