using Blazored.Toast;
using Blazored.Toast.Services;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;
using Obra.Client.Components;
using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;
using System.Security.Cryptography.X509Certificates;

namespace Obra.Client.Pages
{
    public partial class DetailedSummaryByApartment
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

        private List<Activity> activitiesSelect { get; set; } = new();
        private List<Element> elementsSelect { get; set; } = new();
        private List<SubElement> subElementsSelect { get; set; } = new();
        private List<Apartment> apartmentsSelect { get; set; } = new();

        private List<DetalladoDepartamentos> detalladoDepartamentos { get; set; } = new();

        private string messageError = "";
        private bool alert = false;
        public bool activityDetails = true;
        public bool subElement = false;
        public bool department = false;
        private bool allApartments = false;
        private bool allSubElements = false;
        private bool allElements = false;
        private bool allActivities = false;
        private bool apartmentDetails = true;
        private bool buttonReport = false;

        private bool showModal { get; set; } = false;
        private bool loading { get; set; } = false;

        private string observations { get; set; } = "";
        private List<string> images { get; set; } = new();


        private bool _showPreviewFile { get; set; }
        private byte[] _bytesPreviewFile { get; set; }
        private const string PDF_FILE_NAME = "DetallePorActividad.pdf";
        private void ChangeOpenModalPreview() => _showPreviewFile = _showPreviewFile ? false : true;
        public DetailedSummaryByApartment(ApplicationContext context, IApartmentsService apartmentsService, IActivitiesService activitiesService, IAreasService areasService, IElementsService elementsService, ISubElementsService subElementsService, IProgressReportService progressReportService, IProgressLogsService progressLogsService, IReportsService reportesService, IJSRuntime jS, IToastService toastService)
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

                    List<Element> auxElements = await _elementsService.GetElementsAsync(id);

                    elements.AddRange(auxElements);

