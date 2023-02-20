﻿using Blazored.Toast;
using Blazored.Toast.Services;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;
using Obra.Client.Components;
using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;

namespace Obra.Client.Pages
{
    public partial class ActivityDetails
    {
        private readonly ApplicationContext _context;
        private readonly IApartmentsService _apartmentsService;
        private readonly IActivitiesService _activitiesService;
        private readonly IAreasService _areasService;
        private readonly IElementsService _elementsService;
        private readonly ISubElementsService _subElementsService;
        private readonly IProgressReportService _progressReportService;
        private readonly IProgressLogsService _progressLogsService;
        private readonly IReportsService _reportesService;
        private readonly IJSRuntime _JS;
        private readonly IToastService _toastService;
        private List<Apartment> apartments { get; set; } = new List<Apartment>();
        private List<SharedLibrary.Models.Activity> activities { get; set; } = new List<Activity>();
        private List<Area> Areas { get; set; }
        private List<Element> elements { get; set; } = new List<Element>();
        private List<SubElement> subElements { get; set; } = new List<SubElement>();

        private List<int> _idsAparmentSelect { get; set; } = new();
        private List<int> _idsActivitiesSelect { get; set; } = new();
        private List<int> _idsElementsSelect { get; set; } = new();
        private List<int> _idsSubElementsSelect { get; set; } = new();

        private List<ProgressReport> progressReports { get; set; } = new();
        private List<ProgressLog> progressLogs { get; set; } = new();
        private List<Activity> activitiesSelect { get; set; } = new();
        private List<Element> elementsSelect { get; set; } = new();
        private List<SubElement> subElementsSelect { get; set; } = new();
        private List<Apartment> apartmentsSelect { get; set; } = new();
        private SharedLibrary.Models.Activity activity { get; set; }
        private Element element { get; set; }

        private string messageError = "";
        private int activityIdAux = 0;
        private int elementIdAux = 0;
        private bool alert = false;
        public bool activityDetails = true;
        public bool subElement = false;
        public bool department = false;
        private bool allApartments = false;
        private bool allSubElements = false;
        private bool showElements = false;
        private bool showSubElements = false;
        private bool apartmentDetails = true;
        private bool subElementsNulls = false;
        private bool buttonReport = false;

        private bool showModal { get; set; } = false;
        private bool loading { get; set; } = false;

        private string observations { get; set; } = "";
        private List<string> images { get; set; } = new();

        Dictionary<string, string> greenPercentage { get; set; } = new();
        Dictionary<string, string> redPercentage { get; set; } = new();


        private bool _showPreviewFile { get; set; }
        private byte[] _bytesPreviewFile { get; set; }
        private const string PDF_FILE_NAME = "DetallePorActividad.pdf";
        private void ChangeOpenModalPreview() => _showPreviewFile = _showPreviewFile ? false : true;
        public ActivityDetails(ApplicationContext context, IApartmentsService apartmentsService, IActivitiesService activitiesService, IAreasService areasService, IElementsService elementsService, ISubElementsService subElementsService, IProgressReportService progressReportService, IProgressLogsService progressLogsService, IReportsService reportesService, IJSRuntime jS, IToastService toastService)
        {
            _context = context;
            _apartmentsService = apartmentsService;
            _activitiesService = activitiesService;
            _areasService = areasService;
            _elementsService = elementsService;
            _subElementsService = subElementsService;
            _progressReportService = progressReportService;
            _progressLogsService = progressLogsService;
            _reportesService = reportesService;
            _JS = jS;
            _toastService = toastService;
        }

        protected async override Task OnInitializedAsync()
        {
            apartments = await _apartmentsService.GetApartmentsAsync();
            activities = await _activitiesService.GetActivitiesAsync();
            Areas = await _areasService.GetAreasAsync();
        }

