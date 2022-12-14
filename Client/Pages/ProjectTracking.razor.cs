﻿using Blazored.Toast;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Obra.Client.Components;
using Obra.Client.Components.Blobs;
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
        private readonly IAreasService _areasService;
        private readonly IActivitiesService _activitiesService;
        private readonly IElementsService _elementsService;
        private readonly ISubElementsService _subElementsService;
        private readonly IProgressReportService _progressReportService;
        private readonly IProgressLogsService _progressLogsService;
        private readonly IToastService _toastService;
        private readonly AuthenticationStateProvider _getAuthenticationStateAsync;
        private readonly NavigationManager _navigate;

        public ProjectTracking(IBuildingsService buildingsService,
                               IApartmentsService apartmentsService,
                               IAreasService areasService,
                               IActivitiesService activitiesService,
                               IElementsService elementsService,
                               ISubElementsService subElementsService,
                               IProgressReportService progressReportService,
                               IProgressLogsService progressLogsService,
                               AuthenticationStateProvider getAuthenticationStateAsync,
                               NavigationManager navigate,
                               IToastService toastService)
        {
            _buildingsService = buildingsService;
            _apartmentsService = apartmentsService;
            _areasService = areasService;
            _activitiesService = activitiesService;
            _elementsService = elementsService;
            _subElementsService = subElementsService;
            _progressReportService = progressReportService;
            _progressLogsService = progressLogsService;
            _getAuthenticationStateAsync = getAuthenticationStateAsync;
            _navigate = navigate;
            _toastService = toastService;
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
        public bool Loading { get; private set; }

        private FormBlob FormBlob;
        private int SelectedApartment;
        private int SelectedArea;
        private int SelectedActivity;
        private int SelectedElement;
        private int SelectedSubElement;
        private bool ShowDetalle;

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
            SelectedApartment= idApartment;
            SelectedArea = 0;
            ActivitiesList = null;
            SelectedActivity = 0;
            CurrentElementsList = null;
            SelectedElement = 0;
            CurrentSubElementsList = null;
            SelectedSubElement= 0;
            ShowDetalle= false;

            AreasList = AreasList ?? await _areasService.GetAreasAsync();
            CurrentApartment = ApartmentsList.First(x=>x.IdApartment == idApartment);
            StateHasChanged();
        }
        public async void AreaButtonClicked(int idArea)
        {
            SelectedArea= idArea;
            SelectedActivity = 0;
            CurrentElementsList = null;
            SelectedElement = 0;
            CurrentSubElementsList = null;
            SelectedSubElement = 0;
            ShowDetalle = false;

            ActivitiesList = await _activitiesService.GetActivitiesAsync(idArea);
            CurrentArea = AreasList.First(x => x.IdArea == idArea);
            StateHasChanged();
        }
        public async void ActivityButtonClicked(int idActivity)
        {
            SelectedActivity = idActivity;
            SelectedElement = 0;
            CurrentSubElementsList = null;
            SelectedSubElement = 0;
            ShowDetalle = false;

            CurrentActivity = ActivitiesList.First(x => x.IdActivity == idActivity);
            CurrentElementsList = await _elementsService.GetElementsAsync(idActivity);
            StateHasChanged();
        }
        public async void ElementButtonClicked(int idElement)
        {
            if (SelectedElement == idElement) return;
            SelectedElement= idElement;
            SelectedSubElement = 0;          

            CurrentElement = CurrentElementsList.First(x => x.IdElement == idElement);
            CurrentSubElementsList = await _subElementsService.GetSubElementsAsync(idElement);
            if (CurrentSubElementsList == null || CurrentSubElementsList.Count < 1)
            {
                GetCurrentProgressReport(idElement: idElement);
                ShowDetalle= true;
            }
            else
            {
                ShowDetalle = false;
            }

            StateHasChanged();
        }

        public async Task GetCurrentProgressReport(int? idElement =  null, int? idSubElement = null)
        {
            CurrentProgressReport = (await _progressReportService.GetProgressReportsAsync(idBuilding: 1, idAparment: CurrentApartment?.IdApartment, idArea: CurrentArea?.IdArea, 
                idElemnet: idElement ?? CurrentElement?.IdElement, idSubElement: idSubElement))?.OrderByDescending(x => x.DateCreated).FirstOrDefault();
            if (CurrentProgressReport != null)
            {
                var NewProgressLog = NewProgressLogs.FirstOrDefault(x => x.IdProgressReport == CurrentProgressReport.IdProgressReport);
                if (NewProgressLog != null)
                {
                    CurrentProgressLog.Observation = NewProgressLog.Observation;
                    CurrentProgressLog.IdStatus = NewProgressLog.IdStatus;
                    CurrentProgressLog.Pieces = NewProgressLog.Pieces;
                    CurrentProgressLog.IdBlobs = NewProgressLog.IdBlobs ?? new List<Blob>();
                }
                else
                {
                    LastProgressLog = (await _progressLogsService.GetProgressLogsAsync(idProgressReport: CurrentProgressReport.IdProgressReport))?.OrderByDescending(x => x.DateCreated).FirstOrDefault();
                    if (LastProgressLog != null)
                    {
                        CurrentProgressLog.IdProgressReport = LastProgressLog.IdProgressReport;
                        CurrentProgressLog.Observation = LastProgressLog.Observation;
                        CurrentProgressLog.IdStatus = LastProgressLog.IdStatus;
                        CurrentProgressLog.Pieces = LastProgressLog.Pieces;
                        CurrentProgressLog.IdBlobs = LastProgressLog.IdBlobs ?? new List<Blob>();
                        CurrentProgressLog.IdProgressLog = 0;
                    }
                    else
                    {
                        CurrentProgressLog = new ProgressLog()
                        {
                            IdStatus = 1
                        };
                    }
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

        public async void SubElementButtonClicked(int idSubElement)
        {
            if (SelectedSubElement == idSubElement) return;
            SelectedSubElement= idSubElement;
            CurrentSubElement = CurrentSubElementsList.First(x => x.IdSubElement == idSubElement);
            CurrentProgressLog = new ();
            StateHasChanged();

            GetCurrentProgressReport(idSubElement: idSubElement);

            ShowDetalle = true;
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
            if(e.Value?.ToString() == CurrentProgressReport?.TotalPieces)
            {
                CheckboxClicked(3, null);
            }
            else
            {
                CheckboxClicked(2, null);
            }

            StateHasChanged();
        }
        public async void ObservationInput(ChangeEventArgs e)
        {

            var NewProgressLog = NewProgressLogs.FirstOrDefault(x => x.IdProgressReport == CurrentProgressReport.IdProgressReport);
            if (NewProgressLog != null)
            {
                NewProgressLogs.FirstOrDefault(x => x.IdProgressReport == CurrentProgressReport.IdProgressReport).Observation = e.Value?.ToString();
                //NewProgressLogs.FirstOrDefault(x => x.IdProgressReport == CurrentProgressReport.IdProgressReport).IdProgressReport = CurrentProgressReport?.IdProgressReport ?? 1;
            }
            else
            {
                CurrentProgressLog.Observation = e.Value?.ToString();
                CurrentProgressLog.IdProgressReport = CurrentProgressReport?.IdProgressReport ?? 1;
                NewProgressLogs.Add(new ProgressLog()
                {
                    Pieces = CurrentProgressLog.Pieces,
                    Observation = e.Value?.ToString(),
                    IdProgressReport = CurrentProgressReport?.IdProgressReport ?? 1,
                    IdStatus = CurrentProgressLog.IdStatus
                });
            }

            StateHasChanged();
        }
        public async void CheckboxClicked(int idStatus, ChangeEventArgs e)
        {
            CurrentProgressLog.IdStatus = idStatus;
            var NewProgressLog = NewProgressLogs.FirstOrDefault(x => x.IdProgressReport == CurrentProgressReport.IdProgressReport);
            if (NewProgressLog != null)
            {
                NewProgressLogs.FirstOrDefault(x => x.IdProgressReport == CurrentProgressReport.IdProgressReport).IdStatus = idStatus;
                //NewProgressLogs.FirstOrDefault(x => x.IdProgressReport == CurrentProgressReport.IdProgressReport).IdProgressReport = CurrentProgressReport?.IdProgressReport ?? 1;
            }
            else
            {
                CurrentProgressLog.IdStatus = idStatus;
                CurrentProgressLog.IdProgressReport = CurrentProgressReport?.IdProgressReport ?? 1;
                NewProgressLogs.Add(new ProgressLog()
                {
                    Pieces = CurrentProgressLog.Pieces,
                    Observation = CurrentProgressLog.Observation,
                    IdProgressReport = CurrentProgressReport?.IdProgressReport ?? 1,
                    IdStatus = idStatus
                });
            }

            StateHasChanged();
        }
        public async void HandleInputFile(Blob blob)
        {
            //CurrentProgressLog.IdBlobs.Add(blob);
            var NewProgressLog = NewProgressLogs.FirstOrDefault(x => x.IdProgressReport == CurrentProgressReport.IdProgressReport);
             if (NewProgressLog != null)
            {
                NewProgressLogs.FirstOrDefault(x => x.IdProgressReport == CurrentProgressReport.IdProgressReport).IdBlobs.Add(blob);
                //NewProgressLogs.FirstOrDefault(x => x.IdProgressReport == CurrentProgressReport.IdProgressReport).IdProgressReport = CurrentProgressReport?.IdProgressReport ?? 1;
            }
            else
            {
                CurrentProgressLog.IdBlobs.Add(blob);
                CurrentProgressLog.IdProgressReport = CurrentProgressReport?.IdProgressReport ?? 1;
                NewProgressLogs.Add(new ProgressLog()
                {
                    Pieces = CurrentProgressLog.Pieces,
                    Observation = CurrentProgressLog.Observation,
                    IdProgressReport = CurrentProgressReport?.IdProgressReport ?? 1,
                    IdStatus = CurrentProgressLog.IdStatus,
                    IdBlobs = new List<Blob>() { blob }
                });
            }
            FormBlob.CurrentBlobFileEditContext.Validate();
            StateHasChanged();
        }
        public async void SaveButtonClicked()
        {
            if (NewProgressLogs.Count < 1)
                return;
            Loading = true;
            var authstate = await _getAuthenticationStateAsync.GetAuthenticationStateAsync();
            foreach (var progressLog in NewProgressLogs)
            {
                progressLog.IdSupervisor = authstate.User?.Claims?.FirstOrDefault(x => x.Type.Equals("sub"))?.Value;
                progressLog.DateCreated = DateTime.Now;
                _ = await _progressLogsService.PostProgressLogAsync(progressLog);
            }
            NewProgressLogs.Clear();
            Loading = false;
            StateHasChanged();
            //_toastService.ShowSuccess("Se guardaron los campos del detalle de avance");
            _toastService.ShowToast<MyToastComponent>(new ToastInstanceSettings(5, false));
            //_navigate.NavigateTo("/");
        }
    }
}