                    await ShowMessage();
                }
                else
                {
                    _idsActivitiesSelect.Remove(id);

                    allActivities = false;
                    allElements = false;
                    allSubElements = false;

                    List<int> auxIdsElements = new();
                    List<int> auxIdsSubElements = new();

                    if (_idsElementsSelect.Count() > 0)
                    {
                        foreach (var item in _idsElementsSelect)
                        {
                            var auxId = elements.FirstOrDefault(x => x.IdActivity.Equals(id) && x.IdElement.Equals(item))?.IdElement;

                            if (auxId != null)
                            {
                                int auxNotNull = (int)auxId;
                                auxIdsElements.Add(auxNotNull);
                            }
                        }

                        foreach (var item in auxIdsElements)
                        {
                            _idsElementsSelect.Remove(item);
                        }
                    }

                    List<Element> auxDeleteElements = new();

                    auxDeleteElements = elements.Where(x => x.IdActivity.Equals(id)).ToList();

                    foreach (var item in auxDeleteElements)
                    {
                        elements.Remove(item);
                    }

                    if (_idsSubElementsSelect.Count() > 0)
                    {
                        foreach (var aux in auxIdsElements)
                        {
                            foreach (var item in _idsSubElementsSelect)
                            {
                                var auxId = subElements.FirstOrDefault(x => x.IdElement.Equals(aux) && x.IdSubElement.Equals(item))?.IdSubElement;

                                if (auxId != null)
                                {
                                    int auxNotNull = (int)auxId;
                                    auxIdsSubElements.Add(auxNotNull);
                                }
                            }
                        }

                        foreach (var item in auxIdsSubElements)
                        {
                            _idsSubElementsSelect.Remove(item);
                        }
                    }

                    List<SubElement> auxDeleteSubElements = new();

                    foreach (var item in auxIdsElements)
                    {
                        auxDeleteSubElements.AddRange(subElements.Where(x => x.IdElement.Equals(item)).ToList());
                    }

                    foreach (var item in auxDeleteSubElements)
                    {
                        subElements.Remove(item);
                    }

                    if (_idsActivitiesSelect.Count() == 0)
                    {
                        _idsAparmentSelect.Clear();
                        department = false;
                        allApartments = false;
                    }

                    if (_idsElementsSelect.Count() < 1)
                    {
                        _idsAparmentSelect.Clear();
                        department = false;
                        allApartments = false;
                    }
                }
            }
            else if (filter == 3) //Elemento
            {
                if (!_idsElementsSelect.Contains(id))
                {
                    _idsElementsSelect.Add(id);

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

                    allElements = false;
                    allSubElements = false;

                    List<int> auxIdsSubElements = new();

                    if (_idsSubElementsSelect.Count() > 0)
                    {
                        foreach (var item in _idsSubElementsSelect)
                        {
                            var auxId = subElements.FirstOrDefault(x => x.IdElement.Equals(id) && x.IdSubElement.Equals(item))?.IdSubElement;

                            if (auxId != null)
                            {
                                int auxNotNull = (int)auxId;
                                auxIdsSubElements.Add(auxNotNull);
                            }
                        }

                        foreach (var item in auxIdsSubElements)
                        {
                            _idsSubElementsSelect.Remove(item);
                        }
                    }

                    List<SubElement> auxDeleteSubElements = new();

                    auxDeleteSubElements = subElements.Where(x => x.IdElement.Equals(id)).ToList();

                    foreach (var item in auxDeleteSubElements)
                    {
                        subElements.Remove(item);
                    }

                    if (_idsSubElementsSelect.Count() == 0)
                    {
                        _idsAparmentSelect.Clear();
                        department = false;
                        allApartments = false;
                    }
                }
            }
            else if (filter == 4) //SubElemento
            {
                if (!_idsSubElementsSelect.Contains(id))
                {
                    _idsSubElementsSelect.Add(id);

                    department = true;

                    await ShowMessage();
                }
                else
                {
                    _idsSubElementsSelect.Remove(id);

                    allSubElements = false;

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

        public async Task AllActivities()
        {
            if (_idsActivitiesSelect.Count() < 1 && allActivities == false)
            {
                allActivities = true;

                if (allActivities == true)
                {
                    foreach (var item in activities)
                    {
                        _idsActivitiesSelect.Add(item.IdActivity);
                    }
                }

                elements = await _elementsService.GetElementsAsync(null);
            }
            else if (allActivities == true)
            {
                allActivities = false;

                _idsActivitiesSelect.Clear();
                _idsElementsSelect.Clear();
                _idsSubElementsSelect.Clear();
                _idsAparmentSelect.Clear();

                elements.Clear();
                subElements.Clear();

                allElements = false;
                allSubElements = false;
                allApartments = false;

                department = false;

                await ShowMessage();
            }
            else
            {
                allActivities = true;

                _idsActivitiesSelect.Clear();
                _idsElementsSelect.Clear();
                _idsSubElementsSelect.Clear();
                _idsAparmentSelect.Clear();

                elements.Clear();
                subElements.Clear();

                allElements = false;
                allSubElements = false;
                allApartments = false;

                department = false;

                if (allActivities == true)
                {
                    foreach (var item in activities)
                    {
                        _idsActivitiesSelect.Add(item.IdActivity);
                    }
                }

                elements = await _elementsService.GetElementsAsync(null);

                await ShowMessage();
            }
        }

        public async Task AllElements()
        {
            if (_idsElementsSelect.Count() < 1 && allElements == false)
            {
                allElements = true;

                if (allElements == true)
                {
                    foreach (var item in elements)
                    {
                        _idsElementsSelect.Add(item.IdElement);
                    }
                }

                subElements = await _subElementsService.GetSubElementsAsync(null);
            }
            else if (allElements == true)
            {
                allElements = false;

                _idsElementsSelect.Clear();
                _idsSubElementsSelect.Clear();
                _idsAparmentSelect.Clear();

                subElements.Clear();

                allSubElements = false;
                allApartments = false;

                department = false;

                await ShowMessage();
            }
            else
            {
                allElements = true;

                _idsElementsSelect.Clear();
                _idsSubElementsSelect.Clear();
                _idsAparmentSelect.Clear();

                subElements.Clear();

                allSubElements = false;
                allApartments = false;

                department = false;

                if (allElements == true)
                {
                    foreach (var item in elements)
                    {
                        _idsElementsSelect.Add(item.IdElement);
                    }
                }

                subElements = await _subElementsService.GetSubElementsAsync(null);

                await ShowMessage();
            }
        }

        public async Task AllSubElements()
        {
            if (_idsSubElementsSelect.Count() < 1 && allSubElements == false)
            {
                allSubElements = true;

                if (allSubElements == true)
                {
                    foreach (var idSubElement in subElements)
                    {
                        _idsSubElementsSelect.Add(idSubElement.IdSubElement);
                    }
                }

                department = true;
            }
            else if (allSubElements == true)
            {
                allSubElements = false;

                _idsSubElementsSelect.Clear();
            }
            else
            {
                allSubElements = true;

                _idsSubElementsSelect.Clear();

                if (_idsElementsSelect.Count == 0)
                {
                    _idsAparmentSelect.Clear();
                    allApartments = false;
                    department = false;
                }

                if (allSubElements == true)
                {
                    foreach (var idSubElement in subElements)
                    {
                        _idsSubElementsSelect.Add(idSubElement.IdSubElement);
                    }
                }

                department = true;
            }
        }

        public async Task AllApartments()
        {
            if (_idsAparmentSelect.Count() < 1 && allApartments == false)
            {
                allApartments = true;
            }
            else if (allApartments == true)
            {
                allApartments = false;

                _idsAparmentSelect.Clear();

                await ShowMessage();
            }
            else
            {
                allApartments = true;

                _idsAparmentSelect.Clear();

                await ShowMessage();
            }
        }

        public async Task ShowMessage() => alert = false;
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

            elements.Clear();
            _idsElementsSelect.Clear();

            subElements.Clear();
            _idsSubElementsSelect.Clear();

            buttonReport = false;
            apartmentDetails = true;

            subElementsSelect.Clear();
            apartmentsSelect.Clear();
            activitiesSelect.Clear();
            elementsSelect.Clear();

            allApartments = false;
            allSubElements = false;
            allElements = false;
            allActivities = false;

            detalladoDepartamentos.Clear();
        }

        public async Task ChangeView()
        {
            await ShowMessage();
            loading = true;
            buttonReport = false;

            var pdf = await _reportesService.PostReporteDetallesPorDepartamento(detalladoDepartamentos, null);

            if (pdf != null)
            {
                //_bytesPreviewFile = pdf;
                loading = false;
                //_showPreviewFile = true;
                await _JS.InvokeVoidAsync("OpenInNewPagePDF", pdf);
                StateHasChanged();
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
            apartmentDetails = false;
            loading = true;

            if (allApartments == true)
            {
                foreach (var item in apartments)
                {
                    _idsAparmentSelect.Add(item.IdApartment);
                }
            }

            foreach (var item in _idsAparmentSelect)
            {
                apartmentsSelect.Add(apartments.FirstOrDefault(x => x.IdApartment.Equals(item)));
            }

            foreach (var item in _idsActivitiesSelect)
            {
                activitiesSelect.Add(activities.FirstOrDefault(x => x.IdActivity.Equals(item)));
            }

            foreach (var item in _idsElementsSelect)
            {
                elementsSelect.Add(elements.FirstOrDefault(x => x.IdElement.Equals(item)));
            }

            foreach (var item in _idsSubElementsSelect)
            {
                subElementsSelect.Add(subElements.FirstOrDefault(x => x.IdSubElement.Equals(item)));
            }

            ActivitiesDetail data = new();
            data.IdBuilding = 1;
            data.Apartments = _idsAparmentSelect;
            data.Activities = _idsActivitiesSelect;
            data.Elements = _idsElementsSelect;
            data.SubElements = _idsSubElementsSelect;

            detalladoDepartamentos = await _reportesService.PostDataDetallesDepartamentos(data);

            loading = false;
        }

        public async Task CameraButton(int? idProgressLog)
        {
            await ShowMessage();

            if (idProgressLog != null)
            {
                int auxId = (int)idProgressLog;
                ProgressLog aux = await _progressLogsService.GetProgressLogAsync(auxId);

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
        }

        public async Task NotificationImages()
        {
            _toastService.ShowToast<ToastImages>(new ToastInstanceSettings(5, false));
        }
    }
}
