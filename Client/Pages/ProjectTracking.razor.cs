using Microsoft.AspNetCore.Components;
using Obra.Client.Interfaces;
using Obra.Client.Services;
using SharedLibrary.Models;
using System.Collections.Generic;
using System.Diagnostics;
using Activity = SharedLibrary.Models.Activity;

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
        public Activity CurrentActivity { get; private set; }
        public List<Element> CurrentElementsList { get; private set; }
        public SubElement CurrentSubElement { get; private set; }
        public ProgressReport? CurrentProgressReport { get; private set; }
        public ProgressLog? LastProgressLog { get; private set; }
        public Element CurrentElement { get; private set; }
        public List<SubElement> CurrentSubElementsList { get; private set; }
        public Area CurrentArea { get; private set; }
        public Apartment CurrentApartment { get; private set; }
        public ProgressLog CurrentProgressLog { get; private set; }
        public List<ProgressLog> NewProgressLogs { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            ApartmentsList = await _apartmentsService.GetApartmentsAsync();
            NewProgressLogs = new();
            CurrentProgressLog = new ProgressLog()
            {
                IdStatus = 1
            };
        }
        public async void ApartmentButtonClicked(int idApartment)
        {
            //AreasList = AreasList ?? await _areasService.GetAreasAsync();
            CurrentApartment = ApartmentsList.First(x=>x.IdApartment == idApartment);
            StateHasChanged();
        }
        public async void AreaButtonClicked(int idArea)
        {
            ActivitiesList = ActivitiesList ?? await _activitiesService.GetActivitiesAsync();
            CurrentArea = AreasList.First(x => x.IdArea == idArea);
            StateHasChanged();
        }
        public async void ActivityButtonClicked(int idActivity)
        {
            CurrentActivity = ActivitiesList.First(x => x.IdActivity == idActivity);
            CurrentElementsList = await _elementsService.GetElementsAsync(idActivity);
            StateHasChanged();
        }
        public async void ElementButtonClicked(int idElement)
        {
            CurrentElement = CurrentElementsList.First(x => x.IdElement == idElement);
            CurrentSubElementsList = await _subElementsService.GetSubElementsAsync(idElement);
            StateHasChanged();
        }
        public async void SubElementButtonClicked(int idSubElement)
        {
            CurrentSubElement = CurrentSubElementsList.First(x => x.IdSubElement == idSubElement);

            CurrentProgressReport = (await _progressReportService.GetProgressReportsAsync(/*idBuilding: 1, */idAparment: CurrentApartment?.IdApartment, idArea: CurrentArea?.IdArea, idElemnet: CurrentElement?.IdElement, idSubElement: idSubElement))?.OrderByDescending(x => x.DateCreated).FirstOrDefault();
            if (CurrentProgressReport != null)
            {
                var NewProgressLog = NewProgressLogs.FirstOrDefault(x => x.IdProgressReport == CurrentProgressReport.IdProgressReport);
                if (NewProgressLog != null)
                {
                    CurrentProgressLog.Observation = NewProgressLog.Observation;
                    CurrentProgressLog.IdStatus = NewProgressLog.IdStatus;
                    CurrentProgressLog.Pieces = NewProgressLog.Pieces;
                }
                else
                {
                    //LastProgressLog = (await _progressLogsService.GetProgressLogsAsync(idProgressReport: CurrentProgressReport.IdProgressReport))?.OrderByDescending(x => x.DateCreated).FirstOrDefault();
                    //if (LastProgressLog != null)
                    //{
                    //    CurrentProgressLog.IdProgressReport = LastProgressLog.IdProgressReport;
                    //    CurrentProgressLog.Observation = LastProgressLog.Observation;
                    //    CurrentProgressLog.IdStatus = LastProgressLog.IdStatus;
                    //    CurrentProgressLog.Pieces = LastProgressLog.Pieces;
                    //    CurrentProgressLog.IdProgressLog = 0;
                    //}
                    //else
                    //{
                    //    CurrentProgressLog = new ProgressLog()
                    //    {
                    //        IdStatus = 1
                    //    };
                    //}
                }

            }
            else
            {
                //Esto debe arrojar un error pues las piezas vienen precargadas
                CurrentProgressReport = new();
                CurrentProgressLog = new ProgressLog()
                {
                    IdStatus = 1
                };
            }

            StateHasChanged();
        }

        public async void PiecesInput(ChangeEventArgs e)
        {

            var NewProgressLog = NewProgressLogs.FirstOrDefault(x => x.IdProgressReport == CurrentProgressReport.IdProgressReport);
            if (NewProgressLog != null)
            {
                NewProgressLogs.FirstOrDefault(x => x.IdProgressReport == CurrentProgressReport.IdProgressReport).Pieces = e.Value?.ToString();
                //NewProgressLogs.FirstOrDefault(x => x.IdProgressReport == CurrentProgressReport.IdProgressReport).IdProgressReport = CurrentProgressReport?.IdProgressReport ?? 1;
            }
            else
            {
                CurrentProgressLog.Pieces = e.Value?.ToString();
                CurrentProgressLog.IdProgressReport = CurrentProgressReport?.IdProgressReport ?? 1;
                NewProgressLogs.Add(new ProgressLog()
                {
                    Pieces = e.Value?.ToString(),
                    IdProgressReport = CurrentProgressReport?.IdProgressReport ?? 1,
                    Observation = CurrentProgressLog.Observation,
                    IdStatus = CurrentProgressLog.IdStatus
                });
            }

            StateHasChanged();
        }
    }
}
