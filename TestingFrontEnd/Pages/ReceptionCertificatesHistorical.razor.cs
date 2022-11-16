using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;
using FrontEnd.Interfaces;
using SharedLibrary.Models;

namespace FrontEnd.Pages
{
    public partial class ReceptionCertificatesHistorical : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly IReceptionCertificateService _reception;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ITenantService _tenantService;
        private readonly ILessorService _lessorService;


        private List<Tenant> tenants { get; set; }
        private List<Lessor> lessors { get; set; }
        private List<ActasRecepcion>? actasRecepcions { get; set; }
        private List<PropertyType> propertyTypes { get; set; }
        private DateTime? startDay { get; set; } = null;
        private DateTime? endDay { get; set; } = null;
        private int? certificateType { get; set; } = null;
        private int? propertyType { get; set; } = null;
        private int? numberOfRooms { get; set; } = null;
        private int? lessor { get; set; } = null;
        private int? tenant { get; set; } = null;
        private string? delegation { get; set; } = null;
        private string? agent { get; set; } = null;
        private int? currentPage { get; set; } = null;
        private int? rowNumber { get; set; } = null;

        public ReceptionCertificatesHistorical(ApplicationContext context, IReceptionCertificateService reception, IPropertyTypeService propertyTypeService, ITenantService tenantService, ILessorService lessorService)
        {
            _context = context;
            _reception = reception;
            _propertyTypeService = propertyTypeService;
            _tenantService = tenantService;
            _lessorService = lessorService;
        }
        protected override async Task OnInitializedAsync()
        {
            tenants = await _tenantService.GetTenantAsync();
            lessors = await _lessorService.GetLessorAsync();
            propertyTypes = await _propertyTypeService.GetPropertyTypeAsync();
            actasRecepcions = await _reception.GetReceptionCertificatesAsync(null, null, null, null, null, null, null, null, null, null, null);
        }        
        public async Task Filter()
        {
            actasRecepcions = null;
            _context.ActasRecepcionList = null;

            if (startDay is not null && endDay is not null)
            {
                string auxS = startDay.Value.Date.ToString("yyyy-MM-dd");
                string auxE = endDay.Value.Date.ToString("yyyy-MM-dd");


                actasRecepcions = await _reception.GetReceptionCertificatesAsync(auxS, auxE, certificateType, propertyType, numberOfRooms, lessor, tenant, delegation, agent, currentPage, rowNumber);
            }
            else
            {
                actasRecepcions = await _reception.GetReceptionCertificatesAsync(null, null, certificateType, propertyType, numberOfRooms, lessor, tenant, delegation, agent, currentPage, rowNumber);
            }
        }
    }
}
