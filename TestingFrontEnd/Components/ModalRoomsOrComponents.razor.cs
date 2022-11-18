using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;

namespace FrontEnd.Components
{
    public partial class ModalRoomsOrComponents : ComponentBase
    {
        [Parameter]
        public List<Area> Areas { get; set; }
        [Parameter]
        public string TitleTable { get; set; } = "";
        [Parameter]
        public bool ShowModal { get; set; } = false;
        [Parameter]
        public EventCallback OnClick { get; set; }
    }
}
