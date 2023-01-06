using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;

namespace Obra.Client.Pages
{
    public partial class ProgressByActivity : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly IActivitiesService _activityService;
        private readonly NavigationManager _navigationManager;
        private readonly IProgressLogsService _progressLogsService;
        private readonly IReportsService _reportService;
        private readonly IJSRuntime _JS;
        //Variable locales
        private Dictionary<int, Tuple<int, int>> _idsActivitySelect { get; set; } = new();
        public bool _isLoadingProcess { get; set; }
        private bool _isFullActivity { get; set; }
        public ProgressByActivity(ApplicationContext context, NavigationManager navigationManager, IActivitiesService activityService, IProgressLogsService progressLogsService, IReportsService _reportService, IJSRuntime jS)
        {
            _context = context;
            _activityService = activityService;
            _navigationManager = navigationManager;
            _progressLogsService = progressLogsService;
            this._reportService = _reportService;
            _JS = jS;
        }
        protected async override Task OnInitializedAsync()
        {
            _context.Activity = await _activityService.GetActivitiesAsync();
        }
        private void BackPage() => _navigationManager.NavigateTo("/ProjectOverview");
        private async void AddIdActivitySelect(int idActivity)
        {
            _isLoadingProcess = true;
            if (!_idsActivitySelect.ContainsKey(idActivity))
            {
                //change for real endpoint for this view
                var infoProgress = await _reportService.GetProgressByAparmentDataViewAsync(idActivity);
                if (infoProgress != null)
                {
                    var porcentageProgress = (int)Math.Round(infoProgress.FirstOrDefault().ApartmentProgress);
                    var porcentage = new Tuple<int, int>(porcentageProgress, 100 - porcentageProgress);
                    _idsActivitySelect.Add(idActivity, porcentage);
                }
                else
                {
                    _idsActivitySelect.Add(idActivity, new Tuple<int, int>(0, 100));
                }
            }
            else
            {
                _idsActivitySelect = _idsActivitySelect.Where(x => x.Key != idActivity).Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key, x => x.Value);
            }
            _isLoadingProcess = false;
            StateHasChanged();
        }
        private async void FullActivity()
        {
            _isLoadingProcess = true;
            if (_idsActivitySelect.Count() == _context.Activity.Count())
            {
                _isFullActivity = false;
                _idsActivitySelect.Clear();
            }
            else
            {
                _idsActivitySelect.Clear();
                var infoProgress = await _reportService.GetProgressByAparmentDataViewAsync(null);
                if (infoProgress != null)
                {
                    foreach (var activity in _context.Activity)
                    {
                        if (infoProgress.Exists(x => x.ApartmentNumber == activity.ActivityName))
                        {
                            var porcentageProgress = (int)Math.Round(infoProgress.Where(x => x.ApartmentNumber == activity.ActivityName).FirstOrDefault().ApartmentProgress);
                            var porcentage = new Tuple<int, int>(porcentageProgress, 100 - porcentageProgress);
                            _idsActivitySelect.Add(activity.IdActivity, porcentage);
                        }
                        else
                        {
                            _idsActivitySelect.Add(activity.IdActivity, new Tuple<int, int>(0, 100));
                        }
                    }
                    _isFullActivity = true;
                }
            }
            _isLoadingProcess = false;
            StateHasChanged();
        }
        private async void GeneratePDfPorgressaprment()
        {
            _isLoadingProcess = true;
            var listAparmentProgress = _idsActivitySelect.Select(x => new AparmentProgress
            {
                ApartmentNumber = _context.Apartment.Find(o => o.IdApartment == x.Key).ApartmentNumber,
                ApartmentProgress = x.Value.Item1 * 1.0

            }).ToList();

            var bytesForPDF = await _reportService.PostProgressByAparmentPDFAsync(listAparmentProgress);

            if (bytesForPDF != null)
            {

                var fileName = "AvancePorActividad.pdf";
                var fileStream = new MemoryStream(bytesForPDF);
                using var streamRef = new DotNetStreamReference(stream: fileStream);
                await _JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);

            }
            _isLoadingProcess = false;
            StateHasChanged();
        }
    }
}
