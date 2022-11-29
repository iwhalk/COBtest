using FrontEnd.Interfaces;
using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;
using System.Collections;

namespace FrontEnd.Components
{
    public partial class ModalAreas : ComponentBase
    {

        private readonly IAreasService _areaService;
        private readonly ApplicationContext _context;
        public ModalAreas(ApplicationContext context,IAreasService areaService)
        {
            _areaService = areaService;
            _context = context;
        }

        [Parameter]
        public string TitleTable { get; set; } = "";
        [Parameter]
        public List<Area> Areas { get; set; } = new();
        public List<Area> SelectedValues { get; set; } = new();
        [Parameter]
        public Area Area { get; set; } = new();        
        [Parameter]
        public EventCallback OnClick { get; set; }
        [Parameter]
        public EventCallback AgregarOnClick { get; set; }
        [Parameter]
        public EventCallback<string> PostNewArea { get; set; }
        public string NameArea { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Areas = await _areaService.GetAreaAsync();
        }
        private async void CleanBeforePostArea()
        {
            await PostNewArea.InvokeAsync(NameArea);
            NameArea = "";
            //await OnClick.InvokeAsync();
            //_context.Area = null;          
            //Areas = await _areaService.GetAreaAsync();
        }
        public void CheckboxClicked(Area aSelectedId, object aChecked)
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
