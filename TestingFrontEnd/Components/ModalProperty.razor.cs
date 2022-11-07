using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components
{
    public partial class ModalProperty : ComponentBase
    {
        public int MyProperty { get; set; }

        [Parameter]
        public bool ShowModal { get; set; } = false;
        [Parameter]
        public EventCallback OnClick { get; set; }
    }
}
