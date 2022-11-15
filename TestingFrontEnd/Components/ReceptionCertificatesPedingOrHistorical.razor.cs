using FrontEnd.Interfaces;
using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using SharedLibrary.Models;

namespace FrontEnd.Components
{
    public partial class ReceptionCertificatesPedingOrHistorical : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly IReceptionCertificateService _reception;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ITenantService _tenantService;
        private readonly ILessorService _lessorService;
        public ReceptionCertificatesPedingOrHistorical(ApplicationContext context, IReceptionCertificateService reception, IPropertyTypeService propertyTypeService, ITenantService tenantService, ILessorService lessorService)
        {
            _context = context;
            _reception = reception;
            _propertyTypeService = propertyTypeService;
            _tenantService = tenantService;
            _lessorService = lessorService;
        }

        [Parameter]
        public string TypeTableReception { get; set; } = "";

        private List<ActasRecepcion>? actasRecepcions { get; set; }
        private List<PropertyType> propertyTypes { get; set; }
        private List<Tenant> tenants { get; set; }
        private List<Lessor> lessors { get; set; }

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
            _context.ActasRecepcion = null;

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
