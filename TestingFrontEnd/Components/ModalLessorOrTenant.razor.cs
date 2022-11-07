using Microsoft.AspNetCore.Components;

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
    }
}
