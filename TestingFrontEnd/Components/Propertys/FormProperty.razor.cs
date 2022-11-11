using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SharedLibrary.Models;

namespace FrontEnd.Components.Propertys
{
    public partial class FormProperty : ComponentBase
    {

        [Parameter]
        public EventCallback OpenModalProperty { get; set; }
        [Parameter]
        public Property? CurrentProperty { get; set; } = new Property();
        [Parameter]
        public bool IsFormPropertyExit { get; set; }
        public EditContext PropertyEditContext;
        protected override void OnInitialized()
        {
            PropertyEditContext = new EditContext(CurrentProperty);
        }
    }
}