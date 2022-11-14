using FrontEnd.Components.Signatures;
using FrontEnd.Interfaces;
using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Shared.Models;
using SharedLibrary.Models;

namespace FrontEnd.Pages
{
    public partial class ReceptionCertificatesSignatures : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly NavigationManager _navigate;
        private readonly ITenantService _tenantService;
        private readonly ILessorService _lessorService;
        private readonly IPropertyService _propertyService;
        private readonly IInventoryService _inventoryService;
        private readonly IServicesService _servicesService;
        private readonly IAreaService _areaService;
        private readonly IDescriptionService _descriptionService;
        private readonly IFeaturesService _featuresService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IReceptionCertificateService _receptionCertificateService;

        public ReceptionCertificatesSignatures(ApplicationContext context, NavigationManager navigate, ITenantService tenantService, IPropertyService propertyService, ILessorService lessorService, IInventoryService inventoryService, IServicesService servicesService, IAreaService areaService, IDescriptionService descriptionService, IFeaturesService featuresService, IPropertyTypeService propertyTypeService, IReceptionCertificateService receptionCertificateService)
        {
            _context = context;
            _navigate = navigate;
            _tenantService = tenantService;
            _propertyService = propertyService;
            _lessorService = lessorService;
            _inventoryService = inventoryService;
            _servicesService = servicesService;
            _areaService = areaService;
            _descriptionService = descriptionService;
            _featuresService = featuresService;
            _propertyTypeService = propertyTypeService;
            _receptionCertificateService = receptionCertificateService;
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
        private ReceptionCertificate CurrentReceptionCertificate { get; set; }
        public string ImageBase64Lessor { get; set; }
        public string ImageBase64Tenant { get; set; }
        public string Observaciones { get; set; }

        public SignaturesLessor signaturesLessorComponent;
        public SignaturesTenant signaturesTenantComponent;

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
            //CurrentReceptionCertificate = _context.CurrentReceptionCertificate;                        
            CurrentReceptionCertificate = new ReceptionCertificate
            {
                IdReceptionCertificate = 8,
                CreationDate = DateTime.Now,
                IdAgent = "1e6d90d6-32b5-43af-bc6a-0b43678462ec",
                IdProperty = 1,
                IdTenant = 1,
                IdTypeRecord = 1,
            };
        }
        public async void HandleInsertSignatures()
        {
            ImageBase64Lessor = await signaturesLessorComponent._context.ToDataURLAsync();
            ImageBase64Tenant = await signaturesTenantComponent._context.ToDataURLAsync();
            CurrentReceptionCertificate.Observation = Observaciones;
            CurrentReceptionCertificate.ApprovarPathLessor = ImageBase64Lessor;
            CurrentReceptionCertificate.ApprovalPathTenant = ImageBase64Tenant;
            var r = await _receptionCertificateService.PutReceptionCertificatesAsync(CurrentReceptionCertificate);

            if(r is null)
            {
                _navigate.NavigateTo("/Emails");
            }
            
        }
    }
}

