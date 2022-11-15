using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components
{
    public partial class ModalRoomsOrComponents : ComponentBase
    {
        [Parameter]
        public string TitleTable { get; set; } = "";
        [Parameter]
        public bool ShowModal { get; set; } = false;
        [Parameter]
        public EventCallback OnClick { get; set; }
    }
}
