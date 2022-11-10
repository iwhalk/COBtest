using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;

namespace FrontEnd.Components
{
    public partial class ListAreas : ComponentBase
    {
        [Parameter]
        public string TypeList { get; set; } = "";
        [Parameter]
        public List<Area> Areas { get; set; } = new();
        [Parameter]
        public Area Area { get; set; } = new();
        [Parameter]
        public EventCallback OnClick { get; set; }

        [Parameter]
        public EventCallback OpenModalGauges { get; set; }

        [Parameter]
        public EventCallback OpenModalKeys { get; set; }

        protected override async Task OnInitializedAsync()
        {

        }
    }
}
