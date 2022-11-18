using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using SharedLibrary;
using SharedLibrary.Models;
using FrontEnd.Interfaces;
using FrontEnd.Services;
using FrontEnd.Components;
using System.Xml.Linq;
using FrontEnd.Components.Blobs;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

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
        private readonly IBlobService _blobService;
        private readonly IBlobsInventoryService _blobsInventoryService;

        public InventoryReceptionCertificates(ApplicationContext context,
                                              IInventoryService inventoryService,
                                              IServicesService servicesService,
                                              IAreaService areaService,
                                              IFeaturesService featuresService,
                                              IDescriptionService descriptionService,
                                              IBlobService blobService,
                                              IBlobsInventoryService blobsInventoryService)
        {
            _context = context;
            _inventoryService = inventoryService;
            _servicesService = servicesService;
            _areaService = areaService;
            _featuresService = featuresService;
            _descriptionService = descriptionService;
            _blobService = blobService;
            _blobsInventoryService = blobsInventoryService;
        }

        private ModalAreas modalAreas;
        private ModalServices modalServices;
        private FormBlob FormBlob;

        public string MaterialSelect { get; set; } = "";
        public string ColorSelect { get; set; } = "";
        public string StatusSelect { get; set; } = "";

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
        public Blob NewBlob { get; set; } = new();
        private BlobsInventory BlobsInventory { get; set; } = new();
        public Property CurrentProperty { get; private set; }


        private List<Service> Services { get; set; } = new();
        private List<Description> Descriptions { get; set; } = new();
        private List<Inventory> inventories { get; set; }
        private List<Service> ServicesList { get; set; } = new();
        public List<Area> AreasList { get; set; } = new();
        public List<Feature> FeaturesList { get; set; } = new();
        public List<Description> DescriptionsList { get; set; } = new();
        public List<Inventory> InventoriesList { get; set; } = new();
        public List<DtoDescription> dtoDescriptions { get; set; } = new();

        public int[] FeaturesIds { get; set; } = { 2, 5, 7, 11, 15, 25, 30, 33, 60};
        public int LastInventoryAdded;

        public void ChangeOpenModalRooms() => ShowModalRooms = ShowModalRooms ? false : true;
        public void ChangeOpenModalComponents() => ShowModalComponents = ShowModalComponents ? false : true;
        public void ChangeOpenModalCopyValues() => ShowModalCopyValues = ShowModalCopyValues ? false : true;
        public void ChangeOpenModalGauges()
        {
            //ServicesList = Services.Where(x => CurrentArea.IdServices.Any(y => x.IdService.Equals(y.IdService))).ToList();
            ServiceButtonClicked(13);
            ShowModalGauges = ShowModalGauges ? false : true;
        }

        public void ChangeOpenModalKeys()
        {
            ServiceButtonClicked(14);
            ShowModalkeys = ShowModalkeys ? false : true;
        }

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
            Services = await _servicesService.GetServicesAsync();
            Descriptions = await _descriptionService.GetDescriptionAsync();


            //CurrentPropertyId = _context.CurrentReceptionCertificate.IdProperty;


            AreasList = (await _areaService.GetAreaAsync())?.Take(4).ToList();
            //ServicesList = (await _servicesService.GetServicesAsync())?.Take(3).ToList();
        }
        public void AgregarAreas()
        {
            foreach (var selectedArea in modalAreas.SelectedValues)
            {
                if (AreasList.Find(x => x.IdArea == selectedArea.IdArea) == null)
                    AreasList.Add(selectedArea);
            }

            ShowModalRooms = false;
        }
        public async void AreaButtonClicked(int idArea)
        {
            CurrentArea = AreasList.FirstOrDefault(x => x.IdArea == idArea);

            var areaServices = (await _areaService.GetAreaServicesAsync())?.Where(x=>x.IdArea==idArea).Take(4);

            ServicesList = Services.Where(x => areaServices.Any(y => x.IdService.Equals(y.IdService))).ToList();
            //ServicesList = Services.Where(x => CurrentArea.AreaServices.Any(y => x.IdService.Equals(y.IdService))).ToList();
        }
        public void AgregarServicios()
        {
            foreach (var selectedService in modalServices.SelectedValues)
            {
                if (ServicesList.Find(x => x.IdService == selectedService.IdService) == null)
                    ServicesList.Add(selectedService);
            }

            //foreach (var serviceSelected in ServicesList)
            //{
            //    CurrentArea.AreaServices.Add(new() { IdService = serviceSelected.IdService});
            //}
            modalServices.SelectedValues = new();

            ShowModalComponents = false;
            StateHasChanged();
        }
        public async void ServiceButtonClicked(int idService)
        {
            ////var bres = await _blobService.PostBlobAsync(FormBlob.CurrentBlobFile);
            //if (FormBlob.CurrentBlobFile.Blob.IdBlobs != null && FormBlob.CurrentBlobFile.Blob.IdBlobs != 0)
            //{
            //    BlobsInventory.IdBlobs = FormBlob.CurrentBlobFile.Blob.IdBlobs;
            //    BlobsInventory.IdInventory = CurrentInventory.IdInventory;
            //    BlobsInventory.IdProperty = _context.CurrentPropertys.IdProperty;
            //    var res = _blobsInventoryService.PostBlobsInventoryAsync(BlobsInventory);
            //}

            CurrentService = ServicesList.FirstOrDefault(x => x.IdService == idService);
            FeaturesList = (await _featuresService.GetFeaturesAsync())?.Where(x => x.IdService == idService)?.ToList();
            foreach (var feature in FeaturesList)
            {
                dtoDescriptions.Add(new() { IdFeature = feature.IdFeature, IdService = idService });
            }
            StateHasChanged();
        }
        public async void FeatureButtonClicked(int IdFeature)
        {
            //CurrentInventory = new();
            CurrentFeature = FeaturesList.FirstOrDefault(x => x.IdFeature == IdFeature);
            DescriptionsList = (await _descriptionService.GetDescriptionAsync())?.Where(x => x.IdFeature == IdFeature)?.ToList();
            StateHasChanged();
        }

        public async void DescriptionNoteInput(int IdFeature, ChangeEventArgs e)
        {
            dtoDescriptions.FirstOrDefault(x => x.IdFeature == IdFeature).Note = e.Value.ToString();
            CurrentFeature = FeaturesList.FirstOrDefault(x => x.IdFeature == IdFeature);
            DescriptionsList = (await _descriptionService.GetDescriptionAsync())?.Where(x => x.IdFeature == IdFeature)?.ToList();

            CurrentInventory.Note = e.Value.ToString();
            CurrentInventory.IdProperty = _context.CurrentReceptionCertificate.IdProperty;
            CurrentInventory.IdArea = CurrentArea.IdArea;
            CurrentInventory.IdDescription = DescriptionsList.FirstOrDefault()?.IdDescription ?? 1;

            InventoriesList.Add(CurrentInventory);

            //dtoDescriptions.FirstOrDefault(x => x.IdFeature == CurrentFeature.IdFeature).IdDescription = idDescription;
            //dtoDescriptions.FirstOrDefault(x => x.IdDescription == idDescription).Note = name;
            //DescriptionsList = new();

            var res = await _inventoryService.PostInventoryAsync(CurrentInventory);
            CurrentInventory.IdInventory = res.IdInventory;
            LastInventoryAdded = res.IdInventory;
            //if (FormBlob.CurrentBlobFile.Blob.IdBlobs != null && FormBlob.CurrentBlobFile.Blob.IdBlobs != 0)
            //{
            //    BlobsInventory.IdBlobs = FormBlob.CurrentBlobFile.Blob.IdBlobs;
            //    BlobsInventory.IdInventory = CurrentInventory.IdInventory;
            //    BlobsInventory.IdProperty = _context.CurrentReceptionCertificate.IdProperty;
            //    _blobsInventoryService.PostBlobsInventoryAsync(BlobsInventory);
            //    NewBlob = new();
            //}

            //var bres = await _blobService.PostBlobAsync(FormBlob.CurrentBlobFile);
            //BlobsInventory.IdBlobs = bres.IdBlobs;
            //BlobsInventory.IdInventory = res.IdInventory;
            //BlobsInventory.IdProperty = _context.CurrentPropertys.IdProperty;
            CurrentInventory = new();
            DescriptionsList = new();
            StateHasChanged();
        }

        public async void DescriptionButtonClicked(int idDescription)
        {
            var name = Descriptions?.FirstOrDefault(x => x.IdDescription == idDescription)?.DescriptionName;

            //CurrentInventory = new();
            CurrentInventory.IdProperty = _context.CurrentReceptionCertificate.IdProperty;
            CurrentInventory.IdArea = CurrentArea.IdArea;
            CurrentInventory.IdDescription = idDescription;
            //CurrentInventory.Note = name;

            //var newInventory = new Inventory { IdArea = CurrentArea.IdArea, IdProperty = 2, IdDescription = idDescription, Note = name, };
            //CurrentInventory = newInventory;
            InventoriesList.Add(CurrentInventory);

            dtoDescriptions.FirstOrDefault(x => x.IdFeature == CurrentFeature.IdFeature).IdDescription = idDescription;
            dtoDescriptions.FirstOrDefault(x => x.IdDescription == idDescription).Description = name;
            //DescriptionsList = new();

            var res = await _inventoryService.PostInventoryAsync(CurrentInventory);
            CurrentInventory.IdInventory = res.IdInventory;
            LastInventoryAdded = res.IdInventory;

            //if (FormBlob.CurrentBlobFile.Blob.IdBlobs != null && FormBlob.CurrentBlobFile.Blob.IdBlobs != 0)
            //{
            //    BlobsInventory.IdBlobs = FormBlob.CurrentBlobFile.Blob.IdBlobs;
            //    BlobsInventory.IdInventory = CurrentInventory.IdInventory;
            //    BlobsInventory.IdProperty = _context.CurrentReceptionCertificate.IdProperty;
            //    _blobsInventoryService.PostBlobsInventoryAsync(BlobsInventory);
            //    NewBlob = new();
            //}
            CurrentInventory = new();
            DescriptionsList = new();
            StateHasChanged();
        }
        public void RemoveArea(int IdArea)
        {
            AreasList.Remove(CurrentArea);
        }
        public void RemoveService(int IdService)
        {
            ServicesList.Remove(CurrentService);
        }
        public void AddInventoryBlob(int IdBlob)
        {
            BlobsInventory.IdBlobs = IdBlob;
            BlobsInventory.IdInventory = LastInventoryAdded;
            BlobsInventory.IdProperty = _context.CurrentReceptionCertificate.IdProperty;
            _blobsInventoryService.PostBlobsInventoryAsync(BlobsInventory);
        }
        public class DtoDescription
        {
            public int IdDescription { get; set; }
            public int IdService { get; set; }
            public int IdFeature { get; set; }
            public string Description { get; set; }
            public string Note { get; set; }
        }
    }
}
