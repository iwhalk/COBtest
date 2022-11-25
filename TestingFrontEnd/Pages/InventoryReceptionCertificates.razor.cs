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
using System;

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
        public List<Feature> Features { get; private set; }
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
        public ReceptionCertificate CurrentReceptionCertificate { get; private set; }
        public int? CurrentReceptionCertificateId { get; private set; }
        private List<Inventory> inventories { get; set; }
        private List<Service> ServicesList { get; set; } = new();
        public List<Area> AreasList { get; set; } = new();
        public List<Feature> FeaturesList { get; set; } = new();
        public List<Description> DescriptionsList { get; set; } = new();
        public List<Inventory> InventoriesList { get; set; } = new();
        public List<DtoDescription> dtoDescriptions { get; set; } = new();
        public List<Area> SelectedValues { get; set; } = new();

        public int[] FeaturesIds { get; set; } = { 2, 5, 7, 11, 15, 25, 30, 33, 60};
        public int LastInventoryAdded;

        public void ChangeOpenModalRooms() => ShowModalRooms = ShowModalRooms ? false : true;
        public void ChangeOpenModalComponents() => ShowModalComponents = ShowModalComponents ? false : true;
        public void ChangeOpenModalCopyValues()
        {
            ShowModalCopyValues = ShowModalCopyValues ? false : true;
        }
        public void OnClickCopyValues()
        {
            var dtosToCopy = dtoDescriptions.Where(x => x.IdArea == CurrentArea.IdArea && x.IdService == CurrentService.IdService).ToList();
            List<DtoDescription> newDtos = new();
            foreach (var selectedArea in SelectedValues.Where(x => x.IdArea != CurrentArea.IdArea).ToList())
            {
                foreach (var dto in dtosToCopy)
                {
                    DtoDescription newDto = new()
                    {
                        IdDescription = dto.IdDescription,
                        IdService = (int)dto.IdService,
                        IdFeature = dto.IdFeature,
                        IdArea = selectedArea.IdArea,
                        Observation = dto.Observation,
                        Note = dto.Note,
                        Description = dto.Description
                    };
                    newDtos.Add(newDto);
                                    }
            }
            foreach (var item in newDtos)
            {
                dtoDescriptions.Add(item);
            }
        }
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
            Features = await _featuresService.GetFeaturesAsync();
            CurrentReceptionCertificate = _context.CurrentReceptionCertificate ?? _context.ReceptionCertificateExist;
            var NewAreasList =  (await _areaService.GetAreaAsync())?.ToList();
            CurrentArea = NewAreasList.FirstOrDefault(x => x.IdArea == 11);

            if (_context.ReceptionCertificateExist != null)
            {
                var inventorys = (await _inventoryService.GetInventoryAsync()).ToList();
                inventorys = inventorys.Where(x => x.IdReceptionCertificate == _context.ReceptionCertificateExist.IdReceptionCertificate).ToList();                
                var ListIdAreas = inventorys.GroupBy(p => p.IdArea).Select(g => g.FirstOrDefault().IdArea).ToList();
                var ListIdServices = inventorys.GroupBy(p => p.IdService).Select(g => g.FirstOrDefault()).ToList();
                //foreach (var item in ListIdServices)
                //{
                //    Area newArea = NewAreasList.FirstOrDefault(x => x.IdArea == item.IdArea);
                //    newArea.AreaServices.Add(new() { IdService = (int)item.IdService, IdArea = item.IdArea });

                //    AreasList.Add(newArea);
                //}

                foreach (var idArea in ListIdAreas)
                {
                    Area newArea = NewAreasList.FirstOrDefault(x => x.IdArea == idArea);
                    foreach (var item in inventorys.Where(x=>x.IdArea==idArea))
                    {
                        newArea.AreaServices.Add(new() { IdService = (int)item.IdService, IdArea = item.IdArea });
                    }

                    AreasList.Add(newArea);
                }
                foreach (var item in inventorys)
                {
                    DtoDescription dto = new()
                    {
                        IdDescription = item.IdDescription,
                        IdService = (int)item.IdService,
                        IdFeature = (int)item.IdFeature,
                        IdArea = item.IdArea,
                        Observation = item.Observation,
                        Note = item.Note,
                        Description = Descriptions.FirstOrDefault(x => x.IdFeature == item.IdFeature)?.DescriptionName
                    };
                    dtoDescriptions.Add(dto);
                }
                //foreach (var idArea in ListIdAreas)
                //{
                //    AreasList.Add(NewAreasList.First(x => x.IdArea == idArea));
                //}


            }
            else {
                //CurrentPropertyId = _context.CurrentReceptionCertificate.IdProperty;
                AreasList = NewAreasList.Take(4).ToList();
                CurrentArea = AreasList.FirstOrDefault();
                foreach (var service in Services.Take(4))
                {
                    CurrentArea.AreaServices.Add(new() { IdService = service.IdService, IdArea = CurrentArea.IdArea });
                }
                AreasList.Add(NewAreasList.FirstOrDefault(x => x.IdArea == 11));
                AreasList.FirstOrDefault(x=>x.IdArea==11).AreaServices.Add(new() { IdService = 13, IdArea = 11 });
                //ServicesList = (await _servicesService.GetServicesAsync())?.Take(3).ToList();
            }
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
            CurrentInventory = new();
            CurrentArea = AreasList.FirstOrDefault(x => x.IdArea == idArea);

            if (CurrentArea.AreaServices.Count == 0)
            {
                foreach (var service in Services.Take(4))
                {
                    CurrentArea.AreaServices.Add(new() { IdService = service.IdService, IdArea = CurrentArea.IdArea });
                }
                CurrentArea.AreaServices.Add(new() { IdService = 13, IdArea = 11 });
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
            CurrentInventory = new();
            ////var bres = await _blobService.PostBlobAsync(FormBlob.CurrentBlobFile);
            //if (FormBlob.CurrentBlobFile.Blob.IdBlobs != null && FormBlob.CurrentBlobFile.Blob.IdBlobs != 0)
            //{
            //    BlobsInventory.IdBlobs = FormBlob.CurrentBlobFile.Blob.IdBlobs;
            //    BlobsInventory.IdInventory = CurrentInventory.IdInventory;
            //    BlobsInventory.IdProperty = _context.CurrentPropertys.IdProperty;
            //    var res = _blobsInventoryService.PostBlobsInventoryAsync(BlobsInventory);
            //}

            CurrentService = ServicesList.FirstOrDefault(x => x.IdService == idService);
            if (CurrentService == null) { 
                ServicesList.Add(Services.FirstOrDefault(x => x.IdService == idService));
                CurrentService = ServicesList.FirstOrDefault(x => x.IdService == idService);
            }
            FeaturesList = Features?.Where(x => x.IdService == idService)?.ToList();
            if(idService == 14)
                FeaturesList = FeaturesList.Where(x => AreasList.Any(y => x.FeatureName.Equals(y.AreaName))).ToList();
            //if(dtoDescriptions.Count < 1)
            foreach (var feature in FeaturesList)
            {          
                if(dtoDescriptions.FirstOrDefault(x => x.IdFeature == feature.IdFeature && x.IdService == CurrentService.IdService && x.IdArea == CurrentArea.IdArea) == null)
                dtoDescriptions.Add(new() { IdFeature = feature.IdFeature, IdService = idService, IdArea = CurrentArea.IdArea });
            }
            if (FormBlob != null)
            {
                if (FormBlob.ListBase64Blobs != null)
                {
                    FormBlob.ListBase64Blobs = new();
                }
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
            var CurrentDTO = dtoDescriptions.FirstOrDefault(x => x.IdFeature == IdFeature && x.IdService == CurrentService.IdService && x.IdArea == CurrentArea.IdArea);
            if (CurrentDTO != null)
            {
                CurrentDTO.Note = e.Value.ToString();
            }
            else
            {
                dtoDescriptions.Add(new() { IdFeature = IdFeature, IdService = CurrentService.IdService, IdArea = CurrentArea.IdArea });
                CurrentDTO = dtoDescriptions.FirstOrDefault(x => x.IdFeature == IdFeature && x.IdService == CurrentService.IdService && x.IdArea == CurrentArea.IdArea);
                CurrentDTO.Note = e.Value.ToString();
            }
            CurrentFeature = FeaturesList.FirstOrDefault(x => x.IdFeature == IdFeature);
            DescriptionsList = (await _descriptionService.GetDescriptionAsync())?.Where(x => x.IdFeature == IdFeature)?.ToList();

            //if (!string.IsNullOrEmpty(CurrentDTO.Observation))
            //{

            CurrentInventory.Note = e.Value.ToString();
            CurrentInventory.IdProperty = CurrentReceptionCertificate.IdProperty;
            CurrentInventory.IdArea = CurrentFeature?.IdService == 14 ? 11 : CurrentArea.IdArea;
            CurrentInventory.IdDescription = DescriptionsList.FirstOrDefault()?.IdDescription ?? 1;
            CurrentInventory.IdReceptionCertificate = CurrentReceptionCertificate.IdReceptionCertificate;
            CurrentInventory.IdService = CurrentService?.IdService ?? 14;
            CurrentInventory.IdFeature = IdFeature;

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
                //FeatureMedidorCurrent = 0;//Reinicamos el FEatureBAndera para desbloqeuar todos lo input                
            //}
            //StateHasChanged();
            //return;
        }
        public async void DescriptionModalInput(int IdFeature, string type, ChangeEventArgs e)
        {
            FeatureMedidorCurrent = IdFeature;
            StateHasChanged();

            var currentDTO = dtoDescriptions.FirstOrDefault(x => x.IdFeature == IdFeature);
            if(type == "Note")
            {
                currentDTO.Note = e.Value.ToString();
            }
            else
            {
                currentDTO.Observation = e.Value.ToString();
            }

            CurrentFeature = FeaturesList.FirstOrDefault(x => x.IdFeature == IdFeature);
            DescriptionsList = (await _descriptionService.GetDescriptionAsync())?.Where(x => x.IdFeature == IdFeature)?.ToList();

            if(!string.IsNullOrEmpty(currentDTO.Note) && !string.IsNullOrEmpty(currentDTO.Observation)) //Si tiene nota no insertar el inventory
            {

                CurrentInventory.Observation = currentDTO.Observation;
                CurrentInventory.Note = currentDTO.Note;
                CurrentInventory.IdProperty = CurrentReceptionCertificate.IdProperty;
                CurrentInventory.IdArea = 11;
                CurrentInventory.IdDescription = DescriptionsList.FirstOrDefault()?.IdDescription ?? 1;
                CurrentInventory.IdReceptionCertificate = CurrentReceptionCertificate.IdReceptionCertificate;
                CurrentInventory.IdService = 13;
                CurrentInventory.IdFeature = IdFeature;

                InventoriesList.Add(CurrentInventory);          

                var res = await _inventoryService.PostInventoryAsync(CurrentInventory);
                CurrentInventory.IdInventory = res.IdInventory;
                LastInventoryAdded = res.IdInventory;
                
                CurrentInventory = new();
                DescriptionsList = new();
                FeatureMedidorCurrent = 0;//Reinicamos el FEatureBAndera para desbloqeuar todos lo input                
            }
            StateHasChanged();
            return;

        }
        public async void DescriptionButtonClicked(int idDescription)
        {
            var name = Descriptions?.FirstOrDefault(x => x.IdDescription == idDescription)?.DescriptionName;

            //CurrentInventory = new();
            CurrentInventory.IdProperty = CurrentReceptionCertificate.IdProperty;
            CurrentInventory.IdArea = CurrentArea.IdArea;
            CurrentInventory.IdDescription = idDescription;
            CurrentInventory.IdReceptionCertificate = CurrentReceptionCertificate.IdReceptionCertificate;
            CurrentInventory.IdService = CurrentService.IdService;
            CurrentInventory.IdFeature = CurrentFeature.IdFeature;
            //CurrentInventory.Note = name;

            //var newInventory = new Inventory { IdArea = CurrentArea.IdArea, IdProperty = 2, IdDescription = idDescription, Note = name, };
            //CurrentInventory = newInventory;
            InventoriesList.Add(CurrentInventory);

            var dtoDesc = dtoDescriptions.FirstOrDefault(x => x.IdFeature == CurrentFeature.IdFeature && x.IdService == CurrentService.IdService && x.IdArea == CurrentArea.IdArea);
            if (dtoDesc != null)
            {
                dtoDescriptions.FirstOrDefault(x => x.IdFeature == CurrentFeature.IdFeature && x.IdService == CurrentService.IdService && x.IdArea == CurrentArea.IdArea).IdDescription = idDescription;
            }
            else
            {
                dtoDescriptions.Add(new() { IdFeature = CurrentFeature.IdFeature, IdService = CurrentService.IdService, IdArea = CurrentArea.IdArea });
                dtoDescriptions.FirstOrDefault(x => x.IdFeature == CurrentFeature.IdFeature && x.IdService == CurrentService.IdService && x.IdArea == CurrentArea.IdArea).IdDescription = idDescription;
            }
            
            dtoDescriptions.FirstOrDefault(x => x.IdDescription == idDescription && x.IdService == CurrentService.IdService && x.IdArea == CurrentArea.IdArea).Description = name;
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
        public void CheckboxClicked(Area aSelectedId, object aChecked)
        {
            if ((bool)aChecked)
            {
                if (!SelectedValues.Contains(aSelectedId))
                {
                    SelectedValues.Add(aSelectedId);
                }
            }
            else
            {
                if (SelectedValues.Contains(aSelectedId))
                {
                    SelectedValues.Remove(aSelectedId);
                }
            }
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
            CurrentArea.AreaServices.Remove(CurrentArea.AreaServices.FirstOrDefault(x=>x.IdService==IdService));
        }
        public void AddInventoryBlob(int IdBlob)
        {
            BlobsInventory.IdBlobs = IdBlob;
            BlobsInventory.IdInventory = LastInventoryAdded;
            BlobsInventory.IdProperty = CurrentReceptionCertificate.IdProperty;
            _blobsInventoryService.PostBlobsInventoryAsync(BlobsInventory);
        }
        public void GenerarActa()
        {
            _context.CurrentAreasList = AreasList;
            _navigate.NavigateTo("/ReceptionCertificate/Signatures");
        }
        public async void HandleSaveReceptionCertificate()
        {
            _navigate.NavigateTo("/");
            //HandleInsertSignatures();
        }
        public class DtoDescription
        {            
            public int IdDescription { get; set; }
            public int IdService { get; set; }
            public int IdFeature { get; set; }
            public int IdArea { get; set; }
            public string Description { get; set; }
            public string Note { get; set; }
            public string Observation { get; set; }
        }
    }
}
