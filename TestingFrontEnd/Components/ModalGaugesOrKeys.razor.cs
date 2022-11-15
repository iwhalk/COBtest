using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components
{
    public partial class ModalGaugesOrKeys : ComponentBase
    {
        [Parameter]
        public bool ShowModal { get; set; } = false;
        [Parameter]
        public string TitleModal { get; set; } = "";
        [Parameter]
        public EventCallback OnClick { get; set; }
    }
}
