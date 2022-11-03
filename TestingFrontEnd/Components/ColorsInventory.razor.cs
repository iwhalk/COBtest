using Microsoft.AspNetCore.Components;

namespace TestingFrontEnd.Components
{
    public partial class ColorsInventory : ComponentBase
    {
        [Parameter]
        public EventCallback<string> OnClick { get; set; }
    }
}
