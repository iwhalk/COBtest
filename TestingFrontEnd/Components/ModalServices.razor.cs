using FrontEnd.Interfaces;
using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;

namespace FrontEnd.Components
{
    public partial class ModalServices : ComponentBase
    {

        private readonly IServicesService _servicesService;
        private readonly ApplicationContext _context;
        public ModalServices(ApplicationContext context ,IServicesService servicesService)
        {
            _servicesService = servicesService;
            _context = context;
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
        [Parameter]
        public EventCallback<string> PostNewService { get; set; }
        public string NameService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Services = await _servicesService.GetServicesAsync();
        }
        private async void CleanBeforePostService()
        {
            await PostNewService.InvokeAsync(NameService);
            NameService = "";
            //_context.Area = null;          
            //Areas = await _areaService.GetAreaAsync();
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
