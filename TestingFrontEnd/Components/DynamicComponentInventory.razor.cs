using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components
{
    public partial class DynamicComponentInventory : ComponentBase
    {
        [Parameter]
        public EventCallback<string> OnClick { get; set; }
    }
}
