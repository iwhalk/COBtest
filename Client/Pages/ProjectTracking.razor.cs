using Blazored.Toast;
using Blazored.Toast.Services;
using Excubo.Blazor.Canvas;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Obra.Client.Components;
using Obra.Client.Components.Blobs;
using Obra.Client.Interfaces;
using Obra.Client.Services;
using SharedLibrary.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using Activity = SharedLibrary.Models.Activity;
using Blob = SharedLibrary.Models.Blob;

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
        private readonly IBlobsService _blobsService;
        private readonly AuthenticationStateProvider _getAuthenticationStateAsync;
        private readonly NavigationManager _navigate;
        private readonly IObjectAccessService _accessService;

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
                               IToastService toastService,
                               IBlobsService blobsService,
                               IObjectAccessService accessService)
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
            _blobsService = blobsService;
            _accessService = accessService;
        }


        public ObjectAccessUser Accesos { get; private set; }
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
        public int IdBlob { get; private set; }
        public bool ShowModalPicture { get; private set; } = false;

        private FormBlob FormBlob;
        private ImageAnnotation ImageAnnotationComponent;

        private int SelectedApartment;
        private int SelectedArea;
        private int SelectedActivity;
        private int SelectedElement;
        private int SelectedSubElement;
        private bool ShowDetalle;
        private bool ShowFormBlob = true;
        private string? piecesCondition;

        protected override async Task OnInitializedAsync()
        {
            //var authstate = await _getAuthenticationStateAsync.GetAuthenticationStateAsync();
            //string idSupervisor = authstate.User?.Claims?.FirstOrDefault(x => x.Type.Equals("sub"))?.Value;
            //Accesos = await _progressReportService.GetObjectAccessAsync(idSupervisor);
            Accesos = await _accessService.GetObjectAccess();
            //ApartmentsList = await _apartmentsService.GetApartmentsAsync();
            ApartmentsList = Accesos.Apartments;
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
            //AreasList = AreasList ?? await _areasService.GetAreasAsync();
            AreasList = AreasList ?? Accesos.Areas;
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
            var enteros1 = Accesos.Activities.Select(x => x.IdActivity);
            ActivitiesList = ActivitiesList.Where(x => enteros1.Contains(x.IdActivity)).ToList();//*********************Probar
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
            var enteros2 = Accesos.Elements.Select(x => x.IdElement);
            CurrentElementsList = CurrentElementsList.Where(x => enteros2.Contains(x.IdElement)).ToList();//*********************Probar
            StateHasChanged();
        }
        public async void ElementButtonClicked(int idElement)
        {
            if (SelectedElement == idElement) return;
            SelectedElement= idElement;
            SelectedSubElement = 0;          

            CurrentElement = CurrentElementsList.First(x => x.IdElement == idElement);
            CurrentSubElementsList = await _subElementsService.GetSubElementsAsync(idElement);
            var enteros3 = Accesos.SubElements.Select(x => x.IdSubElement);
            CurrentSubElementsList = CurrentSubElementsList.Where(x => enteros3.Contains(x.IdSubElement)).ToList();//*********************Probar
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
            ShowFormBlob = true;
            CurrentProgressReport = (await _progressReportService.GetProgressReportsAsync(idBuilding: Accesos.IdBuilding, idApartment: CurrentApartment?.IdApartment, idArea: CurrentArea?.IdArea, 
                idElement: idElement ?? CurrentElement?.IdElement, idSubElement: idSubElement))?.OrderByDescending(x => x.DateCreated).FirstOrDefault();
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
                piecesCondition = CurrentProgressLog.Pieces;
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
                piecesCondition = e.Value?.ToString();
            }
            else
            {
                int.TryParse(e.Value?.ToString(), out int pieces);
                CurrentProgressLog.Pieces = pieces.ToString();
                CurrentProgressLog.IdProgressReport = CurrentProgressReport?.IdProgressReport ?? 1;                

                NewProgressLogs.Add(new ProgressLog()
                {
                    Pieces = pieces.ToString(),
                    IdProgressReport = CurrentProgressReport?.IdProgressReport ?? 1,
                    Observation = CurrentProgressLog.Observation,
                    IdStatus = CurrentProgressLog.IdStatus
                });

                piecesCondition = CurrentProgressLog.Pieces;
            }            

            if(e.Value?.ToString() == CurrentProgressReport?.TotalPieces)
            {
                CheckboxClicked(3, null);
            }
            else if (e.Value?.ToString() != "0")
            {
                CheckboxClicked(2, null);
            }
            else
            {
                CheckboxClicked(1, null);
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
            ShowFormBlob = (idStatus != 1) ? true :  false;
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
                //CurrentProgressLog.IdBlobs.Add(blob);
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
            //StateHasChanged();
        }
        public void OnClickImage(int value)
        {
            IdBlob = value;
            ShowModalPicture = ShowModalPicture ? false : true;
        }
        public async Task OnAnnotatedImageSave()        
        {
            //ShowBlobs = false;
            var signature = await ImageAnnotationComponent._context.ToDataURLAsync();
            var blob = await _blobsService.GetBlobsAsync(IdBlob);
            await _blobsService.PostImageAsync(new() { BlobUri = blob.FirstOrDefault()?.Uri, StringBase64 = signature });
            foreach (var item in CurrentProgressLog.IdBlobs)
            {
                item.Uri += $"?q={Guid.NewGuid().ToString().Remove(10):N}";
            }
            //FormBlobComponent.renderBlobsList = renderBlob;
            //ShowBlobs = true;
            ShowModalPicture = false;
        }
        public async Task OnAnnotatedImageDeleteAsync()
        {
            ShowModalPicture = false;
            var res = await _blobsService.DeleteBlobAsync(IdBlob);
            //if(res)
                CurrentProgressLog.IdBlobs.Remove(CurrentProgressLog.IdBlobs.FirstOrDefault(x => x.IdBlob == IdBlob));
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
            _toastService.ShowToast<ToastComponent>(new ToastInstanceSettings(5, false));
            //_navigate.NavigateTo("/");
        }
    }
}
