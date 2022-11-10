using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components
{
    public partial class LeftListActas : ComponentBase
    {
        [Parameter]
        public string TypeList { get; set; } = "";
        [Parameter]
        public EventCallback OnClick { get; set; }

        [Parameter]
        public EventCallback OpenModalGauges { get; set; }

        [Parameter]
        public EventCallback OpenModalKeys { get; set; }
    }
}
