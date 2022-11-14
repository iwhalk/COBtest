using FrontEnd.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;
using System.Collections;

namespace FrontEnd.Components
{
    public partial class ModalAreas : ComponentBase
    {

        private readonly IAreaService _areaService;
        public ModalAreas(IAreaService areaService)
        {
            _areaService = areaService;
        }

        [Parameter]
        public string TitleTable { get; set; } = "";
        [Parameter]
        public List<Area> Areas { get; set; } = new();
        public List<Area> SelectedValues { get; set; } = new();
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
            Areas = await _areaService.GetAreaAsync();
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
