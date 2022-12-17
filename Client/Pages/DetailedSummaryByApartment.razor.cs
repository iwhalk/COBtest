﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;

namespace Obra.Client.Pages
{
    public partial class DetailedSummaryByApartment : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly IApartmentsService _apartmentsService;
        private readonly IActivitiesService _activitiesService;
        private readonly IElementsService _elementsService;
        private readonly ISubElementsService _subElementsService;
        private readonly IProgressReportService _progressReportService;
        private readonly IProgressLogsService _progressLogsService;
        private readonly IReportesService _reportesService;

        private List<Apartment> apartments { get; set; }
        private List<SharedLibrary.Models.Activity> activities { get; set; }
        private List<Element> elements { get; set; }
        private List<SubElement> subElements { get; set; }
        private List<ProgressReport> progressReports { get; set; } = new();
        private List<ProgressLog> progressLogs { get; set; } = new();
        private readonly IJSRuntime _JS;

        private List<SubElement> subElementsSelect { get; set; } = new();
        private List<Apartment> apartmentsSelect { get; set; } = new();
        private SharedLibrary.Models.Activity activity { get; set; }
        private Element element { get; set; }

        private List<int> _idsAparmentSelect { get; set; } = new();
        private List<int> _idsActivitiesSelect { get; set; } = new();
        private List<int> _idsElementsSelect { get; set; } = new();
        private List<int> _idsSubElementsSelect { get; set; } = new();

        private string menssageError = "";
        private bool alert = false;
        private int activityIdAux = 0;
        private int elementIdAux = 0;

        private bool showElements = false;
        private bool showSubElements = false;
        private bool apartmentDetails = true;
        private bool buttonReport = false;
        private bool subElementsNulls = false;

        private bool loading { get; set; } = false;

        private string observations = "";
        private List<string> images = new();

        Dictionary<string, string> greenPercentage { get; set; } = new();
        Dictionary<string, string> redPercentage { get; set; } = new();

        private bool isFirstView { get; set; } = true;
        private bool showModal { get; set; } = false;

        public DetailedSummaryByApartment(ApplicationContext context, IApartmentsService apartmentsService, IActivitiesService activitiesService, IElementsService elementsService, ISubElementsService subElementsService, IProgressReportService progressReportService, IProgressLogsService progressLogsService, IReportesService reportesService, IJSRuntime jS)
        {
            _context = context;
            _apartmentsService = apartmentsService;
            _activitiesService = activitiesService;
            _elementsService = elementsService;
            _subElementsService = subElementsService;
            _progressReportService = progressReportService;
            _progressLogsService = progressLogsService;
            _reportesService = reportesService;
            _JS = jS;
        }

        protected async override Task OnInitializedAsync()
        {
            apartments = await _apartmentsService.GetApartmentsAsync();
            activities = await _activitiesService.GetActivitiesAsync();
        }

