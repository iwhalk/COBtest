﻿using FrontEnd.Components.Signatures;
using FrontEnd.Interfaces;
using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
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
        private readonly IAreasService _areaService;
        private readonly IDescriptionsService _descriptionService;
        private readonly IFeaturesService _featuresService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IReceptionCertificateService _receptionCertificateService;
        private readonly IReportsService _reportService;
        private readonly IUserService _userService;

        public ReceptionCertificatesSignatures(ApplicationContext context, NavigationManager navigate, IReportsService reportsService, ITenantService tenantService, IPropertyService propertyService, ILessorService lessorService, IInventoryService inventoryService, IServicesService servicesService, IAreasService areaService, IDescriptionsService descriptionService, IFeaturesService featuresService, IPropertyTypeService propertyTypeService, IReceptionCertificateService receptionCertificateService, IUserService userService)
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
            _reportService = reportsService;
            _userService = userService;
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
        public List<AspNetUser> Agents { get; private set; }
        public byte[]? BlobPDFPreview { get; set; }
        public string PdfName { get; set; }
        public bool ShowModalPreview { get; set; } = false;
        public bool DisablePreView { get; set; } = false;
        public string ImageBase64Lessor { get; set; }
        public string ImageBase64Tenant { get; set; }
        public string Observaciones { get; set; }
        public SignaturesLessor signaturesLessorComponent;
        public SignaturesTenant signaturesTenantComponent;
        public void ChangeOpenModalPreview() => ShowModalPreview = ShowModalPreview ? false : true;

        public string nameTenant { get; set; }
        public string nameLessor { get; set; }
        public string NameAgent { get; set; }
        public string dateAct { get; set; }

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
            CurrentReceptionCertificate = _context.CurrentReceptionCertificate ?? _context.ReceptionCertificateExist;
            Agents = await _userService.GetUsersAsync();

            var nameT = tenants.FirstOrDefault(x => x.IdTenant.Equals(CurrentReceptionCertificate.IdTenant)).Name;
            var lastNameT = tenants.FirstOrDefault(x => x.IdTenant.Equals(CurrentReceptionCertificate.IdTenant)).LastName;
            nameTenant = nameT + " " + lastNameT;

            var aux = properties.FirstOrDefault(x => x.IdProperty.Equals(CurrentReceptionCertificate.IdProperty)).IdLessor;
            var nameL = lessors.FirstOrDefault(x => x.IdLessor.Equals(aux)).Name;
            var lastNameL = lessors.FirstOrDefault(x => x.IdLessor.Equals(aux)).LastName;
            nameLessor = nameL + " " + lastNameL;

            var agent = Agents.FirstOrDefault(x => x.Id == CurrentReceptionCertificate.IdAgent);
            NameAgent = agent?.Name + " " + agent?.LastName;

            dateAct = CurrentReceptionCertificate.CreationDate.ToString("dd-MM-yyyy HH:mm:ss");
        }
        public async void HandlePreviewPdf()
        {
            DisablePreView = true;
            if (CurrentReceptionCertificate != null)
            {
                var IdReceptionCertificate = CurrentReceptionCertificate.IdReceptionCertificate;
                BlobPDFPreview = await _reportService.GetReporteReceptionCertificate(IdReceptionCertificate);
                if (BlobPDFPreview != null)
                {
                    PdfName = "PDFPreview.pdf";
                    Thread.Sleep(5000);
                }
                StateHasChanged();
            }
            DisablePreView = false;
            ShowModalPreview = true;
            StateHasChanged();
        }
        public async void HandleSaveReceptionCertificate()
        {
            _navigate.NavigateTo("/");
            //HandleInsertSignatures();
        }
        public async void HandleInsertSignatures()
        {
            ImageBase64Lessor = await signaturesLessorComponent._context.ToDataURLAsync();
            ImageBase64Tenant = await signaturesTenantComponent._context.ToDataURLAsync();
            CurrentReceptionCertificate.Observation = Observaciones;
            CurrentReceptionCertificate.ApprovarPathLessor = ImageBase64Lessor;
            CurrentReceptionCertificate.ApprovalPathTenant = ImageBase64Tenant;
            CurrentReceptionCertificate = await _receptionCertificateService.PutReceptionCertificatesAsync(CurrentReceptionCertificate);

            if (CurrentReceptionCertificate is null)
            {
                _navigate.NavigateTo("/Emails");
            }
            else
            {
                _context.CurrentReceptionCertificate = CurrentReceptionCertificate;
                _navigate.NavigateTo("/Emails");
            }
        }
    }
}

