using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using FrontEnd.Interfaces;
using SharedLibrary.Models;

namespace FrontEnd.Pages
{
    public partial class ReceptionCertificatesHistorical : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly ITenantService _tenantService;
        private readonly ILessorService _lessorService;
        private readonly IPropertyService _propertyService;
        private readonly IInventoryService _inventoryService;
        private readonly IServicesService _servicesService;
        private readonly IAreaService _areaService;
        private readonly IDescriptionService _descriptionService;
        private readonly IFeaturesService _featuresService;
        private readonly IPropertyTypeService _propertyTypeService;

        public ReceptionCertificatesHistorical(ApplicationContext context, ITenantService tenantService, IPropertyService propertyService, ILessorService lessorService, IInventoryService inventoryService, IServicesService servicesService, IAreaService areaService, IDescriptionService descriptionService, IFeaturesService featuresService, IPropertyTypeService propertyTypeService)
        {
            _context = context;
            _tenantService = tenantService;
            _propertyService = propertyService;
            _lessorService = lessorService;
            _inventoryService = inventoryService;
            _servicesService = servicesService;
            _areaService = areaService;
            _descriptionService = descriptionService;
            _featuresService = featuresService;
            _propertyTypeService = propertyTypeService;
        }

        private List<Tenant> tenants { get; set; }
        private List<Lessor> lessors { get; set; }
        private List<Property> properties { get; set; }
        private List<Inventory> inventories { get; set; }
        private List<Service> services { get; set; }
        private List<Area> areas { get; set; }
        private List<Description> descriptions { get; set; }
        private List<Feature> features { get; set; }
        private List<PropertyType> propertyTypes { get; set; }

        protected override async Task OnInitializedAsync()
        {
            tenants = await _tenantService.GetTenantAsync();
            lessors = await _lessorService.GetLessorAsync();
            properties = await _propertyService.GetPropertyAsync();
            inventories = await _inventoryService.GetInventoryAsync();
            services = await _servicesService.GetServicesAsync();
            areas = await _areaService.GetAreaAsync();
            descriptions = await _descriptionService.GetDescriptionAsync();
            features = await _featuresService.GetFeaturesAsync();
            propertyTypes = await _propertyTypeService.GetPropertyTypeAsync();
        }
    }
}
