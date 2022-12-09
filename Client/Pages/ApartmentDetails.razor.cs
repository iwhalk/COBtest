using Microsoft.AspNetCore.Components;
using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;

namespace Obra.Client.Pages
{
    public partial class ApartmentDetails : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly IApartmentsService _apartmentsService;
        private readonly IActivitiesService _activitiesService;
        private readonly IElementsService _elementsService;
        private readonly ISubElementsService _subElementsService;
        private readonly IProgressReportService _progressReportService;
        private readonly IProgressLogsService _progressLogsService;

        private List<Apartment> apartments { get; set; }
        private List<SharedLibrary.Models.Activity> activities { get; set; }
        private List<Element> elements { get; set; }
        private List<SubElement> subElements { get; set; }
        private List<ProgressReport> progressReports { get; set; }
        private List<ProgressLog> progressLogs { get; set; }

        private List<SubElement> subElementsAux { get; set; } = new();
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

        private bool isFirstView { get; set; } = true;
        private bool showModal { get; set; } = false;

        public ApartmentDetails(ApplicationContext context, IApartmentsService apartmentsService, IActivitiesService activitiesService, IElementsService elementsService, ISubElementsService subElementsService, IProgressReportService progressReportService, IProgressLogsService progressLogsService)
        {
            _context = context;
            _apartmentsService = apartmentsService;
            _activitiesService = activitiesService;
            _elementsService = elementsService;
            _subElementsService = subElementsService;
            _progressReportService = progressReportService;
            _progressLogsService = progressLogsService;
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

                    ShowMenssage();
                }
                else if (_idsAparmentSelect.Count() == 1)
                {
                    _idsAparmentSelect.Remove(id);

                    _idsActivitiesSelect.Clear();

                    if (elements != null)
                    {
                        ShowElements();
                        elements.Clear();
                        _idsElementsSelect.Clear();

                        if (subElements != null)
                        {
                            ShowSubElements();
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

                            ShowMenssage();
                        }
                        else
                        {
                            _idsActivitiesSelect.Remove(id);

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

                        if (elements != null)
                        {
                            ShowElements();
                            elements.Clear();
                            _idsElementsSelect.Clear();

                            if (subElements != null)
                            {
                                ShowSubElements();
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

                        ShowMenssage();
                    }
                    else
                    {
                        _idsElementsSelect.Remove(id);

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
                        ShowSubElements();
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
                }
                else
                    _idsSubElementsSelect.Remove(id);
            }
        }

        public async Task ShowMenssage() => alert = false;
        public async Task ShowElements() => showElements = false;
        public async Task ShowSubElements() => showSubElements = false;

        private void ChangeShowModal() => showModal = showModal ? false : true;
        private void ChangeView() => isFirstView = isFirstView ? false : true;

        public async Task ShowReportAndHideApartment()
        {
            if (_idsElementsSelect != null && _idsElementsSelect.Count() > 0 && _idsSubElementsSelect.Count() > 1)
            {
                activity = activities.FirstOrDefault(x => x.IdActivity == _idsActivitiesSelect.FirstOrDefault());
                element = elements.FirstOrDefault(x => x.IdElement == _idsElementsSelect.FirstOrDefault());


                foreach (var item in _idsSubElementsSelect)
                {
                    SubElement subElement = await _subElementsService.GetSubElementAsync(item);

                    subElementsAux.Add(new SubElement() { IdSubElement = subElement.IdSubElement, SubElementName = subElement.SubElementName, IdElement = subElement.IdElement, Type = subElement.Type });
                }

                AdvancementProgress();

                buttonReport = true;
                apartmentDetails = false;
            }
            else if (_idsElementsSelect != null && _idsElementsSelect.Count() > 0 && _idsSubElementsSelect.Count() < 1)
            {
                activity = activities.FirstOrDefault(x => x.IdActivity == _idsActivitiesSelect.FirstOrDefault());
                element = elements.FirstOrDefault(x => x.IdElement == _idsElementsSelect.FirstOrDefault());

                buttonReport = true;
                apartmentDetails = false;
                subElementsNulls = true;
            }
            else
            {
                menssageError = "Para generar el reporte es necesario elegir un elemento antes";
                alert = true;
            }
        }

        public async Task AdvancementProgress()
        {
            foreach (var idAparment in _idsAparmentSelect)
            {
                foreach (var idsSubElement in _idsSubElementsSelect)
                {
                    ProgressReport progressReport = (await _progressReportService.GetProgressReportsAsync(idAparment: idAparment, idElemnet: _idsElementsSelect.FirstOrDefault(), idSubElement: idsSubElement)).OrderByDescending(x => x.DateCreated).FirstOrDefault();

                    progressReports.Add(new ProgressReport() { IdProgressReport = progressReport.IdProgressReport, DateCreated = progressReport.DateCreated, IdBuilding = progressReport.IdBuilding, IdApartment = progressReport.IdApartment, IdArea = progressReport.IdArea, IdElement = progressReport.IdElement, IdSubElement = progressReport.IdSubElement, TotalPieces = progressReport.TotalPieces, IdSupervisor = progressReport.IdSupervisor });
                }
            }

            foreach (var item in progressReports)
            {
                ProgressLog progressLog = (await _progressLogsService.GetProgressLogsAsync(idProgressReport: item.IdProgressReport)).FirstOrDefault();

                progressLogs.Add(new ProgressLog() { IdProgressLog = progressLog.IdProgressLog, IdProgressReport = progressLog.IdProgressReport, DateCreated = progressLog.DateCreated, IdStatus = progressLog.IdStatus, Pieces = progressLog.Pieces, Observation = progressLog.Observation, IdSupervisor = progressLog.IdSupervisor });
            }
        }
    }
}
