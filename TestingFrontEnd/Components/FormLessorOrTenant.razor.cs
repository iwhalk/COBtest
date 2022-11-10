using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components
{
    public partial class FormLessorOrTenant : ComponentBase
    {
        [Parameter]
        public string TitleForm { get; set; } = "";
        [Parameter]
        public EventCallback OnClick { get; set; }
    }
}
