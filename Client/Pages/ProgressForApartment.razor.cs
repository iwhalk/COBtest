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
        private Dictionary<int, string> _idsAparmentSelect { get; set; } = new();        
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
                    _idsAparmentSelect.Add(idDeparment, infoProgress.FirstOrDefault().ApartmentProgress.ToString());
                }
                else
                {
                    _idsAparmentSelect.Add(idDeparment, 0.ToString());
                }
            }
            else
            {
                _idsAparmentSelect = _idsAparmentSelect.Where(x => x.Key != idDeparment).Select(x => new { x.Key, x.Value }).ToDictionary(x =>  x.Key, x => x.Value);                
            }
            StateHasChanged();
        }
        private void FullAparment()
        {
            if (_idsAparmentSelect.Count() == _context.Apartment.Count())
            {
                _isFullAparment = false;
                _idsAparmentSelect.Clear();
            }
            else
            {
                _isFullAparment = true;
                _idsAparmentSelect = new();//_context.Apartment.Select(x => x.IdApartment).ToList();
            }
        }   
    }
}
