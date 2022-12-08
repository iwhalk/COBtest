using Microsoft.AspNetCore.Components;
using Obra.Client.Interfaces;
using Obra.Client.Services;
using Obra.Client.Stores;

namespace Obra.Client.Pages
{
    public partial class ProgressForApartment : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly IApartmentsService _apartmentsService;
        private readonly IProgressLogsService _progressLogsService;
        private readonly IProgressReportService _progressReportService;
        //Variable locales
        private Dictionary<int, Tuple<int, int>> _idsAparmentSelect { get; set; } = new();        
        private bool _isFullAparment { get; set;}
        
        public ProgressForApartment(ApplicationContext context, IApartmentsService apartmentsService, IProgressLogsService progressLogsService, IProgressReportService progressReportService) 
        {
            _context = context;
            _apartmentsService = apartmentsService;
            _progressLogsService = progressLogsService;
            _progressReportService = progressReportService;
        }
        protected async override Task OnInitializedAsync()
        {
            await _apartmentsService.GetApartmentsAsync();                        
        }
        private async void AddIdAparmentSelect(int idDeparment)
        {
            if (!_idsAparmentSelect.ContainsKey(idDeparment))
            {
                var infoProgress = await _progressReportService.GetProgresReportViewAsync(idDeparment);
                if (infoProgress.Count > 0)
                {
                    var porcentageProgress = (int)Math.Round(infoProgress.FirstOrDefault().ApartmentProgress);
                    var porcentage = new Tuple<int, int>(porcentageProgress, 100 - porcentageProgress);
                    _idsAparmentSelect.Add(idDeparment, porcentage);
                }
                else
                {
                    _idsAparmentSelect.Add(idDeparment, new Tuple<int, int>(0, 100));
                }
            }
            else
            {
                _idsAparmentSelect = _idsAparmentSelect.Where(x => x.Key != idDeparment).Select(x => new { x.Key, x.Value }).ToDictionary(x =>  x.Key, x => x.Value);                
            }
            StateHasChanged();
        }
        private async void FullAparment()
        {
            if (_idsAparmentSelect.Count() == _context.Apartment.Count())
            {
                _isFullAparment = false;
                _idsAparmentSelect.Clear();
            }
            else
            {
                var infoProgress = await _progressReportService.GetProgresReportViewAsync(null);
                if (infoProgress != null)
                {
                    foreach(var aparment in _context.Apartment)
                    {
                        var porcentageProgress = (int)Math.Round(infoProgress.Where(x => x.ApartmentNumber == aparment.ApartmentNumber).FirstOrDefault().ApartmentProgress);
                        var porcentage = new Tuple<int, int>(porcentageProgress, 100 - porcentageProgress);
                        _idsAparmentSelect.Add(aparment.IdApartment, porcentage);
                    }
                    _isFullAparment = true;
                }                                
            }
        }   
    }
}
