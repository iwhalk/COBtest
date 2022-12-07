using Microsoft.AspNetCore.Components;
using Obra.Client.Interfaces;
using Obra.Client.Stores;
using System.Runtime.CompilerServices;

namespace Obra.Client.Pages
{
    public partial class ApartmentDetails : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly IApartmentsService _apartmentsService;
        private readonly IAreasService _areaService;
        private readonly IElementsService _elementsService;
        //Variable locales
        private List<int> _idsAparmentSelect { get; set; } = new();
        private List<int> _idsAreaSelect { get; set; } = new();
        private bool isFirstView { get; set; } = true;
        private bool showModal { get; set; } = false;

        public ApartmentDetails(ApplicationContext context, IApartmentsService apartmentsService, IAreasService areasService, IElementsService elementsService)        
        {
            _context = context;
            _apartmentsService = apartmentsService;                        
            _areaService = areasService;
            _elementsService = elementsService;
        }
        private void ChangeView() => isFirstView = isFirstView ? false : true;
        private void ChangeShowModal() => showModal = showModal ? false : true;
        protected async override Task OnInitializedAsync()
        {
            await _apartmentsService.GetApartmentsAsync();
            await _areaService.GetAreasAsync();
        }
        private void AddIdAparmentSelect(int idDeparment)
        {
            if (!_idsAparmentSelect.Contains(idDeparment))
                _idsAparmentSelect.Add(idDeparment);

            else
            {
                _idsAparmentSelect = _idsAparmentSelect.Where(x => x != idDeparment).ToList();
            }
        }
        private async void AddIdAreaSelect(int idArea)
        {
            if (!_idsAreaSelect.Contains(idArea))
            {
                _idsAreaSelect.Add(idArea);
                await _elementsService.GetElementsAsync();
            }
            else
            {
                _idsAreaSelect = _idsAreaSelect.Where(x => x != idArea).ToList();
            }
        }
    }
}
