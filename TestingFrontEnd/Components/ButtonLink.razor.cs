using Microsoft.AspNetCore.Components;

namespace TestingFrontEnd.Components
{
    public partial class ButtonLink : ComponentBase
    {
        [Parameter]
        public string TypeIcon { get; set; } = "";
        [Parameter]
        public string RouteLink { get; set; } = "";
    }
}
