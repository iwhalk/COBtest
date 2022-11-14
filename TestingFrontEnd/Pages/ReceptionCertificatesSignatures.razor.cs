﻿using FrontEnd.Components.Signatures;
using FrontEnd.Interfaces;
using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using SharedLibrary.Models;

namespace FrontEnd.Pages
{
    public partial class ReceptionCertificatesSignatures : ComponentBase
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

        public ReceptionCertificatesSignatures(ApplicationContext context, ITenantService tenantService, IPropertyService propertyService, ILessorService lessorService, IInventoryService inventoryService, IServicesService servicesService, IAreaService areaService, IDescriptionService descriptionService, IFeaturesService featuresService, IPropertyTypeService propertyTypeService)
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
        private ReceptionCertificate CurrentReceptionCertificate { get; set; }
        public string ImageBase64Lessor { get; set; }
        public string ImageBase64Tenant { get; set; }
        public string Observacionbes { get; set; }

        public SignaturesLessor signaturesLessorComponent = new SignaturesLessor(); 

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

            CurrentReceptionCertificate = _context.CurrentReceptionCertificate;            
        }
        public async void HandleInsertSignature()
        {
            await signaturesLessorComponent.ImageAsync();

        }
    }
}

