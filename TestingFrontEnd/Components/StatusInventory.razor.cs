using Microsoft.AspNetCore.Components;

namespace TestingFrontEnd.Components
{
    public partial class StatusInventory : ComponentBase
    {
        [Parameter]
        public EventCallback<string> OnClick { get; set; }
    }
}
