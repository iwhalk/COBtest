using FrontEnd.Interfaces;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace FrontEnd.Pages
{
    public partial class PruebasGateway : ComponentBase
    {
        private readonly IAreaService _areaService;
        public PruebasGateway(IAreaService areaService)
        {
            _areaService = areaService;
        }

        private List<Area> ListArea { get; set; }
        private Area Area { get; set; }

        Area ejem = new Area()
        {
            AreaName = "PruebaBaseAddress"
        };

        protected override async Task OnInitializedAsync()
        {
            ListArea = await _areaService.GetAreaAsync();

            Area = await _areaService.PostAreaAsync(ejem);
        }
    }
}
