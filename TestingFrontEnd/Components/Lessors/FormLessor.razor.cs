using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SharedLibrary.Models;

namespace TestingFrontEnd.Components.Lessors
{
    public partial class FormLessor : ComponentBase
    {
        [Parameter]
        public EventCallback OpenModalLessor { get; set; }
        [Parameter]
        public Lessor? CurrentLessor { get; set; }
        [Parameter]
        public bool IsFormLessorExit { get; set; }

        public EditContext LessorEditContext { get; set; }

    }
}
