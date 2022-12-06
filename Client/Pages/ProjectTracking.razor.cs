using Microsoft.AspNetCore.Components;
using Obra.Client.Interfaces;
using Obra.Client.Services;
using SharedLibrary.Models;

namespace Obra.Client.Pages
{
    public partial class ProjectTracking : ComponentBase
    {
        private readonly IBuildingsService _buildingsService;
        private readonly IApartmentsService _apartmentsService;
        //private readonly IAreasService _areasService;
        private readonly IActivitiesService _activitiesService;
        private readonly IElementsService _elementsService;
        private readonly ISubElementsService _subElementsService;
        private readonly IProgressReportService _progressReportService;
        private readonly IProgressLogsService _progressLogsService;

        public ProjectTracking(IBuildingsService buildingsService,
                               IApartmentsService apartmentsService,
                               //IAreasService areasService,
                               IActivitiesService activitiesService,
                               IElementsService elementsService,
                               ISubElementsService subElementsService,
                               IProgressReportService progressReportService,
                               IProgressLogsService progressLogsService)
        {
            _buildingsService = buildingsService;
            _apartmentsService = apartmentsService;
            //_areasService = areasService;
            _activitiesService = activitiesService;
            _elementsService = elementsService;
            _subElementsService = subElementsService;
            _progressReportService = progressReportService;
            _progressLogsService = progressLogsService;
        }

        public List<Apartment> ApartmentsList { get; private set; }
        public List<Area> AreasList { get; private set; }
        public List<Activity> ActivitiesList { get; private set; }
        public List<Element> CurrentElementsList { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            ApartmentsList = await _apartmentsService.GetApartmentsAsync();
            //AreasList = await _areasService.GetAreasAsync();
            ActivitiesList = await _activitiesService.GetActivitiesAsync();
            ActivitiesList = await _activitiesService.GetActivitiesAsync();
        }
        public async void ActivityButtonClicked(int idActivity)
        {
            CurrentElementsList = await _elementsService.GetElementsAsync();
        }
    }
}
