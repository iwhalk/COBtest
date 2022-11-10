using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components
{
    public partial class StatusInventory : ComponentBase
    {
        [Parameter]
        public EventCallback<string> OnClick { get; set; }
    }
}
