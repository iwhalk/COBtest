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

        [Parameter]
        public EventCallback ChageFilters { get; set; }
        [Parameter]
        public DateTime? StartDay { get; set; } = null;
        [Parameter]
        public DateTime? EndDay { get; set; } = null;
        [Parameter]
        public int? CertificateType { get; set; } = null;
        [Parameter]
        public int? PropertyType { get; set; } = null;
        [Parameter]
        public int? NumberOfRooms { get; set; } = null;
        [Parameter]
        public int? Lessor { get; set; } = null;
        [Parameter]
        public int? Tenant { get; set; } = null;
        [Parameter]
        public string? Delegation { get; set; } = null;
        [Parameter]
        public string? Agent { get; set; } = null;
        //[Parameter]
        //public int? currentPage { get; set; } = null;
        //[Parameter]
        //public int? rowNumber { get; set; } = null;

        public HeaderReceptionCertificatePendingOrHistorical(ApplicationContext context, IReceptionCertificateService reception, IPropertyTypeService propertyTypeService, ITenantService tenantService, ILessorService lessorService)
        {
            _context = context;
            _reception = reception;
            _propertyTypeService = propertyTypeService;
            _tenantService = tenantService;

        }
        protected override async Task OnInitializedAsync()
        {
            tenants = await _tenantService.GetTenantAsync();
            lessors = await _lessorService.GetLessorAsync();
        }
    }
}
