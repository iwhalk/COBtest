using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;
using System.ComponentModel;

namespace Obra.Client.Pages
{
    public partial class ProgressForActivity : ComponentBase
    {        
        private readonly ApplicationContext _context;
        private readonly IActivitiesService _activityService;
        private readonly IProgressLogsService _progressLogsService;
        private readonly IProgressReportService _progressReportService;
        private readonly IJSRuntime _JS;
        //Variable locales
        private Dictionary<int, Tuple<int, int>> _idsActivitySelect { get; set; } = new();
        public bool _isLoadingProcess { get; set; }
        private bool _isFullActivity { get; set; }
        public ProgressForActivity(ApplicationContext context, IActivitiesService activityService, IProgressLogsService progressLogsService, IProgressReportService progressReportService, IJSRuntime jS)
        {
            _context = context;
            _activityService = activityService;
            _progressLogsService = progressLogsService;
            _progressReportService = progressReportService;
            _JS = jS;
        }
        protected async override Task OnInitializedAsync()
        {
            _context.Activity = await _activityService.GetActivitiesAsync();
        }
        private async void AddIdActivitySelect(int idActivity)
        {
            _isLoadingProcess = true;
            if (!_idsActivitySelect.ContainsKey(idActivity))
            {
                //change for real endpoint for this view
                var infoProgress = await _progressReportService.GetProgresReportViewAsync(idActivity);
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
                var infoProgress = await _progressReportService.GetProgresReportViewAsync(null);
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

            var bytesForPDF = await _progressReportService.PostProgressReporPDFtAsync(listAparmentProgress);

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
