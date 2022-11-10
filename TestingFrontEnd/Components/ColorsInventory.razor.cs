using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components
{
    public partial class ColorsInventory : ComponentBase
    {
        [Parameter]
        public EventCallback<string> OnClick { get; set; }
    }
}
