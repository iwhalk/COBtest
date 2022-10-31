using Microsoft.AspNetCore.Components;

namespace Shared.Components
{
    public partial class ModalTypeActa : ComponentBase
    {

        [Parameter]
        public bool ShowModal { get; set; } = false;
        public ModalTypeActa()
        {            
        }
    }
}
