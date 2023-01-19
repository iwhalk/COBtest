using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Azure;
using Microsoft.JSInterop;
using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;
using static Obra.Client.Pages.ProgressOfApartmentsByActivity;

namespace Obra.Client.Pages
{
    public partial class ProgressOfActivitiesByApartment : ComponentBase
    {
        public int MyProperty { get; set; }
        private readonly ApplicationContext _context;
        private readonly IActivitiesService _activityService;
        private readonly IApartmentsService _apartmentService;
        private readonly NavigationManager _navigationManager;
        private Dictionary<int, List<InfoActivityIn>> _idsAparmentSelect { get; set; } = new();
        private readonly IProgressLogsService _progressLogsService;
        private readonly IReportsService _reportService;
        private readonly IJSRuntime _JS;
        //Variable locales        
        public bool _isLoadingProcess { get; set; }
        private bool _isFullAparment { get; set; }
        public ProgressOfActivitiesByApartment(ApplicationContext context, NavigationManager navigationManager, IActivitiesService activityService, IApartmentsService apartmentService, IProgressLogsService progressLogsService, IReportsService reportService, IJSRuntime jS)
        {
            _context = context;
            _activityService = activityService;
            _navigationManager = navigationManager;
            _apartmentService = apartmentService;
            _progressLogsService = progressLogsService;
            _reportService = reportService;
            _JS = jS;            
        }
        protected async override Task OnInitializedAsync()
        {
            _context.Activity = await _activityService.GetActivitiesAsync();
            _context.Apartment = await _apartmentService.GetApartmentsAsync();
            await _reportService.GetProgressOfActivityByAparmentDataViewAsync(null);
        }
        private void BackPage() => _navigationManager.NavigateTo("/ProjectOverview");
        private async void AddIdAparmentSelect(int idAparment)
        {
            _isLoadingProcess = true;
            if (!_idsAparmentSelect.ContainsKey(idAparment))
            {
                //change for real endpoint for this view
                var infoProgress = await _reportService.GetProgressOfActivityByAparmentDataViewAsync(idAparment);
                if (infoProgress != null)
                {
                    List<InfoActivityIn> listAparmentPorcentage = new List<InfoActivityIn>();
                    foreach (var item in infoProgress)
                    {
                        var porcentageProgress = Math.Round(item.ApartmentProgress, 2);
                        var porcentage = new Tuple<double, double>(porcentageProgress, 100 - porcentageProgress);
                        listAparmentPorcentage.Add(new InfoActivityIn { activityNumber = item.Activity_, aparmentNumber = item.ApartmentNumber, porcentage = porcentage });
                    }
                    _idsAparmentSelect.Add(idAparment, listAparmentPorcentage);
                }
                else
                {
                    var listAparmentPorcentage = new List<InfoActivityIn>();
                    foreach (var activity in _context.Activity)
                    {
                        listAparmentPorcentage.Add(new InfoActivityIn
                        {
                            activityNumber = activity.ActivityName,
                            porcentage = new Tuple<double, double>(0.0, 100.0)
                        });
                    }
                    _idsAparmentSelect.Add(idAparment, listAparmentPorcentage);
                }
            }
            else
            {
                _idsAparmentSelect = _idsAparmentSelect.Where(x => x.Key != idAparment).Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key, x => x.Value);
            }
            _isLoadingProcess = false;
            StateHasChanged();
        }
        private async void FullActivity()
        {
            _isLoadingProcess = true;
            if (_idsAparmentSelect.Count() == _context.Activity.Count())
            {
                _isFullAparment = false;
                _idsAparmentSelect.Clear();
            }
            else
            {
                _idsAparmentSelect.Clear();
                var infoProgress = await _reportService.GetProgressOfAparmentByActivityDataViewAsync(null);
                if (infoProgress != null)
                {

                    foreach (var apartment in _context.Apartment)
                    {
                        if (infoProgress.Any(x => x.ApartmentNumber == apartment.ApartmentNumber))
                        {
                            List<InfoActivityIn> listAparmentPorcentage = new List<InfoActivityIn>();
                            foreach (var item in infoProgress.Where(x => x.ApartmentNumber == apartment.ApartmentNumber))
                            {
                                var porcentageProgress = Math.Round(item.ApartmentProgress, 2);
                                var porcentage = new Tuple<double, double>(porcentageProgress, 100 - porcentageProgress);
                                listAparmentPorcentage.Add(new InfoActivityIn { activityNumber = item.Activity_, aparmentNumber = item.ApartmentNumber, porcentage = porcentage });
                            }
                            _idsAparmentSelect.Add(apartment.IdApartment, listAparmentPorcentage);
                        }
                        else
                        {
                            var listAparmentPorcentage = new List<InfoActivityIn>();
                            foreach (var aparment in _context.Activity)
                            {
                                listAparmentPorcentage.Add(new InfoActivityIn
                                {
                                    activityNumber = aparment.ActivityName,
                                    porcentage = new Tuple<double, double>(0.0, 100.0)
                                });
                            }
                            _idsAparmentSelect.Add(apartment.IdApartment, listAparmentPorcentage);
                        }
                    }
                }
            }
            _isLoadingProcess = false;
            StateHasChanged();
        }
        private async void GeneratePDfPorgressaprment()
        {
            var listAparmentProgress = new List<ActivityProgressByAparment>();

            foreach (var item in _idsAparmentSelect)
            {
                foreach (var activity in item.Value)
                {
                    listAparmentProgress.Add(new ActivityProgressByAparment
                    {
                        //Activity_ = _context.Activity.Find(x => x.ActivityName == item.).ActivityName,
                        Activity_ = activity.activityNumber,
                        ApartmentNumber = activity.aparmentNumber,
                        ApartmentProgress = activity.porcentage.Item1
                        
                    });
                }
            }

            var bytesForPDF = await _reportService.PostProgressOfActivityByParmentPDFAsync(listAparmentProgress);

            if (bytesForPDF != null)
            {

                var fileName = "AvanceActividadPorDepartamento.pdf";
                var fileStream = new MemoryStream(bytesForPDF);
                using var streamRef = new DotNetStreamReference(stream: fileStream);
                await _JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);

            }
            _isLoadingProcess = false;
            StateHasChanged();
        }
        public class InfoActivityIn
        {
            public string activityNumber { get; set; }
            public string aparmentNumber { get; set; }
            public Tuple<double, double> porcentage { get; set; }
        }
    }
}