        public async Task AddIdSelect(int id, int filter)
        {
            if (filter == 1) //Departemento
            {
                if (!_idsAparmentSelect.Contains(id))
                {
                    _idsAparmentSelect.Add(id);

                    allApartments = false;

                    await ShowMessage();
                }
                else
                {
                    _idsAparmentSelect.Remove(id);
                }
            }
            else if (filter == 2) //Actividad
            {
                if (!_idsActivitiesSelect.Contains(id))
                {
                    _idsActivitiesSelect.Add(id);
                    showElements = true;

                    List<Element> auxElements = await _elementsService.GetElementsAsync(id);

                    elements.AddRange(auxElements);

                    await ShowMessage();
                }
                else
                {
                    _idsActivitiesSelect.Remove(id);
                }
            }
            else if (filter == 3) //Elemento
            {
                if (!_idsElementsSelect.Contains(id))
                {
                    _idsElementsSelect.Add(id);
                    showSubElements = true;

                    List<SubElement> auxSubElement = await _subElementsService.GetSubElementsAsync(id);

                    subElements.AddRange(auxSubElement);

                    if (subElements == null || subElements.Count() < 1)
                    {
                        department = true;
                    }

                    await ShowMessage();
                }
                else
                {
                    _idsElementsSelect.Remove(id);
                }
            }
            else if (filter == 4) //SubElemento
            {
                if (!_idsSubElementsSelect.Contains(id))
                {
                    _idsSubElementsSelect.Add(id);

                    department = true;
                    allSubElements = false;

                    await ShowMessage();
                }
                else
                {
                    _idsSubElementsSelect.Remove(id);

                    await ShowMessage();

                    if (_idsSubElementsSelect.Count() < 1)
                    {
                        department = false;
                        allApartments = false;
                        _idsAparmentSelect.Clear();
                    }
                }
            }
        }

        public async Task AllSubElements()
        {
            if (_idsSubElementsSelect.Count() < 1 && allSubElements == false)
            {
                allSubElements = true;
                department = true;
            }
            else
            {
                allSubElements = false;
                _idsSubElementsSelect.Clear();
                _idsAparmentSelect.Clear();
                allApartments = false;
                department = false;
            }
        }

        public async Task AllApartments()
        {
            if (_idsAparmentSelect.Count() < 1 && allApartments == false)
            {
                allApartments = true;
            }
            else
            {
                allApartments = false;

                _idsAparmentSelect.Clear();

                await ShowMessage();
            }
        }

        public async Task ShowMessage() => alert = false;
        public async Task ShowElements() => showElements = false;
        public async Task ShowSubElements() => showSubElements = false;
        public async Task ShowDepartment() => department = true;

        public void ChangeShowModal()
        {
            showModal = showModal ? false : true;
            observations = "";
            images.Clear();
        }

        public async Task GoBack()
        {
            await ShowMessage();
            if (apartments != null)
            {
                _idsAparmentSelect.Clear();
                department = false;
                allApartments = false;
            }

            _idsActivitiesSelect.Clear();

            await ShowElements();
            elements.Clear();
            _idsElementsSelect.Clear();

            await ShowSubElements();
            subElements.Clear();
            _idsSubElementsSelect.Clear();

            buttonReport = false;
            apartmentDetails = true;

            progressLogs.Clear();
            progressReports.Clear();
            subElementsSelect.Clear();
            apartmentsSelect.Clear();

            activity = null;
            element = null;

            subElementsNulls = false;
            allApartments = false;
            allSubElements = false;

            greenPercentage.Clear();
            redPercentage.Clear();
        }

