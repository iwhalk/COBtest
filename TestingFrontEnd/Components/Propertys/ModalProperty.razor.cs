using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;

namespace FrontEnd.Components.Propertys
{
    public partial class ModalProperty : ComponentBase
    {
        [Parameter]
        public bool ShowModal { get; set; }
        [Parameter]
        public List<Property> Propertys { get; set; }

        [Parameter]
        public EventCallback CloseModalProperty { get; set; }
        [Parameter]
        public EventCallback<int> SendIdProperty { get; set; }

        public int IdProperty { get; set; }
        public bool DisableCheckBox { get; set; } = false;

        public void CheckboxPropertySelect(int idProperty, object checkedValue)
        {
            IdProperty = idProperty;
            DisableCheckBox = (bool)checkedValue
            ? DisableCheckBox = true
            : DisableCheckBox = false;
        }
    }
}
