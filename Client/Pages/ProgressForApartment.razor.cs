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

        private List<int> _idsAparmentSelect { get; set; } = new();
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
            var progressLog = await _progressLogsService.GetProgressLogsAsync();
            var progressReport = await _progressReportService.GetProgressReportsAsync();
        }
        private void AddIdAparmentSelect(int idDeparment)
        {
            if(!_idsAparmentSelect.Contains(idDeparment))
                _idsAparmentSelect.Add(idDeparment);

            else
            {
                _idsAparmentSelect = _idsAparmentSelect.Where(x => x != idDeparment).ToList();
            }
        }
        private void FullAparment()
        {
            //if (_idsAparmentSelect.Count() > 0)
            //{
            //    _isFullAparment = false;
            //    _idsAparmentSelect.Clear();
            //}
            //else
            //{
                _isFullAparment = true;
                _idsAparmentSelect = _context.Apartment.Select(x => x.IdApartment).ToList();
            //}
        }
    }
}
