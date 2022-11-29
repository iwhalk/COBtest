using Client.Interfaces;
using Client.Stores;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;
using System.Reflection.Metadata.Ecma335;

namespace Client.Pages
{
    public partial class Index : ComponentBase
    {
        private readonly NavigationManager _navigationManager;
        private readonly ApplicationContext _context;
        private readonly IAreaService _areaService;
        public Index(NavigationManager navigationManager, ApplicationContext context, IAreaService areaService)
        {
            _navigationManager = navigationManager;
            _context = context;
            _areaService = areaService;
        }

        public List<AreaService> Areas { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            Areas = await _areaService.GetAreaServicesAsync();
        }
    }
}