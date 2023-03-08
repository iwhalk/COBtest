﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;

namespace Obra.Client.Pages
{
    public partial class ProgressByApartment : ComponentBase
    {        
        private readonly ApplicationContext _context;
        private readonly IApartmentsService _apartmentsService;
        private readonly IProgressLogsService _progressLogsService;
        private readonly NavigationManager _navigationManager;
        private readonly IReportsService _reportService;
        private readonly IJSRuntime _JS;
        private readonly IObjectAccessService _accessService;
        //Variable locales
        private Dictionary<int, Tuple<double, double>> _idsAparmentSelect { get; set; } = new();
        public bool _isLoadingProcess { get; set; }
        private bool _isFullAparment { get; set; }
        private bool _showPreviewFile { get; set; }
        private byte[] _bytesPreviewFile { get; set; }
        private const string PDF_FILE_NAME = "AvancePorDepartamento.pdf";
        public ObjectAccessUser Accesos { get; private set; }
        public ProgressByApartment(ApplicationContext context, NavigationManager navigationManager, IApartmentsService apartmentsService,
            IProgressLogsService progressLogsService, IReportsService reportService, IJSRuntime jS, IObjectAccessService accessService)
        {
            _context = context;
            _apartmentsService = apartmentsService;
            _progressLogsService = progressLogsService;
            _navigationManager = navigationManager;
            _reportService = reportService;
            _JS = jS;
            _accessService = accessService;
        }
        protected async override Task OnInitializedAsync()
        {
            Accesos = await _accessService.GetObjectAccess();
            var apartmentsId = Accesos.Apartments.Select(x => x.IdApartment);
            _context.Apartment = await _apartmentsService.GetApartmentsAsync();
            _context.Apartment = _context.Apartment.Where(x => apartmentsId.Contains(x.IdApartment)).ToList();
        }
        private void BackPage() => _navigationManager.NavigateTo("/ProjectOverview");
        private void ChangeOpenModalPreview() => _showPreviewFile = _showPreviewFile ? false : true;
        private async void AddIdAparmentSelect(int idDeparment)
        {
            _isLoadingProcess = true;
            if (!_idsAparmentSelect.ContainsKey(idDeparment))
            {
                var infoProgress = await _reportService.GetProgressByAparmentDataViewAsync(Accesos.IdBuilding, idDeparment);
                if (infoProgress != null)
                {
                    var porcentageProgress = Math.Round(infoProgress.FirstOrDefault().ApartmentProgress, 2);
                    var porcentage = new Tuple<double, double>(porcentageProgress, 100 - porcentageProgress);
                    _idsAparmentSelect.Add(idDeparment, porcentage);
                }
                else
                {
                    _idsAparmentSelect.Add(idDeparment, new Tuple<double, double>(0.0, 100.00));
                }
            }
            else
            {
                _idsAparmentSelect = _idsAparmentSelect.Where(x => x.Key != idDeparment).Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key, x => x.Value);
            }
            _isLoadingProcess = false;
            StateHasChanged();
        }
        private async void FullAparment()
        {
            _isLoadingProcess = true;
            if (_idsAparmentSelect.Count() == _context.Apartment.Count())
            {
                _isFullAparment = false;
                _idsAparmentSelect.Clear();
            }
            else
            {
                _idsAparmentSelect.Clear();
                var infoProgress = await _reportService.GetProgressByAparmentDataViewAsync(Accesos.IdBuilding, null);
                if (infoProgress != null)
                {
                    foreach (var aparment in _context.Apartment)
                    {
                        if (infoProgress.Exists(x => x.ApartmentNumber == aparment.ApartmentNumber))
                        {
                            var porcentageProgress = Math.Round(infoProgress.Where(x => x.ApartmentNumber == aparment.ApartmentNumber).FirstOrDefault().ApartmentProgress, 2);
                            var porcentage = new Tuple<double, double>(porcentageProgress, 100 - porcentageProgress);
                            //porcentageProgress = porcentageProgress < 1.0 ? porcentageProgress + 1.5 : porcentageProgress;
                            _idsAparmentSelect.Add(aparment.IdApartment, porcentage);
                        }
                        else
                        {
                            _idsAparmentSelect.Add(aparment.IdApartment, new Tuple<double, double>(0.0, 100.00));
                        }
                    }
                    _isFullAparment = true;
                }
            }
            _isLoadingProcess = false;
            StateHasChanged();
        }
        private async void PreviewFileReport()
        {
            _isLoadingProcess = true;
            var listAparmentProgress = _idsAparmentSelect.Select(x => new AparmentProgress
            {
                ApartmentNumber = _context.Apartment.Find(o => o.IdApartment == x.Key).ApartmentNumber,
                ApartmentProgress = x.Value.Item1 * 1.0

            }).ToList();

            var bytes = await _reportService.PostProgressByAparmentPDFAsync(listAparmentProgress);
            
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
            var listAparmentProgress = _idsAparmentSelect.Select(x => new AparmentProgress
            {
                ApartmentNumber = _context.Apartment.Find(o => o.IdApartment == x.Key).ApartmentNumber,
                ApartmentProgress = x.Value.Item1 * 1.0

            }).ToList();

            var bytesForPDF = await _reportService.PostProgressByAparmentPDFAsync(listAparmentProgress);

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
