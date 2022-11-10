using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.DesignTokens;
using SharedLibrary.Models;

namespace TestingFrontEnd.Components.Lessors
{
    public partial class ModalLessor : ComponentBase
    {
        [Parameter]
        public bool ShowModal { get; set; }
        [Parameter]
        public List<Lessor> Lessors { get; set; }
        [Parameter]
        public EventCallback CloseModalLessor { get; set; }
        [Parameter]
        public EventCallback<int> SendIdLessor { get; set; }

        public int IdLessor { get; set; }
        public bool DisableCheckBox { get; set; } = false;

        public void CheckboxLessorSelect(int idLessor, object checkedValue)
        {
            IdLessor = idLessor;
            DisableCheckBox = (bool)checkedValue
            ? DisableCheckBox = true
            : DisableCheckBox = false;
        }
    }
}