        public async Task AddIdSelect(int id, int filter)
        {
            if (filter == 1) //Departemento
            {
                if (!_idsAparmentSelect.Contains(id))
                {
                    _idsAparmentSelect.Add(id);

                    await ShowMenssage();
                }
                else if (_idsAparmentSelect.Count() == 1)
                {
                    _idsAparmentSelect.Remove(id);

                    await ShowMenssage();

                    _idsActivitiesSelect.Clear();

                    if (elements != null)
                    {
                        await ShowElements();
                        elements.Clear();
                        _idsElementsSelect.Clear();

                        if (subElements != null)
                        {
                            await ShowSubElements();
                            subElements.Clear();
                            _idsSubElementsSelect.Clear();
                        }
                    }
                }
                else
                {
                    _idsAparmentSelect.Remove(id);
                }
            }
            else if (filter == 2) //Actividad
            {
                if (_idsAparmentSelect != null && _idsAparmentSelect.Count() > 0)
                {
                    if (_idsActivitiesSelect.Count() < 1)
                    {
                        if (!_idsActivitiesSelect.Contains(id))
                        {
                            _idsActivitiesSelect.Add(id);
                            activityIdAux = id;
                            showElements = true;
                            elements = await _elementsService.GetElementsAsync(id);

                            await ShowMenssage();
                        }
                        else
                        {
                            _idsActivitiesSelect.Remove(id);

                            await ShowMenssage();

                            if (elements != null)
                            {
                                elements.Clear();
                                _idsElementsSelect.Clear();

                                if (subElements != null)
                                {
                                    subElements.Clear();
                                    _idsSubElementsSelect.Clear();
                                }
                            }
                        }
                    }
                    else if (activityIdAux == id)
                    {
                        _idsActivitiesSelect.Remove(id);

                        await ShowMenssage();

                        if (elements != null)
                        {
                            await ShowElements();
                            elements.Clear();
                            _idsElementsSelect.Clear();

                            if (subElements != null)
                            {
                                await ShowSubElements();
                                subElements.Clear();
                                _idsSubElementsSelect.Clear();
                            }
                        }
                    }
                }
                else
                {
                    menssageError = "Es necesario elegir un un Departamento antes de una Actividad";
                    alert = true;
                }
            }
            else if (filter == 3) //Elemento
            {
                if (_idsElementsSelect.Count() < 1)
                {
                    if (!_idsElementsSelect.Contains(id))
                    {
                        _idsElementsSelect.Add(id);
                        elementIdAux = id;
                        showSubElements = true;
                        subElements = await _subElementsService.GetSubElementsAsync(id);

                        await ShowMenssage();
                    }
                    else
                    {
                        _idsElementsSelect.Remove(id);

                        await ShowMenssage();

                        if (subElements != null)
                        {
                            subElements.Clear();
                            _idsSubElementsSelect.Clear();
                        }
                    }
                }
                else if (elementIdAux == id)
                {
                    _idsElementsSelect.Remove(id);

                    if (subElements != null)
                    {
                        await ShowSubElements();
                        subElements.Clear();
                        _idsSubElementsSelect.Clear();
                    }
                }
            }
            else if (filter == 4) //SubElemento
            {
                if (!_idsSubElementsSelect.Contains(id))
                {
                    _idsSubElementsSelect.Add(id);

                    await ShowMenssage();
                }
                else
                {

                    _idsSubElementsSelect.Remove(id);

                    await ShowMenssage();
                }
            }
        }

        public async Task AllApartments()
        {
            if (_idsAparmentSelect.Count() < 1)
            {
                foreach (var idAparment in apartments)
                {
                    _idsAparmentSelect.Add(idAparment.IdApartment);
                }
            }
            else
            {
                _idsAparmentSelect.Clear();

                await ShowMenssage();

                _idsActivitiesSelect.Clear();

                if (elements != null)
                {
                    await ShowElements();
                    elements.Clear();
                    _idsElementsSelect.Clear();

                    if (subElements != null)
                    {
                        await ShowSubElements();
                        subElements.Clear();
                        _idsSubElementsSelect.Clear();
                    }
                }
            }
        }

        public async Task AllSubElements()
        {
            if (_idsSubElementsSelect.Count() < 1)
            {
                foreach (var idSubElement in subElements)
                {
                    _idsSubElementsSelect.Add(idSubElement.IdSubElement);
                }
            }
            else
            {
                _idsSubElementsSelect.Clear();
            }
        }

        public async Task ShowMenssage() => alert = false;
        public async Task ShowElements() => showElements = false;
        public async Task ShowSubElements() => showSubElements = false;

        public async Task GoBack()
        {
            _idsAparmentSelect.Clear();
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

            greenPercentage.Clear();
            redPercentage.Clear();
        }

        public void ChangeShowModal()
        {
            showModal = showModal ? false : true;
            observations = "";
            images.Clear();
        }
        public async Task ChangeView()
        {
            loading = true;
            isFirstView = isFirstView ? false : true;

            ActivitiesDetail reporte = new();
            reporte.Activities = new();
            reporte.Elements = new();

            if (subElementsSelect != null)
            {
                reporte.IdBuilding = 1;
                reporte.Apartments = _idsAparmentSelect;
                reporte.Activities.Add(activity.IdActivity);
                reporte.Elements.Add(element.IdElement);
                reporte.SubElements = _idsSubElementsSelect;
            }
            else
            {
                reporte.IdBuilding = 1;
                reporte.Apartments = _idsAparmentSelect;
                reporte.Activities.Add(activity.IdActivity);
                reporte.Elements.Add(element.IdElement);
            }

            var pdf = await _reportesService.PostReporteDetallesAsync(reporte);

            if (pdf != null)
            {
                var fileName = "DetallePorDepartemento.pdf";
                var fileStream = new MemoryStream(pdf);
                using var streamRef = new DotNetStreamReference(stream: fileStream);
                await _JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            }

            loading = false;
        }

