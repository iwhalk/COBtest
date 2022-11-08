using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace TestingFrontEnd.Components.Propertys
{
    public partial class FormProperty : ComponentBase
    {
        [Parameter]
        public EventCallback OpenModalProperty { get; set; }

        [Parameter]
        public Property? property { get; set; }
    }
}
