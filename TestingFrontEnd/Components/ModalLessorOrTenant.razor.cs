using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;

namespace TestingFrontEnd.Components
{
    public partial class ModalLessorOrTenant : ComponentBase
    {
        [Parameter]
        public string TitleModal { get; set; } = "";
        [Parameter]
        public bool ShowModal { get; set; }
        [Parameter]
        public EventCallback OnClick { get; set; }
        [Parameter]
        public List<Lessor> Lessors { get; set; } = new List<Lessor>();
        [Parameter]
        public List<Lessor> Tenants { get; set; } = new List<Lessor>();
    }
}
