using Microsoft.AspNetCore.Components;
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

        public void HandlePost()
        {

            var l = new Property();
            l.Street = "'";
        }
    }
}