        public async Task ShowReportAndHideApartment()
        {
            loading = true;

            if (_idsElementsSelect != null && _idsElementsSelect.Count() > 0 && _idsSubElementsSelect.Count() >= 1)
            {
                activity = activities.FirstOrDefault(x => x.IdActivity == _idsActivitiesSelect.FirstOrDefault());
                element = elements.FirstOrDefault(x => x.IdElement == _idsElementsSelect.FirstOrDefault());


                foreach (var item in _idsSubElementsSelect)
                {
                    SubElement subElement = subElements.FirstOrDefault(x => x.IdSubElement == item);

                    subElementsSelect.Add(new SubElement() { IdSubElement = subElement.IdSubElement, SubElementName = subElement.SubElementName, IdElement = subElement.IdElement, Type = subElement.Type });
                }

                await AdvancementProgressSubElements();

                if (alert == false)
                {
                    await DivisionOfColors();

                    buttonReport = true;
                    apartmentDetails = false;
                    loading = false;
                }
            }
            else if (_idsElementsSelect != null && _idsElementsSelect.Count() > 0 && _idsSubElementsSelect.Count() < 1)
            {
                activity = activities.FirstOrDefault(x => x.IdActivity == _idsActivitiesSelect.FirstOrDefault());
                element = elements.FirstOrDefault(x => x.IdElement == _idsElementsSelect.FirstOrDefault());

                await AdvancementProgress();

                if (alert == false)
                {
                    await DivisionOfColors();

                    buttonReport = true;
                    apartmentDetails = false;
                    subElementsNulls = true;
                    loading = false;
                }
            }
            else
            {
                menssageError = "Para generar el reporte es necesario elegir un Elemento antes";
                alert = true;
                loading = false;
            }
        }

        public async Task AdvancementProgressSubElements()
        {
            //Lo nuevo super rapido alv

            foreach (var apart in _idsAparmentSelect)
            {
                List<ProgressReport> progresses = await _progressReportService.GetProgressReportsAsync(idBuilding: 1, idApartment: apart, includeProgressLogs: true);

                foreach (var act in activity.IdAreas)
                {
                    foreach (var sub in _idsSubElementsSelect)
                    {
                        ProgressReport aux = progresses.OrderByDescending(x => x.DateCreated).FirstOrDefault(x => x.IdArea == act.IdArea && x.IdElement == _idsElementsSelect.FirstOrDefault() && x.IdSubElement == sub);

                        if (aux != null)
                            progressReports.Add(aux);
                    }
                }

                Apartment apartment = apartments.FirstOrDefault(x => x.IdApartment == apart);

                apartmentsSelect.Add(new Apartment() { IdApartment = apartment.IdApartment, ApartmentNumber = apartment.ApartmentNumber });
            }

            foreach (var item in progressReports)
            {
                if (item.ProgressLogs != null && item.ProgressLogs.Count() > 0)
                {
                    ProgressLog aux = item.ProgressLogs.OrderByDescending(x => x.DateCreated).FirstOrDefault();

                    progressLogs.Add(new ProgressLog() { IdProgressLog = aux.IdProgressLog, IdProgressReport = aux.IdProgressReport, DateCreated = aux.DateCreated, IdStatus = aux.IdStatus, Pieces = aux.Pieces, Observation = aux.Observation, IdSupervisor = aux.IdSupervisor, IdBlobs = aux.IdBlobs });
                }
            }

            //Lo viejo lento pero sirve
            //foreach (var idAparment in _idsAparmentSelect)
            //{
            //    foreach (var idsSubElement in _idsSubElementsSelect)
            //    {
            //        List<ProgressReport> progressReport = await _progressReportService.GetProgressReportsAsync(idBuilding: 1, idAparment: idAparment, idElemnet: _idsElementsSelect.FirstOrDefault(), idSubElement: idsSubElement);

            //        if (progressReport != null)
            //        {
            //            foreach (var item in progressReport)
            //            {
            //                progressReports.Add(new ProgressReport() { IdProgressReport = item.IdProgressReport, DateCreated = item.DateCreated, IdBuilding = item.IdBuilding, IdApartment = item.IdApartment, IdArea = item.IdArea, IdElement = item.IdElement, IdSubElement = item.IdSubElement, TotalPieces = item.TotalPieces, IdSupervisor = item.IdSupervisor });
            //            }
            //        }
            //    }

            //    if (progressReports != null && progressReports.Count() > 0)
            //    {
            //        foreach (var item in progressReports)
            //        {
            //            ProgressLog progressLog = (await _progressLogsService.GetProgressLogsAsync(idProgressReport: item.IdProgressReport)).OrderByDescending(x => x.DateCreated).FirstOrDefault();

            //            if (progressLog != null)
            //            {
            //                progressLogs.Add(new ProgressLog() { IdProgressLog = progressLog.IdProgressLog, IdProgressReport = progressLog.IdProgressReport, DateCreated = progressLog.DateCreated, IdStatus = progressLog.IdStatus, Pieces = progressLog.Pieces, Observation = progressLog.Observation, IdSupervisor = progressLog.IdSupervisor, IdBlobs = progressLog.IdBlobs });
            //            }
            //        }
            //    }

            //}
        }

