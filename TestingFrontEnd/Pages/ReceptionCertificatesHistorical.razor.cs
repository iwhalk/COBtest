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

        public ReceptionCertificatesHistorical(ApplicationContext context, IReceptionCertificateService reception, IPropertyTypeService propertyTypeService, ITenantService tenantService, ILessorService lessorService)
        {
            _context = context;
            _reception = reception;
            _propertyTypeService = propertyTypeService;
            _tenantService = tenantService;
            _lessorService = lessorService;
        }

        private List<ActasRecepcion> actasRecepcions { get; set; }
        private List<PropertyType> propertyTypes { get; set; }
        private List<Tenant> tenants { get; set; }
        private List<Lessor> lessors { get; set; }

        protected override async Task OnInitializedAsync()
        {
            tenants = await _tenantService.GetTenantAsync();
            lessors = await _lessorService.GetLessorAsync();
            propertyTypes = await _propertyTypeService.GetPropertyTypeAsync();
            actasRecepcions = await _reception.GetReceptionCertificatesAsync(null, null, null, null, null, null, null, null, null, null, null);
        }
    }
}
