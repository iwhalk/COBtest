using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using SharedLibrary;
using SharedLibrary.Models;
using FrontEnd.Interfaces;
using FrontEnd.Services;
using FrontEnd.Components;
using System.Xml.Linq;

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

        private Inventory CurrentInventory { get; set; } = new();
        private Area CurrentArea { get; set; } = new();
        private Service CurrentService { get; set; } = new();
        private Description CurrentDescription { get; set; } = new();
        private Feature CurrentFeature { get; set; } = new();
        private DtoDescription CurrentDtoDescription { get; set; } = new();

        private List<Service> Services { get; set; } = new();
        private List<Description> Descriptions { get; set; } = new();

        private List<Inventory> inventories { get; set; }
        private List<Service> ServicesList { get; set; } = new();
        public List<Area> AreasList { get; set; } = new();
        public List<Feature> FeaturesList { get; set; } = new();
        public List<Description> DescriptionsList { get; set; } = new();
        public List<Inventory> InventoriesList { get; set; } = new();
        public List<DtoDescription> dtoDescriptions { get; set; } = new();

        public int[] FeaturesIds { get; set; } = { 2, 7, 11, 15, 25, 30, 33, 60};

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

        public void SetColor(string newColor) => ColorSelect = newColor;
        public void SetMaterial(string newMaterial) => MaterialSelect = newMaterial;
        public void SetStatus(string newStatus) => StatusSelect = newStatus;

        protected override async Task OnInitializedAsync()
        {
            inventories = await _inventoryService.GetInventoryAsync();
            Services = await _servicesService.GetServicesAsync();
            Descriptions = await _descriptionService.GetDescriptionAsync();

            AreasList = (await _areaService.GetAreaAsync())?.Take(4).ToList();
            //ServicesList = (await _servicesService.GetServicesAsync())?.Take(3).ToList();
        }
        public void AgregarAreas()
        {
            foreach (var selectedArea in modalAreas.SelectedValues)
            {
                AreasList.Add(selectedArea);
            }

            ShowModalRooms = false;
        }
        public void AgregarServicios()
        {
            foreach (var selectedService in modalServices.SelectedValues)
            {
                ServicesList.Add(selectedService);
            }

            foreach (var serviceSelected in ServicesList)
            {
                CurrentArea.IdServices.Add(new() { IdService = serviceSelected.IdService});
            }
            modalServices.SelectedValues = new();

            ShowModalComponents = false;
        }
        public async void AreaButtonClicked(int idArea)
        {
            CurrentArea = AreasList.FirstOrDefault(x => x.IdArea == idArea);
            ServicesList = Services.Where(x => CurrentArea.IdServices.Any(y => x.IdService.Equals(y.IdService))).ToList();
        }
        public async void ServiceButtonClicked(int idService)
        {
            CurrentService = ServicesList.FirstOrDefault(x => x.IdService == idService);
            FeaturesList = (await _featuresService.GetFeaturesAsync())?.Where(x => x.IdService == idService)?.ToList();
            foreach (var feature in FeaturesList)
            {
                dtoDescriptions.Add(new() { IdFeature = feature.IdFeature, IdService = idService });
            }
        }
        public async void FeatureButtonClicked(int IdFeature)
        {
            //CurrentInventory = new();
            CurrentFeature = FeaturesList.FirstOrDefault(x => x.IdFeature == IdFeature);
            DescriptionsList = (await _descriptionService.GetDescriptionAsync())?.Where(x => x.IdFeature == IdFeature)?.ToList();

        }

        public async void FeatureInput(int IdFeature)
        {
            CurrentFeature = FeaturesList.FirstOrDefault(x => x.IdFeature == IdFeature);

            CurrentInventory.IdProperty = _context.CurrentPropertys.IdProperty;
            CurrentInventory.IdArea = CurrentArea.IdArea;

            InventoriesList.Add(CurrentInventory);

            //dtoDescriptions.FirstOrDefault(x => x.IdFeature == CurrentFeature.IdFeature).IdDescription = idDescription;
            //dtoDescriptions.FirstOrDefault(x => x.IdDescription == idDescription).Note = name;
            //DescriptionsList = new();

            var res = await _inventoryService.PostInventoryAsync(CurrentInventory);
        }

        public async void DescriptionButtonClicked(int idDescription)
        {
            var name = Descriptions?.FirstOrDefault(x => x.IdDescription == idDescription)?.DescriptionName;
            CurrentInventory.IdProperty = _context.CurrentPropertys.IdProperty;
            CurrentInventory.IdArea = CurrentArea.IdArea;
            CurrentInventory.IdDescription = idDescription;
            //CurrentInventory.Note = name;

            //var newInventory = new Inventory { IdArea = CurrentArea.IdArea, IdProperty = 2, IdDescription = idDescription, Note = name, };
            //CurrentInventory = newInventory;
            InventoriesList.Add(CurrentInventory);

            dtoDescriptions.FirstOrDefault(x => x.IdFeature == CurrentFeature.IdFeature).IdDescription = idDescription;
            dtoDescriptions.FirstOrDefault(x => x.IdDescription == idDescription).Note = name;
            //DescriptionsList = new();

            var res = await _inventoryService.PostInventoryAsync(CurrentInventory);
        }

        public class DtoDescription
        {
            public int IdDescription { get; set; }
            public int IdService { get; set; }
            public int IdFeature { get; set; }
            public string Note { get; set; }
        }
    }
}
