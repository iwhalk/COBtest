using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using SharedLibrary;
using SharedLibrary.Models;
using FrontEnd.Interfaces;
using FrontEnd.Services;
using FrontEnd.Components;

namespace FrontEnd.Pages
{
    public partial class InventoryReceptionCertificates : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly IInventoryService _inventoryService;
        private readonly IServicesService _servicesService;
        private readonly IAreaService _areaService;
        private readonly IFeaturesService _featuresService;
        private readonly IDescriptionService _descriptionService;

        public InventoryReceptionCertificates(ApplicationContext context,
                                              IInventoryService inventoryService,
                                              IServicesService servicesService,
                                              IAreaService areaService,
                                              IFeaturesService featuresService,
                                              IDescriptionService descriptionService)
        {
            _context = context;
            _inventoryService = inventoryService;
            _servicesService = servicesService;
            _areaService = areaService;
            _featuresService = featuresService;
            _descriptionService = descriptionService;
        }

        private ModalAreas modalAreas;
        private ModalServices modalServices;

        public string MaterialSelect { get; set; } = "";
        public string ColorSelect { get; set; } = "";
        public string StatusSelect { get; set; } = "";
        public int Cantaida { get; set; }
        public int M2 { get; set; }

        public bool ShowModalRooms { get; set; } = false;
        public bool ShowModalComponents { get; set; } = false;
        public bool ShowModalCopyValues { get; set; } = false;
        public bool ShowModalGauges { get; set; } = false;
        public bool ShowModalkeys { get; set; } = false;
        public string TypeButtonsInventory { get; set; } = "";

        private List<Inventory> inventories { get; set; }
        private List<Service> ServicesList { get; set; } = new();
        public List<Area> AreasList { get; set; } = new();
        public List<Feature> FeaturesList { get; set; } = new();
        public List<Description> DescriptionsList { get; set; } = new();

        public void ChangeOpenModalRooms() => ShowModalRooms = ShowModalRooms ? false : true;
        public void ChangeOpenModalComponents() => ShowModalComponents = ShowModalComponents ? false : true;
        public void ChangeOpenModalCopyValues() => ShowModalCopyValues = ShowModalCopyValues ? false : true;
        public void ChangeOpenModalGauges() => ShowModalGauges = ShowModalGauges ? false : true;
        public void ChangeOpenModalKeys() => ShowModalkeys = ShowModalkeys ? false : true;

        public void ChangeButtonsShow(string newButtonsShow)
        {
            TypeButtonsInventory = newButtonsShow;

            if (newButtonsShow == "Material")
                MaterialSelect = "";
            if (newButtonsShow == "Color")
                ColorSelect = "";
            if (newButtonsShow == "EstadoGeneral")
                StatusSelect = "";
        }
        public async void ChangeDescriptionButtons(int IdFeature)
        {
            DescriptionsList = (await _descriptionService.GetDescriptionAsync())?.Where(x => x.IdFeature == IdFeature)?.ToList();
        }

        public void SetColor(string newColor) => ColorSelect = newColor;
        public void SetMaterial(string newMaterial) => MaterialSelect = newMaterial;
        public void SetStatus(string newStatus) => StatusSelect = newStatus;

        protected override async Task OnInitializedAsync()
        {
            inventories = await _inventoryService.GetInventoryAsync();
            FeaturesList = (await _featuresService.GetFeaturesAsync())?.Where(x => x.IdService == 1)?.ToList();

        }
        public void AgregarAreas()
        {
            AreasList = modalAreas.SelectedValues;
        }
        public void AgregarServicios()
        {
            ServicesList = modalServices.SelectedValues;
        }
    }
}
