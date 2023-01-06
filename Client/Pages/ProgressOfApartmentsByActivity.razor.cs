using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;

namespace Obra.Client.Pages
{
    public partial class ProgressOfApartmentsByActivity : ComponentBase
    {             
        private readonly ApplicationContext _context;
        private readonly IActivitiesService _activityService;
        private readonly IApartmentsService _apartmentService;
        private readonly NavigationManager _navigationManager;
        private readonly IProgressLogsService _progressLogsService;
        private readonly IReportsService _reportService;
        private readonly IJSRuntime _JS;
        //Variable locales
        private Dictionary<int, Tuple<int, int>> _idsAparmentSelect { get; set; } = new();
        private Dictionary<int, Dictionary<string, Tuple<double, double>>> _idsActivitySelect { get; set; } = new();
        public bool _isLoadingProcess { get; set; }
        private bool _isFullAparment { get; set; }
        public ProgressOfApartmentsByActivity(ApplicationContext context, NavigationManager navigationManager, IActivitiesService activityService, IApartmentsService apartmentService, IProgressLogsService progressLogsService, IReportsService reportService, IJSRuntime jS)
        {
            _context = context;
            _activityService = activityService;
            _apartmentService = apartmentService;
            _navigationManager = navigationManager;
            _progressLogsService = progressLogsService;
            _reportService = reportService;
            _JS = jS;
        }
        protected async override Task OnInitializedAsync()
        {
            _context.Activity = await _activityService.GetActivitiesAsync();
            _context.Apartment = await _apartmentService.GetApartmentsAsync();
        }
        private void BackPage() => _navigationManager.NavigateTo("/ProjectOverview");
        private async void AddIdActivitySelect(int idActivity)
        {
            _isLoadingProcess = true;
            if (!_idsAparmentSelect.ContainsKey(idActivity))
            {
                //change for real endpoint for this view
                var infoProgress = await _reportService.GetProgressOfAparmentByActivityDataViewAsync(idActivity);               
                if (infoProgress != null)
                {
                    var dictionaryIn = new Dictionary<string, Tuple<double, double>>();
                    foreach(var item in infoProgress)
                    {
                        var porcentageProgress = Math.Round(infoProgress.FirstOrDefault().ApartmentProgress, 2);
                        var porcentage = new Tuple<double, double>(porcentageProgress, 100 - porcentageProgress);
                        dictionaryIn.Add(item.ApartmentNumber, porcentage);
                    }
                    _idsActivitySelect.Add(idActivity, dictionaryIn);
                    //var porcentageProgress = (int)Math.Round(infoProgress.FirstOrDefault().ApartmentProgress);
                    //var porcentage = new Tuple<int, int>(porcentageProgress, 100 - porcentageProgress);
                    //_idsAparmentSelect.Add(idActivity, porcentage);
                }
                else
                {
                    var dictionaryIn = new Dictionary<string, Tuple<double, double>>();
                    foreach (var aparment in _context.Apartment)
                    {
                        dictionaryIn.Add(aparment.ApartmentNumber, new Tuple<double, double>(0, 100.0));
                    }
                    _idsActivitySelect.Add(idActivity, dictionaryIn);
                }
            }
            else
            {
                _idsActivitySelect = _idsAparmentSelect.Where(x => x.Key != idActivity).Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key, x => x.Value);
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
                var infoProgress = await _reportService.GetProgressByAparmentDataViewAsync(null);
                if (infoProgress != null)
                {
                    foreach (var activity in _context.Activity)
                    {
                        if (infoProgress.Exists(x => x.ApartmentNumber == activity.ActivityName))
                        {
                            var porcentageProgress = (int)Math.Round(infoProgress.Where(x => x.ApartmentNumber == activity.ActivityName).FirstOrDefault().ApartmentProgress);
                            var porcentage = new Tuple<int, int>(porcentageProgress, 100 - porcentageProgress);
                            _idsAparmentSelect.Add(activity.IdActivity, porcentage);
                        }
                        else
                        {
                            _idsAparmentSelect.Add(activity.IdActivity, new Tuple<int, int>(0, 100));
                        }
                    }
                    _isFullAparment = true;
                }
            }
            _isLoadingProcess = false;
            StateHasChanged();
        }
        private async void GeneratePDfPorgressaprment()
        {
            _isLoadingProcess = true;
            var listAparmentProgress = _idsAparmentSelect.Select(x => new AparmentProgress
            {
                ApartmentNumber = _context.Apartment.Find(o => o.IdApartment == x.Key).ApartmentNumber,
                ApartmentProgress = x.Value.Item1 * 1.0

            }).ToList();

            var bytesForPDF = await _reportService.PostProgressByAparmentPDFAsync(listAparmentProgress);

            if (bytesForPDF != null)
            {

                var fileName = "AvanceDepartamentoPorActividad.pdf";
                var fileStream = new MemoryStream(bytesForPDF);
                using var streamRef = new DotNetStreamReference(stream: fileStream);
                await _JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            }
            _isLoadingProcess = false;
            StateHasChanged();
        }
    }
}
