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
        public int rowNumber { get; set; }

        public ReceptionCertificatesHistorical(ApplicationContext context, IReceptionCertificateService reception)
        {
            _context = context;
            _reception = reception;               
        }
        protected override async Task OnInitializedAsync()
        {
            currentPage = 1;
            rowNumberForPage = 10;                                                        
            actasRecepcions = await _reception.GetReceptionCertificatesAsync(null, null, null, null, null, null, null, null, null, currentPage, rowNumberForPage);
            rowNumber = _context.NumberPaginationCurrent;
            _context.CurremtFilterPagination = new FilterReceptionCertificate();
        }        
        public async Task Filter(FilterReceptionCertificate filterReception)
        {
            actasRecepcions = null;
            _context.ActasRecepcionList = null;            

            if (filterReception.StartDay is not null && filterReception.EndDay is not null)
            {
                string auxS = filterReception.StartDay.Value.Date.ToString("yyyy-MM-dd");
                string auxE = filterReception.EndDay.Value.Date.ToString("yyyy-MM-dd");
                actasRecepcions = await _reception.GetReceptionCertificatesAsync(auxS, auxE, filterReception.CertificateType, filterReception.PropertyType, filterReception.NumberOfRooms, filterReception.Lessor, filterReception.Tenant, filterReception.Delegation, filterReception.Agent, currentPage, rowNumberForPage);                                
                rowNumber = _context.NumberPaginationCurrent;
            }
            else
            {
                actasRecepcions = await _reception.GetReceptionCertificatesAsync(null, null, filterReception.CertificateType, filterReception.PropertyType, filterReception.NumberOfRooms, filterReception.Lessor, filterReception.Tenant, filterReception.Delegation, filterReception.Agent, currentPage, rowNumberForPage);                                
                rowNumber = _context.NumberPaginationCurrent;                
            }
            _context.CurremtFilterPagination = filterReception;
        }
        public void HandlePaginationAction(PaginationAction paginationAction)
        {
            actasRecepcions = null;
            _context.ActasRecepcionList = null;
            switch (paginationAction)
            {
                case PaginationAction.EndPage:
                    currentPage = _context.NumberPaginationCurrent;
                    Filter(_context.CurremtFilterPagination);
                break;
            }
        }
    }
}