        public async Task ChangeView()//DESCARGAR
        {
            await ShowMessage();
            loading = true;
            buttonReport = false;

            ActivitiesDetail reporte = new();
            reporte.Activities = new();
            reporte.Elements = new();

            if (subElementsSelect != null)
            {
                reporte.IdBuilding = 1;
                reporte.Apartments = _idsAparmentSelect;
                reporte.Activities = _idsActivitiesSelect;
                reporte.Elements = _idsElementsSelect;
                reporte.SubElements = _idsSubElementsSelect;
            }
            else
            {
                reporte.IdBuilding = 1;
                reporte.Apartments = _idsAparmentSelect;
                reporte.Activities.Add(activity.IdActivity);
                reporte.Elements.Add(element.IdElement);
            }

            var pdf = await _reportesService.PostReporteDetallesPorActividadAsync(reporte);

            if (pdf != null)
            {
                _bytesPreviewFile = pdf;
                loading = false;
                _showPreviewFile = true;
                StateHasChanged();
                // var fileName = "DetallePorActividad.pdf";
                // var fileStream = new MemoryStream(pdf);
                // using var streamRef = new DotNetStreamReference(stream: fileStream);
                // await _JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            }
            else
            {
                _toastService.ShowToast<ToastReport>(new ToastInstanceSettings(5, false));
            }

            loading = false;
            buttonReport = true;
        }

        public async Task ShowReportAndHideApartment()
        {
            buttonReport = true;
            loading = true;
            Activity activity = new();
            Element element = new();
            SubElement subElement = new();

            if (allSubElements == true)
            {
                foreach (var idSubElement in subElements)
                {
                    _idsSubElementsSelect.Add(idSubElement.IdSubElement);
                }
            }

            foreach (var item in _idsActivitiesSelect)
            {
                activity = activities.FirstOrDefault(x => x.IdActivity == item);

                activitiesSelect.Add(new Activity() { IdActivity = activity.IdActivity, ActivityName = activity.ActivityName, Elements = activity.Elements, IdAreas = activity.IdAreas });
            }

            foreach (var item in _idsElementsSelect)
            {
                element = elements.FirstOrDefault(x => x.IdElement == item);

                elementsSelect.Add(new Element() { IdElement = element.IdElement, ElementName = element.ElementName, IdActivity = element.IdActivity });
            }

            foreach (var item in _idsSubElementsSelect)
            {
                subElement = subElements.FirstOrDefault(x => x.IdSubElement == item);

                subElementsSelect.Add(new SubElement() { IdSubElement = subElement.IdSubElement, SubElementName = subElement.SubElementName, IdElement = subElement.IdElement, Type = subElement.Type });
            }

            await AdvancementProgressSubElements();

            if (alert == false)
            {
                await DivisionOfColors();

                apartmentDetails = false;
            }

            loading = false;
        }

        public async Task AdvancementProgressSubElements()
        {
            List<ProgressReport> progresses = new();
            ProgressReport auxR = new();
            Apartment apartment = new();
            ProgressLog auxL = new();

            foreach (var apart in _idsAparmentSelect)
            {
                progresses = await _progressReportService.GetProgressReportsAsync(idBuilding: 1, idApartment: apart, includeProgressLogs: true);

                foreach (var activity in activitiesSelect)
                {
                    foreach (var area in Areas)
                    {
                        var aux = activity.IdAreas.FirstOrDefault(x => x.IdArea == area.IdArea).IdArea;

                        if (aux != null)
                        {
                            foreach (var element in elementsSelect)
                            {
                                if (element.IdActivity == activity.IdActivity)
                                {
                                    var molina = progresses.Where(x => subElementsSelect.Any(y => x.IdSubElement.Equals(y.IdSubElement)));
                                    auxR = progresses.OrderByDescending(x => x.DateCreated).FirstOrDefault(x => x.IdArea == aux && x.IdElement == element.IdElement);
                                    if (auxR != null)
                                        progressReports.Add(auxR);
                                }
                            }
                        }
                    }
                }

                apartment = apartments.FirstOrDefault(x => x.IdApartment == apart);

                apartmentsSelect.Add(new Apartment() { IdApartment = apartment.IdApartment, ApartmentNumber = apartment.ApartmentNumber });
            }

            foreach (var item in progressReports)
            {
                if (item.ProgressLogs != null && item.ProgressLogs.Count() > 0)
                {
                    auxL = item.ProgressLogs.OrderByDescending(x => x.DateCreated).FirstOrDefault();

                    progressLogs.Add(new ProgressLog() { IdProgressLog = auxL.IdProgressLog, IdProgressReport = auxL.IdProgressReport, DateCreated = auxL.DateCreated, IdStatus = auxL.IdStatus, Pieces = auxL.Pieces, Observation = auxL.Observation, IdSupervisor = auxL.IdSupervisor, IdBlobs = auxL.IdBlobs });
                }
            }
        }

        public async Task AdvancementProgress()
        {
            //Lo nuevo super rapido alv
            List<ProgressReport> progresses = new();
            ProgressReport auxR = new();
            Apartment apartment = new();
            ProgressLog auxL = new();

            foreach (var apart in _idsAparmentSelect)
            {
                progresses = await _progressReportService.GetProgressReportsAsync(idBuilding: 1, idApartment: apart, includeProgressLogs: true);

                foreach (var activity in _idsActivitiesSelect)
                {
                    foreach (var element in _idsElementsSelect)
                    {
                        auxR = progresses.OrderByDescending(x => x.DateCreated).FirstOrDefault(x => x.IdElement == _idsElementsSelect.FirstOrDefault());

                        if (auxR != null)
                            progressReports.Add(auxR);
                    }
                }

                apartment = apartments.FirstOrDefault(x => x.IdApartment == apart);

                apartmentsSelect.Add(new Apartment() { IdApartment = apartment.IdApartment, ApartmentNumber = apartment.ApartmentNumber });
            }

            foreach (var item in progressReports)
            {
                if (item.ProgressLogs != null && item.ProgressLogs.Count() > 0)
                {
                    auxL = item.ProgressLogs.OrderByDescending(x => x.DateCreated).FirstOrDefault();

                    progressLogs.Add(new ProgressLog() { IdProgressLog = auxL.IdProgressLog, IdProgressReport = auxL.IdProgressReport, DateCreated = auxL.DateCreated, IdStatus = auxL.IdStatus, Pieces = auxL.Pieces, Observation = auxL.Observation, IdSupervisor = auxL.IdSupervisor, IdBlobs = auxL.IdBlobs });
                }
            }
        }

        public async Task DivisionOfColors()
        {
            try
            {
                foreach (var item in progressReports)
                {
                    if (progressLogs.FirstOrDefault(x => x.IdProgressReport == item.IdProgressReport)?.IdProgressLog != null)
                    {
                        int auxGreen = Convert.ToInt16(progressLogs.FirstOrDefault(x => x.IdProgressReport == item.IdProgressReport).Pieces) * 100;

                        double auxResult = auxGreen / Convert.ToInt16(item.TotalPieces);

                        greenPercentage.Add($"{item.IdProgressReport}", auxResult.ToString("0.##"));

                        decimal auxRed = Convert.ToInt32(auxResult.ToString("0.##"));

                        redPercentage.Add($"{item.IdProgressReport}", (100 - auxRed).ToString());
                    }
                    else
                    {
                        greenPercentage.Add($"{item.IdProgressReport}", 0.ToString());

                        redPercentage.Add($"{item.IdProgressReport}", 100.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public bool isContentPhoto(int id)
        {
            var aux = progressLogs.FirstOrDefault(x => x.IdProgressLog == id);
            if (aux is null) return false;
            if (aux.Observation != null && aux.IdBlobs.Count > 0)
            {
                return true;
            }

            return false;
        }
        public async Task CameraButton(int id)
        {
            await ShowMessage();

            ProgressLog aux = progressLogs.FirstOrDefault(x => x.IdProgressLog == id);

            if (aux.Observation != null)
            {
                observations = aux.Observation;
            }

            if (aux.IdBlobs != null)
            {
                foreach (var item in aux.IdBlobs)
                {
                    images.Add(item.Uri);
                }
            }

            if (observations != null && observations != "" || images.Count() > 0)
            {
                showModal = true;
            }
            else
            {
                _toastService.ShowToast<ToastImages>(new ToastInstanceSettings(5, false));
            }
        }

        public async Task NotificationImages()
        {
            _toastService.ShowToast<ToastImages>(new ToastInstanceSettings(5, false));
        }
    }
}
