using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Obra.Client.Interfaces;
using Obra.Client.Services;
using Obra.Client.Stores;
using SharedLibrary.Models;
using System.IO;

namespace Obra.Client.Pages
{
    public partial class ProgressForApartment : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly IApartmentsService _apartmentsService;
        private readonly IProgressLogsService _progressLogsService;
        private readonly IProgressReportService _progressReportService;
        private readonly IJSRuntime _JS;
        //Variable locales
        private Dictionary<int, Tuple<int, int>> _idsAparmentSelect { get; set; } = new();
        private bool _isFullAparment { get; set;}
        public ProgressForApartment(ApplicationContext context, IApartmentsService apartmentsService, IProgressLogsService progressLogsService, IProgressReportService progressReportService, IJSRuntime jS)
        {
            _context = context;
            _apartmentsService = apartmentsService;
            _progressLogsService = progressLogsService;
            _progressReportService = progressReportService;
            _JS = jS;
        }
        protected async override Task OnInitializedAsync()
        {
            _context.Apartment = await _apartmentsService.GetApartmentsAsync();                        
        }
        private async void AddIdAparmentSelect(int idDeparment)
        {
            if (!_idsAparmentSelect.ContainsKey(idDeparment))
            {
                var infoProgress = await _progressReportService.GetProgresReportViewAsync(idDeparment);
                if (infoProgress != null)
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
                _idsAparmentSelect = _idsAparmentSelect.Where(x => x.Key != idDeparment).Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key, x => x.Value);
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
                _idsAparmentSelect.Clear();
                var infoProgress = await _progressReportService.GetProgresReportViewAsync(null);
                if (infoProgress != null)
                {
                    foreach (var aparment in _context.Apartment)
                    {
                        if (infoProgress.Exists(x => x.ApartmentNumber == aparment.ApartmentNumber))
                        {
                            var porcentageProgress = (int)Math.Round(infoProgress.Where(x => x.ApartmentNumber == aparment.ApartmentNumber).FirstOrDefault().ApartmentProgress);
                            var porcentage = new Tuple<int, int>(porcentageProgress, 100 - porcentageProgress);
                            _idsAparmentSelect.Add(aparment.IdApartment, porcentage);
                        }
                        else
                        {
                            _idsAparmentSelect.Add(aparment.IdApartment, new Tuple<int, int>(0, 100));
                        }
                    }
                    _isFullAparment = true;
                }                
            }
            StateHasChanged();
        }
        private async void GeneratePDfPorgressaprment()
        {
            var listAparmentProgress = _idsAparmentSelect.Select(x => new AparmentProgress
            {
                ApartmentNumber = _context.Apartment.Find(o => o.IdApartment == x.Key).ApartmentNumber,
                ApartmentProgress = x.Value.Item1 * 1.0

            }).ToList();

            var bytesForPDF = await  _progressReportService.PostProgressReporPDFtAsync(listAparmentProgress);

            if (bytesForPDF != null)
            {

                var fileName = "test.pdf";
                var fileStream = new MemoryStream(bytesForPDF);
                using var streamRef = new DotNetStreamReference(stream: fileStream);
                await _JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);

            }
        }
    }
}
