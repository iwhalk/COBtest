using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components
{
    public partial class ButtonLink : ComponentBase
    {
        [Parameter]
        public string TypeIcon { get; set; } = "";
        [Parameter]
        public string RouteLink { get; set; } = "";
    }
}
