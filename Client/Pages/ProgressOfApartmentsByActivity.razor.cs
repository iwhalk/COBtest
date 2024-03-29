﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using static Obra.Client.Pages.ProgressOfActivitiesByApartment;

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
        private readonly IObjectAccessService _accessService;
        //Variable locales        
        private Dictionary<int, List<InfoAparmentIn>> _idsActivitySelect { get; set; } = new();
        private Dictionary<int, List<InfoAparmentIn>> _idsAparmentSelectCost { get; set; } = new();
        public bool _isLoadingProcess { get; set; }
        private bool _isFullAparment { get; set; }
        private bool _showPreviewFile { get; set; }
        private byte[] _bytesPreviewFile { get; set; }
        private const string PDF_FILE_NAME = "AvanceDepartamentoPorActividad.pdf";
        public ObjectAccessUser Accesos { get; private set; }

        public bool ButtonMoneyAndPorcentaje { get; set; } = false;

        public ProgressOfApartmentsByActivity(ApplicationContext context, NavigationManager navigationManager, IActivitiesService activityService, IApartmentsService apartmentService,
            IProgressLogsService progressLogsService, IReportsService reportService, IJSRuntime jS, IObjectAccessService accessService)
        {
            _context = context;
            _activityService = activityService;
            _apartmentService = apartmentService;
            _navigationManager = navigationManager;
            _progressLogsService = progressLogsService;
            _reportService = reportService;
            _JS = jS;
            _accessService = accessService;
        }
        protected async override Task OnInitializedAsync()
        {
            Accesos = await _accessService.GetObjectAccess();
            var activitiesId = Accesos.Activities.Select(x => x.IdActivity);
            var apartmentsId = Accesos.Apartments.Select(x => x.IdApartment);
            _context.Activity = await _activityService.GetActivitiesAsync();
            _context.Activity = _context.Activity.Where(x => activitiesId.Contains(x.IdActivity)).ToList();
            _context.Apartment = await _apartmentService.GetApartmentsAsync();
            _context.Apartment = _context.Apartment.Where(x => apartmentsId.Contains(x.IdApartment)).ToList();
        }

        private void ButtonMP() => ButtonMoneyAndPorcentaje = !ButtonMoneyAndPorcentaje;
        private void BackPage() => _navigationManager.NavigateTo("/ProjectOverview");
        private void ChangeOpenModalPreview() => _showPreviewFile = _showPreviewFile ? false : true;
        private async void AddIdActivitySelect(int idActivity)
        {

            _isLoadingProcess = true;
            if (!_idsActivitySelect.ContainsKey(idActivity))
            {
                //change for real endpoint for this view
                var infoProgress = await _reportService.GetProgressOfAparmentByActivityDataViewAsync(Accesos.IdBuilding, idActivity);

                if (infoProgress != null)
                {
                    List<InfoAparmentIn> listAparmentPorcentage = new List<InfoAparmentIn>();
                    List<InfoAparmentIn> listAparmentCost = new List<InfoAparmentIn>();

                    foreach (var item in infoProgress)
                    {
                        var porcentageProgress = Math.Round(item.ApartmentProgress, 2);
                        var porcentage = new Tuple<double, double>(porcentageProgress, 100 - porcentageProgress);
                        string moneyProgress = item.ApartmentCost.ToString("0.##");
                        string moneyTotal = item.ApartmentCostTotal.ToString("0.##");

                        var restante = Convert.ToDouble(moneyTotal) - Convert.ToDouble(moneyProgress);

                        var moneyR = new Tuple<double, double>(Convert.ToDouble(moneyProgress), restante);

                        listAparmentCost.Add(new InfoAparmentIn { aparmentNumber = item.ApartmentNumber, porcentage = moneyR });
                        listAparmentPorcentage.Add(new InfoAparmentIn { aparmentNumber = item.ApartmentNumber, porcentage = porcentage });
                    }

                    _idsAparmentSelectCost.Add(idActivity, listAparmentCost);
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

                    var listAparmentCost = new List<InfoAparmentIn>();

                    foreach (var aparment in _context.Apartment)
                    {
                        var aux = await _reportService.GetCostTotalActivitiesByAparment(Accesos.IdBuilding, aparment.IdApartment);

                        listAparmentCost.Add(new InfoAparmentIn
                        {
                            aparmentNumber = aparment.ApartmentNumber,
                            porcentage = new Tuple<double, double>(0.0, aux)
                        });
                    }

                    _idsAparmentSelectCost.Add(idActivity, listAparmentCost);
                    _idsActivitySelect.Add(idActivity, listAparmentPorcentage);
                }
            }
            else
            {
                _idsActivitySelect = _idsActivitySelect.Where(x => x.Key != idActivity).Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key, x => x.Value);
                _idsAparmentSelectCost = _idsAparmentSelectCost.Where(x => x.Key != idActivity).Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key, x => x.Value);
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
                _idsAparmentSelectCost.Clear();
            }
            else
            {
                _idsActivitySelect.Clear();
                _idsAparmentSelectCost.Clear();
                var infoProgress = await _reportService.GetProgressOfAparmentByActivityDataViewAsync(Accesos.IdBuilding, null);            
                if (infoProgress != null)
                {
                    
                    foreach (var activity in _context.Activity)
                    {
                        if (infoProgress.Any(x => x.Activity_ == activity.ActivityName))
                        {
                            List<InfoAparmentIn> listAparmentPorcentage = new List<InfoAparmentIn>();
                            List<InfoAparmentIn> listAparmentCost = new List<InfoAparmentIn>();

                            foreach (var item in infoProgress.Where(x => x.Activity_ == activity.ActivityName))
                            {
                                var porcentageProgress = Math.Round(item.ApartmentProgress, 2);
                                var porcentage = new Tuple<double, double>(porcentageProgress, 100 - porcentageProgress);
                                string moneyProgress = item.ApartmentCost.ToString("0.##");
                                string moneyTotal = item.ApartmentCostTotal.ToString("0.##");

                                var restante = Convert.ToDouble(moneyTotal) - Convert.ToDouble(moneyProgress);

                                var moneyR = new Tuple<double, double>(Convert.ToDouble(moneyProgress), restante);

                                listAparmentCost.Add(new InfoAparmentIn { aparmentNumber = item.ApartmentNumber, porcentage = moneyR });
                                listAparmentPorcentage.Add(new InfoAparmentIn { aparmentNumber = item.ApartmentNumber, porcentage = porcentage });
                            }

                            _idsAparmentSelectCost.Add(activity.IdActivity, listAparmentCost);
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

                            var listAparmentCost = new List<InfoAparmentIn>();

                            foreach (var aparment in _context.Apartment)
                            {
                                var aux = await _reportService.GetCostTotalActivitiesByAparment(Accesos.IdBuilding, aparment.IdApartment);

                                listAparmentCost.Add(new InfoAparmentIn
                                {
                                    aparmentNumber = aparment.ApartmentNumber,
                                    porcentage = new Tuple<double, double>(0.0, aux)
                                });
                            }

                            _idsAparmentSelectCost.Add(activity.IdActivity, listAparmentCost);
                            _idsActivitySelect.Add(activity.IdActivity, listAparmentPorcentage);
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
            bool all = Accesos.Activities.Select(x => x.IdActivity).Count() == listAparmentProgress.GroupBy(x => x.Activity_).ToList().Count();
            var bytes = await _reportService.PostProgressOfAparmentByActivityPDFAsync(listAparmentProgress, all);
            
            if (bytes is not null)
            {                
                _isLoadingProcess = false;                
                await _JS.InvokeVoidAsync("OpenInNewPagePDF", bytes);
                StateHasChanged();
            }
            else
            {
                _isLoadingProcess = false;   
                StateHasChanged();
            }
        }
        private string GetDynamicHeightForButtons(int idActivity)
        {
            if (_idsActivitySelect.ContainsKey(idActivity))
            {
                return _idsActivitySelect[idActivity].Count == 4
                    ? "h-auto"
                    : "h-80";
            }
            return "h-12";
        } 
        public class InfoAparmentIn
        {
            public string aparmentNumber { get; set; }
            public Tuple<double, double> porcentage { get; set; }
        }
    }
}
