using FrontEnd.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;

namespace FrontEnd.Components
{
    public partial class ModalServices : ComponentBase
    {

        private readonly IServicesService _servicesService;
        public ModalServices(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        [Parameter]
        public string TitleTable { get; set; } = "";
        [Parameter]
        public List<Service> Services { get; set; } = new();
        public List<Service> SelectedValues { get; set; } = new();
        [Parameter]
        public Area Area { get; set; } = new();
        [Parameter]
        public bool ShowModal { get; set; } = false;
        [Parameter]
        public EventCallback OnClick { get; set; }
        [Parameter]
        public EventCallback AgregarOnClick { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Services = await _servicesService.GetServicesAsync();
        }
        public void CheckboxClicked(Service aSelectedId, object aChecked)
        {
            if ((bool)aChecked)
            {
                if (!SelectedValues.Contains(aSelectedId))
                {
                    SelectedValues.Add(aSelectedId);
                }
            }
            else
            {
                if (SelectedValues.Contains(aSelectedId))
                {
                    SelectedValues.Remove(aSelectedId);
                }
            }
            StateHasChanged();
        }
    }
}
