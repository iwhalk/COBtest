using Microsoft.AspNetCore.Components;
using Shared.Models;
namespace TestingFrontEnd.Components.Lessors
{
    public partial class Form : ComponentBase
    {
        [Parameter]
        public EventCallback OpenModalLessor { get; set; }
        [Parameter]
        public Lessor? lessor { get; set; }

    }
}
