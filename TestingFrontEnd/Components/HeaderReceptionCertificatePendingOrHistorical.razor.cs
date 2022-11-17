using FrontEnd.Interfaces;
using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;

namespace FrontEnd.Components
{
    public partial class HeaderReceptionCertificatePendingOrHistorical : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly IReceptionCertificateService _reception;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ITenantService _tenantService;
        private readonly ILessorService _lessorService;
        private List<Tenant> tenants { get; set; }
        private List<Lessor> lessors { get; set; }
        private List<PropertyType> propertyTypes { get; set; }
        public FilterReceptionCertificate FilterReception { get; set; } = new();
        [Parameter]
        public EventCallback<FilterReceptionCertificate> ChageFilters { get; set; }        
        public class FilterReceptionCertificate
        {                                   
            public DateTime? StartDay { get; set; } = null;            
            public DateTime? EndDay { get; set; } = null;            
            public int? CertificateType { get; set; } = null;            
            public int? PropertyType { get; set; } = null;            
            public int? NumberOfRooms { get; set; } = null;            
            public int? Lessor { get; set; } = null;            
            public int? Tenant { get; set; } = null;            
            public string? Delegation { get; set; } = null;            
            public string? Agent { get; set; } = null;
        }

        public HeaderReceptionCertificatePendingOrHistorical(ApplicationContext context, IReceptionCertificateService reception, IPropertyTypeService propertyTypeService, ITenantService tenantService, ILessorService lessorService)
        {
            _context = context;
            _reception = reception;
            _propertyTypeService = propertyTypeService;
            _tenantService = tenantService;
            _lessorService= lessorService;

        }
        protected override async Task OnInitializedAsync()
        {
            tenants = await _tenantService.GetTenantAsync();
            lessors = await _lessorService.GetLessorAsync();
            propertyTypes = await _propertyTypeService.GetPropertyTypeAsync();
        }
    }
}
