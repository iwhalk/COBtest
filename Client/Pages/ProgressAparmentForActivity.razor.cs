using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;

namespace Obra.Client.Pages
{    
    public partial class ProgressAparmentForActivity : ComponentBase
    {
        public int MyProperty { get; set; }        
        private readonly ApplicationContext _context;
        private readonly IActivitiesService _activityService;
        private readonly IApartmentsService _apartmentService;
        private readonly IProgressLogsService _progressLogsService;
        private readonly IProgressReportService _progressReportService;
        private readonly IJSRuntime _JS;
        //Variable locales
        private Dictionary<int, Tuple<int, int>> _idsAparmentSelect { get; set; } = new();
        public bool _isLoadingProcess { get; set; }
        private bool _isFullAparment { get; set; }
        public ProgressAparmentForActivity(ApplicationContext context, IActivitiesService activityService, IApartmentsService apartmentService, IProgressLogsService progressLogsService, IProgressReportService progressReportService, IJSRuntime jS)
        {
            _context = context;
            _activityService = activityService;
            _apartmentService = apartmentService;
            _progressLogsService = progressLogsService;
            _progressReportService = progressReportService;
            _JS = jS;
        }
        protected async override Task OnInitializedAsync()
        {
            _context.Activity = await _activityService.GetActivitiesAsync();
            _context.Apartment = await _apartmentService.GetApartmentsAsync();
        }
        private async void AddIdActivitySelect(int idActivity)
        {
            _isLoadingProcess = true;
            if (!_idsAparmentSelect.ContainsKey(idActivity))
            {
                //change for real endpoint for this view
                var infoProgress = await _progressReportService.GetProgresReportViewAsync(idActivity);
                if (infoProgress != null)
                {
                    var porcentageProgress = (int)Math.Round(infoProgress.FirstOrDefault().ApartmentProgress);
                    var porcentage = new Tuple<int, int>(porcentageProgress, 100 - porcentageProgress);
                    _idsAparmentSelect.Add(idActivity, porcentage);
                }
                else
                {
                    _idsAparmentSelect.Add(idActivity, new Tuple<int, int>(0, 100));
                }
            }
            else
            {
                _idsAparmentSelect = _idsAparmentSelect.Where(x => x.Key != idActivity).Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key, x => x.Value);
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
                var infoProgress = await _progressReportService.GetProgresReportViewAsync(null);
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

            var bytesForPDF = await _progressReportService.PostProgressReporPDFtAsync(listAparmentProgress);

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
