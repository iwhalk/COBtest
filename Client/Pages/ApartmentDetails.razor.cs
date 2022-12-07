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
        private readonly IActivitiesService _activityService;
        private readonly IElementsService _elementsService;
        private readonly ISubElementsService _subElementsService;
        //Variable locales
        private List<int> _idsAparmentSelect { get; set; } = new();
        private List<int> _idsActivitySelect { get; set; } = new();
        private List<int> _idsElementSelect { get; set; } = new();
        private List<int> _idsSubElementSelect { get; set; } = new();
        private bool isFirstView { get; set; }
        private bool showModal { get; set; }

        public ApartmentDetails(ApplicationContext context, IApartmentsService apartmentsService, IActivitiesService activityService, IElementsService elementsService, ISubElementsService subElementsService)        
        {
            _context = context;
            _apartmentsService = apartmentsService;                        
            _activityService = activityService;
            _elementsService = elementsService;
            _subElementsService = subElementsService;
        }
        private void ChangeView() => isFirstView = isFirstView ? false : true;
        private void ChangeShowModal() => showModal = showModal ? false : true;
        protected async override Task OnInitializedAsync()
        {
            await _apartmentsService.GetApartmentsAsync();
            await _activityService.GetActivitiesAsync();
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
        private async void AddIdActivitySelect(int idActivity)
        {
            if (!_idsActivitySelect.Contains(idActivity))
            {
                _idsActivitySelect.Add(idActivity);                
                _context.Element = await _elementsService.GetElementsAsync(idActivity);
                _context.Element = _context.Element.Where(x => x.IdActivity == idActivity).ToList();
            }
            else
            {
                _context.Element = null;
                _idsActivitySelect = _idsActivitySelect.Where(x => x != idActivity).ToList();
            }
        }
        private async void AddIdElement(int idElement)
        {
            if (!_idsElementSelect.Contains(idElement))
            { 
                _idsElementSelect.Add(idElement);
                await _subElementsService.GetSubElementsAsync(idElement);
            }
            else
            {                
                _idsElementSelect.Where(x => x != idElement).ToList();
            }
        }
        private async void AddIdSubElement(int idSubElement)
        {
            if (!_idsSubElementSelect.Contains(idSubElement))            
                _idsSubElementSelect.Add(idSubElement);                            
            else
            {                
                _idsSubElementSelect.Where(x => x != idSubElement).ToList();
            }
        }
    }
}
