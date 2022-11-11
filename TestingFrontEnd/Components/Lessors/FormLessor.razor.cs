using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SharedLibrary.Models;

namespace FrontEnd.Components.Lessors
{
    public partial class FormLessor : ComponentBase
    {
        [Parameter]
        public EventCallback OpenModalLessor { get; set; }
        [Parameter]
        public Lessor? CurrentLessor { get; set; } = new Lessor();
        [Parameter]
        public bool IsFormLessorExit { get; set; }

        public EditContext LessorEditContext;

        protected override void OnInitialized()
        {
            LessorEditContext = new EditContext(CurrentLessor);
        }
    }
}
