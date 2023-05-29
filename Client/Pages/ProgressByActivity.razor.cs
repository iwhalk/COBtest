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
        private readonly IObjectAccessService _accessService;
        //Variable locales
        private Dictionary<int, Tuple<double, double>> _idsActivitySelect { get; set; } = new();
        private Dictionary<int, Tuple<double, double>> _idsActivitySelectMoney { get; set; } = new();
        public bool _isLoadingProcess { get; set; }
        private bool _isFullActivity { get; set; }
        private bool _showPreviewFile { get; set; }
        private byte[] _bytesPreviewFile { get; set; }
        private const string PDF_FILE_NAME = "AvancePorActividad.pdf"; 
        private readonly IJSInProcessRuntime _js;
        private IEnumerable<int> activitiesId;
        private string subTitle = "(Seleccionadas)";
        public ObjectAccessUser Accesos { get; private set; }

        public bool ButtonMoneyAndPorcentaje { get; set; } = false;

        public ProgressByActivity(ApplicationContext context, NavigationManager navigationManager, IActivitiesService activityService, IProgressLogsService progressLogsService, IReportsService _reportService,
            IJSRuntime jS, IJSInProcessRuntime js, IObjectAccessService accessService)
        {
            _context = context;
            _activityService = activityService;
            _navigationManager = navigationManager;
            _progressLogsService = progressLogsService;
            this._reportService = _reportService;
            _JS = jS;
            _js = js;
            _accessService = accessService;
        }
        protected async override Task OnInitializedAsync()
        {
            Accesos = await _accessService.GetObjectAccess();
            activitiesId = Accesos.Activities.Select(x => x.IdActivity);
            _context.Activity = await _activityService.GetActivitiesAsync();
            _context.Activity = _context.Activity.Where(x => activitiesId.Contains(x.IdActivity)).ToList();
        }

        private void ButtonMP() => ButtonMoneyAndPorcentaje = !ButtonMoneyAndPorcentaje;

        private void ChangeOpenModalPreview() => _showPreviewFile = _showPreviewFile ? false : true;
        private void BackPage() => _navigationManager.NavigateTo("/ProjectOverview");
        private async void AddIdActivitySelect(int idActivity)
        {
            _isLoadingProcess = true;
            if (!_idsActivitySelect.ContainsKey(idActivity))
            {
                //change for real endpoint for this view
                var infoProgress = await _reportService.GetProgressByActivityDataViewAsync(Accesos.IdBuilding, idActivity);
                if (infoProgress != null)
                {
                    var porcentageProgress = Math.Round(infoProgress.FirstOrDefault().Progress, 2);
                    var porcentage = new Tuple<double, double>(porcentageProgress, 100 - porcentageProgress);

                    string moneyProgress = infoProgress.FirstOrDefault().ActivitytCost.ToString("0.##");
                    string moneyTotal = infoProgress.FirstOrDefault().ActivityCostTotal.ToString("0.##");

                    var restante = Convert.ToDouble(moneyTotal) - Convert.ToDouble(moneyProgress);

                    var moneyR = new Tuple<double, double>(Convert.ToDouble(moneyProgress), restante);

                    _idsActivitySelect.Add(idActivity, porcentage);
                    _idsActivitySelectMoney.Add(idActivity, moneyR);
                }
                else
                {
                    _idsActivitySelect.Add(idActivity, new Tuple<double, double>(0.0, 100.0));

                    var aux = (await _reportService.GetCostTotalActivity(Accesos.IdBuilding, idActivity)).ToString("0.##");

                    _idsActivitySelectMoney.Add(idActivity, new Tuple<double, double>(0.0, Convert.ToDouble(aux)));
                }
                if (activitiesId.Count() == _idsActivitySelect.Count())
                    subTitle = "(Todas)";
            }
            else
            {
                _idsActivitySelect = _idsActivitySelect.Where(x => x.Key != idActivity).Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key, x => x.Value);
                _idsActivitySelectMoney = _idsActivitySelectMoney.Where(x => x.Key != idActivity).Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key, x => x.Value);

                subTitle = "(Seleccionadas)";
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
                _idsActivitySelectMoney.Clear();

                subTitle = "(Seleccionadas)";
            }
            else
            {
                _idsActivitySelect.Clear();
                _idsActivitySelectMoney.Clear();

                subTitle = "(Todas)";
                var infoProgress = await _reportService.GetProgressByActivityDataViewAsync(Accesos.IdBuilding, null);
                if (infoProgress != null)
                {
                    foreach (var activity in _context.Activity)
                    {
                        if (infoProgress.Exists(x => x.ActivityName == activity.ActivityName))
                        {
                            var porcentageProgress = Math.Round(infoProgress.Where(x => x.ActivityName == activity.ActivityName).FirstOrDefault().Progress, 2);
                            var porcentage = new Tuple<double, double>(porcentageProgress, 100 - porcentageProgress);

                            string moneyProgress = infoProgress.Where(x => x.ActivityName == activity.ActivityName).FirstOrDefault().ActivitytCost.ToString("0.##");
                            string moneyTotal = infoProgress.Where(x => x.ActivityName == activity.ActivityName).FirstOrDefault().ActivityCostTotal.ToString("0.##");

                            var restante = Convert.ToDouble(moneyTotal) - Convert.ToDouble(moneyProgress);

                            var moneyR = new Tuple<double, double>(Convert.ToDouble(moneyProgress), restante);

                            _idsActivitySelect.Add(activity.IdActivity, porcentage);
                            _idsActivitySelectMoney.Add(activity.IdActivity, moneyR);
                        }
                        else
                        {
                            _idsActivitySelect.Add(activity.IdActivity, new Tuple<double, double>(0.0, 100.0));

                            var aux = (await _reportService.GetCostTotalActivity(Accesos.IdBuilding, activity.IdActivity)).ToString("0.##");

                            _idsActivitySelectMoney.Add(activity.IdActivity, new Tuple<double, double>(0.0, Convert.ToDouble(aux)));
                        }
                    }
                    _isFullActivity = true;
                }
            }
            _isLoadingProcess = false;
            StateHasChanged();
        }

        private async void PreviewFileReport()
        {
            _isLoadingProcess = true;
            var listActivityProgress = _idsActivitySelect.Select(x => new ActivityProgress
            {
                ActivityName = _context.Activity.Find(o => o.IdActivity == x.Key).ActivityName,
                Progress = x.Value.Item1 * 1.0

            }).ToList();
            var bytes = await _reportService.PostProgressByActivityPDFAsync(listActivityProgress, subTitle);
            if (bytes is not null)
            {
                //_bytesPreviewFile = bytes;
                _isLoadingProcess = false;
                //_showPreviewFile = true;
                await _JS.InvokeVoidAsync("OpenInNewPagePDF", bytes);
                StateHasChanged();                
            }
            else
            {
                _isLoadingProcess = false;   
                StateHasChanged();
            }
        }
        private async void GeneratePDfPorgressaprment()
        {
            _isLoadingProcess = true;
            var listActivityProgress = _idsActivitySelect.Select(x => new ActivityProgress
            {
                ActivityName = _context.Activity.Find(o => o.IdActivity == x.Key).ActivityName,
                Progress = x.Value.Item1 * 1.0

            }).ToList();

            var bytesForPDF = await _reportService.PostProgressByActivityPDFAsync(listActivityProgress, subTitle);

            if (bytesForPDF != null)
            {
                var fileName = PDF_FILE_NAME;
                var fileStream = new MemoryStream(bytesForPDF);
                using var streamRef = new DotNetStreamReference(stream: fileStream);
                await _JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            }
            _isLoadingProcess = false;
            StateHasChanged();
        }
    }
}
