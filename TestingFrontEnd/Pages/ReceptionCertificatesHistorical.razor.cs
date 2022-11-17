using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;
using FrontEnd.Interfaces;
using static FrontEnd.Components.HeaderReceptionCertificatePendingOrHistorical;
using static FrontEnd.Components.PaginationReceptionCertificate;

namespace FrontEnd.Pages
{
    public partial class ReceptionCertificatesHistorical : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly IReceptionCertificateService _reception;                        
        public List<ActasRecepcion>? actasRecepcions { get; set; }
        public int currentPage { get; set; }
        public int rowNumberForPage { get; set; }
        public int maxNumberPage { get; set; }
        public string TypeTableHistoricalOrPending { get; set; }
        public ReceptionCertificatesHistorical(ApplicationContext context, IReceptionCertificateService reception)
        {
            _context = context;
            _reception = reception;               
        }
        protected override async Task OnInitializedAsync()
        {
            TypeTableHistoricalOrPending = _context.TypeHistoricalOrPending;
            currentPage = 1;
            rowNumberForPage = 10;                                                        
            actasRecepcions = await _reception.GetReceptionCertificatesAsync(null, null, null, null, null, null, null, null, null, currentPage, rowNumberForPage);
            maxNumberPage = _context.MaxNumberPagination;
            _context.CurrentFilterPagination = new FilterReceptionCertificate();
            _context.ListPageInPaginate = CreatePaginationNumber();
        }        
        public async Task Filter(FilterReceptionCertificate filterReception)
        {
            actasRecepcions = null;
            _context.ActasRecepcionList = null;
            _context.NumberPaginationCurrent = 1;
            currentPage = _context.NumberPaginationCurrent;

            if (filterReception.StartDay is not null && filterReception.EndDay is not null)
            {
                string auxS = filterReception.StartDay.Value.Date.ToString("yyyy-MM-dd");
                string auxE = filterReception.EndDay.Value.Date.ToString("yyyy-MM-dd");
                actasRecepcions = await _reception.GetReceptionCertificatesAsync(auxS, auxE, filterReception.CertificateType, filterReception.PropertyType, filterReception.NumberOfRooms, filterReception.Lessor, filterReception.Tenant, filterReception.Delegation, filterReception.Agent, currentPage, rowNumberForPage);                                
                maxNumberPage = _context.MaxNumberPagination;
                _context.ListPageInPaginate = CreatePaginationNumber();
            }
            else
            {
                actasRecepcions = await _reception.GetReceptionCertificatesAsync(null, null, filterReception.CertificateType, filterReception.PropertyType, filterReception.NumberOfRooms, filterReception.Lessor, filterReception.Tenant, filterReception.Delegation, filterReception.Agent, currentPage, rowNumberForPage);                                
                maxNumberPage = _context.MaxNumberPagination;
                _context.ListPageInPaginate = CreatePaginationNumber();
            }
            StateHasChanged();
            _context.CurrentFilterPagination = filterReception;
        }
        public async void HandlePaginationAction(PaginationAction paginationAction)
        {
            var filterReception = _context.CurrentFilterPagination;
            switch (paginationAction)
            {
                case PaginationAction.EndPage:
                    currentPage = _context.MaxNumberPagination;                    
                    break;

                case PaginationAction.FirstPage:
                    _context.NumberPaginationCurrent = 1;
                    currentPage = _context.NumberPaginationCurrent;
                    break;

                case PaginationAction.PreviewPage:
                    if(currentPage - 1 >= 1)
                    {
                        currentPage = currentPage - 1;
                        _context.NumberPaginationCurrent = currentPage;
                        break;
                    }
                    return;
                    break;
                case PaginationAction.NextPage:
                    if(currentPage + 1 <= _context.MaxNumberPagination)
                    {                        
                        currentPage = currentPage + 1;
                        _context.NumberPaginationCurrent = currentPage;
                        break;
                    }
                    return;
                    break;
            }
            actasRecepcions = null;
            _context.ActasRecepcionList = null;
            if (filterReception.StartDay is not null && filterReception.EndDay is not null)
            {
                string auxS = filterReception.StartDay.Value.Date.ToString("yyyy-MM-dd");
                string auxE = filterReception.EndDay.Value.Date.ToString("yyyy-MM-dd");
                actasRecepcions = await _reception.GetReceptionCertificatesAsync(auxS, auxE, filterReception.CertificateType, filterReception.PropertyType, filterReception.NumberOfRooms, filterReception.Lessor, filterReception.Tenant, filterReception.Delegation, filterReception.Agent, currentPage, rowNumberForPage);
                maxNumberPage = _context.MaxNumberPagination;
                _context.ListPageInPaginate = CreatePaginationNumber();
            }
            else
            {
                actasRecepcions = await _reception.GetReceptionCertificatesAsync(null, null, filterReception.CertificateType, filterReception.PropertyType, filterReception.NumberOfRooms, filterReception.Lessor, filterReception.Tenant, filterReception.Delegation, filterReception.Agent, currentPage, rowNumberForPage);
                maxNumberPage = _context.MaxNumberPagination;
                _context.ListPageInPaginate = CreatePaginationNumber();
            }
            StateHasChanged();
        }
        public async void HandlePaginationForPage(int numberPage)
        {
            actasRecepcions = null;
            _context.ActasRecepcionList = null;

            var filterReception = _context.CurrentFilterPagination;
            _context.NumberPaginationCurrent = numberPage;
            currentPage = numberPage;

            if (filterReception.StartDay is not null && filterReception.EndDay is not null)
            {
                string auxS = filterReception.StartDay.Value.Date.ToString("yyyy-MM-dd");
                string auxE = filterReception.EndDay.Value.Date.ToString("yyyy-MM-dd");
                actasRecepcions = await _reception.GetReceptionCertificatesAsync(auxS, auxE, filterReception.CertificateType, filterReception.PropertyType, filterReception.NumberOfRooms, filterReception.Lessor, filterReception.Tenant, filterReception.Delegation, filterReception.Agent, currentPage, rowNumberForPage);
                maxNumberPage = _context.MaxNumberPagination;
                _context.ListPageInPaginate = CreatePaginationNumber();
            }
            else
            {
                actasRecepcions = await _reception.GetReceptionCertificatesAsync(null, null, filterReception.CertificateType, filterReception.PropertyType, filterReception.NumberOfRooms, filterReception.Lessor, filterReception.Tenant, filterReception.Delegation, filterReception.Agent, currentPage, rowNumberForPage);
                maxNumberPage = _context.MaxNumberPagination;
                _context.ListPageInPaginate = CreatePaginationNumber();
            }
            StateHasChanged();
        }
        private List<int> CreatePaginationNumber()
        {
            List<int> paginas = new List<int>();
            for(int i = 1; i <= maxNumberPage; i++)
            {
                paginas.Add(i);
            }            
            return paginas;
        }
    }
}
