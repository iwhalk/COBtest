using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;

namespace FrontEnd.Components
{
    public partial class ButtonsDescriptions : ComponentBase
    {
        [Parameter]
        public EventCallback<int> OnClick { get; set; }
        [Parameter]
        public List<Description> DescriptionsList { get; set; }
    }
}
