using Blazored.Toast;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
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
        private readonly IObjectAccessService _accessService;
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
        private bool showActivities = false;
        private bool withoutSubelements = false;
        private int? statusOption = null;
        private string _status1 = "Not Started";
        private string _status2 = "Started";
        private string _status3 = "Finished";
        public ObjectAccessUser Accesos { get; private set; }

        private bool showModal { get; set; } = false;
        private bool loading { get; set; } = false;

        private string observations { get; set; } = "";
        private List<string> images { get; set; } = new();


        private bool _showPreviewFile { get; set; }
        private byte[] _bytesPreviewFile { get; set; }
        private const string PDF_FILE_NAME = "DetallePorActividad.pdf";
        private void ChangeOpenModalPreview() => _showPreviewFile = _showPreviewFile ? false : true;
        public DetailedSummaryByApartment(ApplicationContext context, IApartmentsService apartmentsService, IActivitiesService activitiesService, IAreasService areasService, IElementsService elementsService, ISubElementsService subElementsService, IProgressReportService progressReportService,
            IProgressLogsService progressLogsService, IReportsService reportesService, IJSRuntime jS, IToastService toastService, IObjectAccessService accessService)
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
            _accessService = accessService;
        }

        protected async override Task OnInitializedAsync()
        {
            Accesos = await _accessService.GetObjectAccess();
            var apartmentsId = Accesos.Apartments.Select(x => x.IdApartment);
            var activitiesId = Accesos.Activities.Select(x => x.IdActivity);
            var areasId = Accesos.Areas.Select(x => x.IdArea);
            apartments = await _apartmentsService.GetApartmentsAsync();
            apartments = apartments.Where(x => apartmentsId.Contains(x.IdApartment)).ToList();
            activities = await _activitiesService.GetActivitiesAsync();
            activities = activities.Where(x => activitiesId.Contains(x.IdActivity)).ToList();
            Areas = await _areasService.GetAreasAsync();
            Areas = Areas.Where(x => areasId.Contains(x.IdArea)).ToList();
            var resultStatuses = await _accessService.GetStatuses();
            if (resultStatuses != null)
            {
                _status1 = resultStatuses.ElementAtOrDefault(0) == null ? _status1 : resultStatuses.ElementAt(0).StatusName;
                _status2 = resultStatuses.ElementAtOrDefault(1) == null ? _status2 : resultStatuses.ElementAt(1).StatusName;
                _status3 = resultStatuses.ElementAtOrDefault(2) == null ? _status3 : resultStatuses.ElementAt(2).StatusName;
            }
        }

        public async Task AddIdSelect(int id, int filter)
        {
            if (filter == 1) //Departemento
            {
                if (!_idsAparmentSelect.Contains(id))
                {
                    _idsAparmentSelect.Add(id);
                    if (apartments.Count == _idsAparmentSelect.Count)
                        allApartments = true;
                    else
                        allApartments = false;

                    if (allApartments && (allActivities || _idsActivitiesSelect.Count > 3))
                    {
                        allApartments = false;
                        _idsAparmentSelect.Remove(id);
                        _toastService.ShowWarning("Se deben seleccionar máximo 3 Actividades si elige todos los Departamentos", "Advertencia");
                        return;
                    }

                    showActivities= true;
                    await ShowMessage();
                }
                else
                {
                    _idsAparmentSelect.Remove(id);
                    allApartments = false;
                    allElements = false;
                    allSubElements = false;                    
                    if (_idsAparmentSelect.Count == 0)
                    {
                        allActivities = false;
                        showActivities = false;
                        _idsActivitiesSelect.Clear();
                        _idsElementsSelect.Clear();
                        _idsSubElementsSelect.Clear();
                        elements.Clear();
                        subElements.Clear();
                    }                    
                }
            }
            else if (filter == 2) //Actividad
            {
                if (!_idsActivitiesSelect.Contains(id))
                {
                    if (allApartments && _idsActivitiesSelect.Count == 3)
                    {
                        _toastService.ShowWarning("Se deben seleccionar máximo 3 Actividades si eligió todos los Departamentos", "Advertencia");
                        return;
                    }
                    _idsActivitiesSelect.Add(id);                    
                    if(activities.Count == _idsActivitiesSelect.Count)
                        allActivities = true;
                    else
                        allActivities = false;

                    List<Element> auxElements = await _elementsService.GetElementsAsync(id);
                    var elementsId = Accesos.Elements.Select(x => x.IdElement);
                    auxElements = auxElements.Where(x => elementsId.Contains(x.IdElement)).ToList();
                    elements.AddRange(auxElements);

                    
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

                    //if (_idsActivitiesSelect.Count() == 0)
                    //{
                    //    _idsAparmentSelect.Clear();
                    //    department = false;
                    //    allApartments = false;
                    //}

                    //if (_idsElementsSelect.Count() < 1)
                    //{
                    //    _idsAparmentSelect.Clear();
                    //    department = false;
                    //    allApartments = false;
                    //}
                }
            }
            else if (filter == 3) //Elemento
            {
                if (!_idsElementsSelect.Contains(id))
                {
                    _idsElementsSelect.Add(id);

                    List<SubElement> auxSubElement = await _subElementsService.GetSubElementsAsync(id);
                    var subElementsId = Accesos.SubElements.Select(x => x.IdSubElement);
                    auxSubElement = auxSubElement.Where(x => subElementsId.Contains(x.IdSubElement)).ToList();
                    subElements.AddRange(auxSubElement);

                    if (subElements == null || subElements.Count() < 1)
                    {
                        withoutSubelements = true;
                        department = true;
                    }
                    else
                        withoutSubelements = false;

                    
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
                    if (subElements == null || subElements.Count() < 1)
                        withoutSubelements = true;
                    else
                        withoutSubelements = false;
                    //if (_idsSubElementsSelect.Count() == 0)
                    //{
                    //    _idsAparmentSelect.Clear();
                    //    department = false;
                    //    allApartments = false;
                    //}
                }
            }
            else if (filter == 4) //SubElemento
            {
                if (!_idsSubElementsSelect.Contains(id))
                {
                    _idsSubElementsSelect.Add(id);

                    department = true;

                    
                }
                else
                {
                    _idsSubElementsSelect.Remove(id);

                    allSubElements = false;

                    

                    //if (_idsSubElementsSelect.Count() < 1)
                    //{
                    //    department = false;
                    //    allApartments = false;
                    //    _idsAparmentSelect.Clear();
                    //}
                }
            }
        }

        public async Task AllActivities()
        {
            if (_idsActivitiesSelect.Count() < 1 && allActivities == false)
            {
                if (allApartments)
                {
                    _toastService.ShowWarning("Se deben seleccionar máximo 3 Actividades si eligió todos los Departamentos", "Advertencia");
                    return;
                }

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
                //_idsAparmentSelect.Clear();

                elements.Clear();
                subElements.Clear();

                allElements = false;
                allSubElements = false;
                //allApartments = false;

                department = false;

                
            }
            else
            {
                if (allApartments)
                {
                    _toastService.ShowWarning("Se deben seleccionar máximo 3 Actividades si eligió todos los Departamentos", "Advertencia");
                    return;
                }

                allActivities = true;

                _idsActivitiesSelect.Clear();
                _idsElementsSelect.Clear();
                _idsSubElementsSelect.Clear();
                //_idsAparmentSelect.Clear();

                elements.Clear();
                subElements.Clear();

                allElements = false;
                allSubElements = false;
                //allApartments = false;

                department = false;

                if (allActivities == true)
                {
                    foreach (var item in activities)
                    {
                        _idsActivitiesSelect.Add(item.IdActivity);
                    }
                }

                elements = await _elementsService.GetElementsAsync(null);

                
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
                //_idsAparmentSelect.Clear();

                subElements.Clear();

                allSubElements = false;
                //allApartments = false;

                department = false;

                
            }
            else
            {
                allElements = true;

                _idsElementsSelect.Clear();
                _idsSubElementsSelect.Clear();
                //_idsAparmentSelect.Clear();

                subElements.Clear();

                allSubElements = false;
                //allApartments = false;

                department = false;

                if (allElements == true)
                {
                    foreach (var item in elements)
                    {
                        _idsElementsSelect.Add(item.IdElement);
                    }
                }

                subElements = await _subElementsService.GetSubElementsAsync(null);

                
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

                //if (_idsElementsSelect.Count == 0)
                //{
                //    _idsAparmentSelect.Clear();
                //    allApartments = false;
                //    department = false;
                //}

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
                if (allApartments == true)
                    foreach (var apartment in apartments)
                    {
                        _idsAparmentSelect.Add(apartment.IdApartment);
                    }

                if (allActivities || _idsActivitiesSelect.Count > 3)
                {
                    allApartments = false;
                    _toastService.ShowWarning("Se deben seleccionar máximo 3 Actividades si elige todos los Departamentos", "Advertencia");
                    return;
                }

                showActivities = true;
            }
            else if (allApartments == true)
            {
                allApartments = false;
                showActivities= false;
                _idsAparmentSelect.Clear();
                _idsActivitiesSelect.Clear();
                _idsElementsSelect.Clear();
                _idsSubElementsSelect.Clear();
                elements.Clear();
                subElements.Clear();

                
            }
            else
            {
                allApartments = true;
                showActivities = true;
                //_idsAparmentSelect.Clear();

                if (allActivities || _idsActivitiesSelect.Count > 3)
                {
                    allApartments = false;
                    _toastService.ShowWarning("Se deben seleccionar máximo 3 Actividades si elige todos los Departamentos", "Advertencia");
                    return;
                }

                if (allApartments == true)
                    foreach (var apartment in apartments)
                    {
                        _idsAparmentSelect.Add(apartment.IdApartment);
                    }                

                
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
            showActivities= false;
            statusOption = null;

            detalladoDepartamentos.Clear();
        }

        public async Task ChangeView()
        {
            
            loading = true;
            buttonReport = false;

            var pdf = await _reportesService.PostReporteDetallesPorDepartamento(detalladoDepartamentos, statusOption);

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
            statusOption = null;
            loading = false;
            buttonReport = true;
        }

        public async Task ShowReportAndHideApartment()
        {

            if (_idsAparmentSelect.Count == 0)
            {
                _toastService.ShowError("Es necesario elegir un departamento antes", "¡Error!");
                return;
            }
            if (_idsActivitiesSelect.Count == 0)
            {
                _toastService.ShowError("Es necesario elegir una actividad antes", "¡Error!");
                return;
            }
            if (_idsElementsSelect.Count == 0)
            {
                _toastService.ShowError("Es necesario elegir un elemento antes", "¡Error!");
                return;
            }
            if (_idsSubElementsSelect.Count == 0 && !withoutSubelements)
            {
                _toastService.ShowError("Es necesario elegir un subelemento antes", "¡Error!");
                return;
            }
            var x = statusOption;
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
            data.IdBuilding = Accesos.IdBuilding;
            data.Apartments = _idsAparmentSelect;
            data.Activities = _idsActivitiesSelect;
            data.Elements = _idsElementsSelect;
            data.SubElements = _idsSubElementsSelect;

            detalladoDepartamentos = await _reportesService.PostDataDetallesDepartamentos(data);

            loading = false;
                
        }

        public async Task CameraButton(int? idProgressLog)
        {
            
            
            if (idProgressLog != null)
            {
                int contador = 0;
                int auxId = (int)idProgressLog;
                var currentProgressReport = await _progressLogsService.GetProgressLogAsync(auxId);
                var logsByProgressReport = await _progressLogsService.GetProgressLogsAsync(idProgressReport: currentProgressReport.IdProgressReport);
                List<string> listUris = new List<string>();
                if (logsByProgressReport != null)
                {
                    logsByProgressReport = logsByProgressReport.OrderByDescending(x => x.IdProgressLog).ToList();
                    foreach (var log in logsByProgressReport)
                    {
                        var currentBlob = log.IdBlobs.FirstOrDefault();
                        string? currentUri = currentBlob == null ? null : currentBlob.Uri;
                        if (currentUri != null)
                        {
                            listUris.Add(currentUri);
                            contador++;
                        }
                        if (contador == 3)
                            break;
                    }
                }

                ProgressLog aux = await _progressLogsService.GetProgressLogAsync(auxId);

                if (aux.Observation != null)
                {
                    observations = aux.Observation;
                }

                //if (aux.IdBlobs != null)
                //{
                //    foreach (var item in aux.IdBlobs)
                //    {
                //        images.Add(item.Uri);
                //    }
                //}
                if (listUris != null)
                {
                    foreach (var item in listUris)
                    {
                        images.Add(item);
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
            else
            {
                _toastService.ShowToast<ToastImages>(new ToastInstanceSettings(5, false));
            }
        }

        public async Task NotificationImages()
        {
            _toastService.ShowToast<ToastImages>(new ToastInstanceSettings(5, false));
        }

        public void CheckboxClicked(int? idStatus, ChangeEventArgs e)
        {
            statusOption = idStatus;
        }
    }
}
