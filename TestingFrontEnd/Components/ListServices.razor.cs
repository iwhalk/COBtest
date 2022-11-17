using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;

namespace FrontEnd.Components
{
    public partial class ListServices : ComponentBase
    {
        [Parameter]
        public string TypeList { get; set; } = "";
        [Parameter]
        public List<Service> Services { get; set; } = new();
        [Parameter]
        public Service Service { get; set; } = new();
        [Parameter]
        public EventCallback OnClick { get; set; }
        [Parameter]
        public EventCallback<int> OnMinusClick { get; set; }

        [Parameter]
        public EventCallback OpenModalGauges { get; set; }

        [Parameter]
        public EventCallback OpenModalKeys { get; set; }
        [Parameter]
        public EventCallback<int> OnServiceClick { get; set; }
        public int SelectedService { get; set; }

        protected override async Task OnInitializedAsync()
        {

        }
        private async Task OnClicked(int IdService)
        {
            SelectedService = IdService;
            await OnServiceClick.InvokeAsync(IdService);
        }
    }
}