        public async Task AdvancementProgress()
        {
            //Lo nuevo super rapido alv

            foreach (var apart in _idsAparmentSelect)
            {
                List<ProgressReport> progresses = await _progressReportService.GetProgressReportsAsync(idBuilding: 1, idApartment: apart, includeProgressLogs: true);

                foreach (var act in activity.IdAreas)
                {
                    ProgressReport aux = progresses.OrderByDescending(x => x.DateCreated).FirstOrDefault(x => x.IdArea == act.IdArea && x.IdElement == _idsElementsSelect.FirstOrDefault());

                    if (aux != null)
                        progressReports.Add(aux);
                }

                Apartment apartment = apartments.FirstOrDefault(x => x.IdApartment == apart);

                apartmentsSelect.Add(new Apartment() { IdApartment = apartment.IdApartment, ApartmentNumber = apartment.ApartmentNumber });
            }

            foreach (var item in progressReports)
            {
                if (item.ProgressLogs != null && item.ProgressLogs.Count() > 0)
                {
                    ProgressLog aux = item.ProgressLogs.OrderByDescending(x => x.DateCreated).FirstOrDefault();

                    progressLogs.Add(new ProgressLog() { IdProgressLog = aux.IdProgressLog, IdProgressReport = aux.IdProgressReport, DateCreated = aux.DateCreated, IdStatus = aux.IdStatus, Pieces = aux.Pieces, Observation = aux.Observation, IdSupervisor = aux.IdSupervisor, IdBlobs = aux.IdBlobs });
                }
            }

            //foreach (var idAparment in _idsAparmentSelect)
            //{
            //    List<ProgressReport> progressReport = await _progressReportService.GetProgressReportsAsync(idBuilding: 1, idAparment: idAparment, idElemnet: _idsElementsSelect.FirstOrDefault());

            //    if (progressReport != null)
            //    {
            //        foreach (var item in progressReport)
            //        {
            //            progressReports.Add(new ProgressReport() { IdProgressReport = item.IdProgressReport, DateCreated = item.DateCreated, IdBuilding = item.IdBuilding, IdApartment = item.IdApartment, IdArea = item.IdArea, IdElement = item.IdElement, IdSubElement = item.IdSubElement, TotalPieces = item.TotalPieces, IdSupervisor = item.IdSupervisor });
            //        }
            //    }

            //    foreach (var item in progressReports)
            //    {
            //        ProgressLog progressLog = (await _progressLogsService.GetProgressLogsAsync(idProgressReport: item.IdProgressReport)).OrderByDescending(x => x.DateCreated).FirstOrDefault();

            //        if (progressLog != null)
            //        {
            //            progressLogs.Add(new ProgressLog() { IdProgressLog = progressLog.IdProgressLog, IdProgressReport = progressLog.IdProgressReport, DateCreated = progressLog.DateCreated, IdStatus = progressLog.IdStatus, Pieces = progressLog.Pieces, Observation = progressLog.Observation, IdSupervisor = progressLog.IdSupervisor, IdBlobs = progressLog.IdBlobs });
            //        }
            //    }

            //    Apartment apartment = apartments.FirstOrDefault(x => x.IdApartment == idAparment);

            //    apartmentsSelect.Add(new Apartment() { IdApartment = apartment.IdApartment, ApartmentNumber = apartment.ApartmentNumber });
            //}
        }

        public async Task DivisionOfColors()
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

        public async Task CamareButton(int id)
        {
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

            showModal = true;
        }
    }
}