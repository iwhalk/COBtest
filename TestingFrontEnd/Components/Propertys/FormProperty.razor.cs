using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SharedLibrary.Models;

namespace TestingFrontEnd.Components.Propertys
{
    public partial class FormProperty : ComponentBase
    {

        [Parameter]
        public EventCallback OpenModalProperty { get; set; }
        [Parameter]
        public Property? CurrentProperty { get; set; }
        [Parameter]
        public bool IsFormPropertyExit { get; set; }

        public EditContext PropertyEditContext { get; set; }
    }
}