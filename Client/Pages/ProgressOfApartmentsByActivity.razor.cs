using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

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
        private Dictionary<int, List<InfoAparmentIn>> _idsActivitySelect { get; set; } = new();
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
            if (!_idsActivitySelect.ContainsKey(idActivity))
            {
                //change for real endpoint for this view
                var infoProgress = await _reportService.GetProgressOfAparmentByActivityDataViewAsync(idActivity);
                if (infoProgress != null)
                {
                    List<InfoAparmentIn> listAparmentPorcentage = new List<InfoAparmentIn>();
                    foreach (var item in infoProgress)
                    {
                        var porcentageProgress = Math.Round(item.ApartmentProgress, 2);
                        var porcentage = new Tuple<double, double>(porcentageProgress, 100 - porcentageProgress);
                        listAparmentPorcentage.Add(new InfoAparmentIn { aparmentNumber = item.ApartmentNumber, porcentage = porcentage });
                    }
                    _idsActivitySelect.Add(idActivity, listAparmentPorcentage);
                }
                else
                {
                    var listAparmentPorcentage = new List<InfoAparmentIn>();
                    foreach (var aparment in _context.Apartment)
                    {
                        listAparmentPorcentage.Add(new InfoAparmentIn
                        {
                            aparmentNumber = aparment.ApartmentNumber,
                            porcentage = new Tuple<double, double>(0.0, 100.0)
                        });
                    }
                    _idsActivitySelect.Add(idActivity, listAparmentPorcentage);
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
                _isFullAparment = false;
                _idsActivitySelect.Clear();
            }
            else
            {
                _idsActivitySelect.Clear();
                var infoProgress = await _reportService.GetProgressOfAparmentByActivityDataViewAsync(null);            
                if (infoProgress != null)
                {
                    
                    foreach (var activity in _context.Activity)
                    {
                        if (infoProgress.Any(x => x.Activity_ == activity.ActivityName))
                        {
                            List<InfoAparmentIn> listAparmentPorcentage = new List<InfoAparmentIn>();
                            foreach (var item in infoProgress.Where(x => x.Activity_ == activity.ActivityName))
                            {
                                var porcentageProgress = Math.Round(item.ApartmentProgress, 2);
                                var porcentage = new Tuple<double, double>(porcentageProgress, 100 - porcentageProgress);
                                listAparmentPorcentage.Add(new InfoAparmentIn { aparmentNumber = item.ApartmentNumber, porcentage = porcentage });
                            }
                            _idsActivitySelect.Add(activity.IdActivity, listAparmentPorcentage);
                        }
                        else
                        {
                            var listAparmentPorcentage = new List<InfoAparmentIn>();
                            foreach (var aparment in _context.Apartment)
                            {
                                listAparmentPorcentage.Add(new InfoAparmentIn
                                {
                                    aparmentNumber = aparment.ApartmentNumber,
                                    porcentage = new Tuple<double, double>(0.0, 100.0)
                                });
                            }
                            _idsActivitySelect.Add(activity.IdActivity, listAparmentPorcentage);
                        }
                    }
                }
            }
            _isLoadingProcess = false;
            StateHasChanged();
        }
        private async void GeneratePDfPorgressaprment()
        {
            _isLoadingProcess = true;
            var listAparmentProgress = new List<AparmentProgress>();

            foreach (var item in _idsActivitySelect)
            {
                foreach(var aparment in item.Value)
                {
                    listAparmentProgress.Add(new AparmentProgress
                    {
                        Activity_ = _context.Activity.Find(x => x.IdActivity == item.Key).ActivityName,
                        ApartmentNumber = aparment.aparmentNumber,
                        ApartmentProgress = aparment.porcentage.Item1
                    });
                }
            }
            var bytesForPDF = await _reportService.PostProgressOfAparmentByActivityPDFAsync(listAparmentProgress);

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
        public class InfoAparmentIn
        {
            public string aparmentNumber { get; set; }
            public Tuple<double, double> porcentage { get; set; }
        }
    }
}
