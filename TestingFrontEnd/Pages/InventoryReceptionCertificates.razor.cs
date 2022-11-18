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
using System.Reflection.Metadata.Ecma335;

namespace FrontEnd.Pages
{
    public partial class InventoryReceptionCertificates : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly NavigationManager _navigate;
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
                                              IBlobsInventoryService blobsInventoryService,
                                              NavigationManager navigate)
        {
            _context = context;
            _inventoryService = inventoryService;
            _servicesService = servicesService;
            _areaService = areaService;
            _featuresService = featuresService;
            _descriptionService = descriptionService;
            _blobService = blobService;
            _blobsInventoryService = blobsInventoryService;
            _navigate = navigate;
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
        public int FeatureMedidorCurrent { get; set; } //BLoquea las demas filas que nosea este IdFeature
        public string TypeButtonsInventory { get; set; } = "";
        public string NameKey { get; set; }
        public string? NameMedidor { get; set; }

        private Inventory CurrentInventory { get; set; } = new();
        private Area CurrentArea { get; set; } = new();
        public List<AreaService>? areaServices { get; private set; }
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
            CurrentArea = AreasList.FirstOrDefault();
            foreach (var service in Services.Take(4))
            {
                CurrentArea.AreaServices.Add(new() { IdService = service.IdService, IdArea = CurrentArea.IdArea });
            }
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

            if (CurrentArea.AreaServices.Count == 0)
            {
                foreach (var service in Services.Take(4))
                {
                    CurrentArea.AreaServices.Add(new() { IdService = service.IdService, IdArea = CurrentArea.IdArea });
                }
            }
            //CurrentArea.AreaServices.Add();
            //areaServices = (await _areaService.GetAreaServicesAsync())?.Where(x=>x.IdArea==idArea).Take(4).ToList();

            //ServicesList = Services.Where(x => areaServices.Any(y => x.IdService.Equals(y.IdService))).ToList();
            ServicesList = Services.Where(x => CurrentArea.AreaServices.Any(y => x.IdService.Equals(y.IdService))).ToList();
        }
        public void AgregarServicios()
        {
            foreach (var selectedService in modalServices.SelectedValues)
            {
                if (ServicesList.Find(x => x.IdService == selectedService.IdService) == null)
                    ServicesList.Add(selectedService);
            }

            foreach (var serviceSelected in ServicesList)
            {
                CurrentArea.AreaServices.Add(new() { IdService = serviceSelected.IdService, IdArea = CurrentArea.IdArea });
            }
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
            FeatureMedidorCurrent = IdFeature;
            StateHasChanged();

            var CurrentDTO = dtoDescriptions.FirstOrDefault(x => x.IdFeature == IdFeature);
            CurrentDTO.Note = e.Value.ToString();
            CurrentFeature = FeaturesList.FirstOrDefault(x => x.IdFeature == IdFeature);
            DescriptionsList = (await _descriptionService.GetDescriptionAsync())?.Where(x => x.IdFeature == IdFeature)?.ToList();

            if (!string.IsNullOrEmpty(CurrentDTO.Observation))
            {

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
                FeatureMedidorCurrent = 0;//Reinicamos el FEatureBAndera para desbloqeuar todos lo input
                StateHasChanged();
            }
            StateHasChanged();
            return;
        }
        public async void DescriptionObservationInput(int IdFeature, ChangeEventArgs e)
        {
            FeatureMedidorCurrent = IdFeature;
            StateHasChanged();

            var currentDTO = dtoDescriptions.FirstOrDefault(x => x.IdFeature == IdFeature);
            currentDTO.Observation = e.Value.ToString();
            CurrentFeature = FeaturesList.FirstOrDefault(x => x.IdFeature == IdFeature);
            DescriptionsList = (await _descriptionService.GetDescriptionAsync())?.Where(x => x.IdFeature == IdFeature)?.ToList();

            if(!string.IsNullOrEmpty(currentDTO.Note)) //Si tiene nota no insertar el inventory
            {
                CurrentInventory.Observation = e.Value.ToString();
                CurrentInventory.IdProperty = _context.CurrentReceptionCertificate.IdProperty;
                CurrentInventory.IdArea = CurrentArea.IdArea;
                CurrentInventory.IdDescription = DescriptionsList.FirstOrDefault()?.IdDescription ?? 1;

                InventoriesList.Add(CurrentInventory);          

                var res = await _inventoryService.PostInventoryAsync(CurrentInventory);
                CurrentInventory.IdInventory = res.IdInventory;
                LastInventoryAdded = res.IdInventory;
                
                CurrentInventory = new();
                DescriptionsList = new();
                FeatureMedidorCurrent = 0;//Reinicamos el FEatureBAndera para desbloqeuar todos lo input
                StateHasChanged();
            }
            StateHasChanged();
            return;

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
        public async void HandlePostNewService(string nameArea)
        {
            Service? newService = new Service { ServiceName = nameArea };
            newService = await _servicesService.PostServicesAsync(newService);
            if (newService != null)
            {
                ServicesList.Add(newService);
            }
            ShowModalComponents = false;
            StateHasChanged();
        }
        public async void HandlePostNewArea(string nameArea)
        {
            Area? newArea = new Area { AreaName = nameArea };
            newArea =  await _areaService.PostAreaAsync(newArea);
            if(newArea != null)
            {
                AreasList.Add(newArea);                
            }
            ShowModalRooms = false;
            StateHasChanged();
        }
        public async void HandlePostNewMedidor()
        {
            if (NameMedidor != "")
            {
                Feature? newFeature = new Feature { FeatureName = NameMedidor, IdService = 13 };
                newFeature = await _featuresService.PostFeaturesAsync(newFeature);
                if (newFeature != null)
                {
                    FeaturesList.Add(newFeature);
                    StateHasChanged();
                }
            }
            NameMedidor = "";
            return;
        }
        public async void HandlePostNewKey()
        {
            if (NameKey != "")
            {
                Feature? newFeature = new Feature { FeatureName = NameKey, IdService = 14 };
                newFeature = await _featuresService.PostFeaturesAsync(newFeature);
                if (newFeature != null)
                {
                    FeaturesList.Add(newFeature);
                    StateHasChanged();
                }
            }
            NameKey = "";
            return;
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
        public void GenerarActa()
        {
            _context.CurrentAreasList = AreasList;
            _navigate.NavigateTo("/ReceptionCertificate/Signatures");
        }
        public class DtoDescription
        {            
            public int IdDescription { get; set; }
            public int IdService { get; set; }
            public int IdFeature { get; set; }
            public string Description { get; set; }
            public string Note { get; set; }
            public string Observation { get; set; }
        }
    }
}
