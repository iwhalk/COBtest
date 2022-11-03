using Microsoft.AspNetCore.Components;

namespace TestingFrontEnd.Components
{
    public partial class DynamicComponentInventory : ComponentBase
    {
        [Parameter]
        public EventCallback<string> OnClick { get; set; }
    }
}
