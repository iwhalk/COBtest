using FrontEnd.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;
using FrontEnd.Interfaces;
using FrontEnd.Components.Propertys;
using FrontEnd.Components.Tenants;
using FrontEnd.Components.Lessors;
using SharedLibrary.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Authorization;

namespace FrontEnd.Pages
{
    public partial class CreateReceptionCertificates : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly NavigationManager _navigation;
        private readonly ITenantService _tenantService;
        private readonly ILessorService _lessorService;
        private readonly IPropertyService _propertyService;
        private readonly IReceptionCertificateService _receptionCertificateService;
        private readonly IUserService _userService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly AuthenticationStateProvider _getAuthenticationStateAsync;
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        public CreateReceptionCertificates(ApplicationContext context, NavigationManager navigationManager, ITenantService tenantService, IPropertyService propertyService, ILessorService lessorService, IReceptionCertificateService receptionCertificateService, AuthenticationStateProvider getAuthenticationStateAsync, IUserService userService, IPropertyTypeService propertyTypeService)
        {
            _context = context;
            _navigation = navigationManager;
            _tenantService = tenantService;
            _lessorService = lessorService;
            _propertyService = propertyService;
            _receptionCertificateService = receptionCertificateService;
            _getAuthenticationStateAsync = getAuthenticationStateAsync;
            _userService = userService;
            _propertyTypeService = propertyTypeService;
        }

        public bool ShowModalLessor { get; set; } = false;
        public bool ShowModalTenant { get; set; } = false;
        public bool ShowModalProperty { get; set; } = false;
        public Lessor? CurrentLessor { get; set; } = new Lessor();
        public Tenant? CurrentTenant { get; set; } = new Tenant();
        public Property? CurrentProperty { get; set; } = new Property();
        public List<Lessor> lessors { get; set; } = new();
        private List<Tenant> tenants { get; set; } = new();
        private List<Property> properties { get; set; } = new();
        private List<AspNetUser> users { get; set; } = new();

        private List<PropertyType> propertyTypes = new();

        public int MyProperty { get; set; }
        public string UserId { get; set; }

        public void ChangeOpenModalLessor() => ShowModalLessor = ShowModalLessor ? false : true;       
        public void ChangeOpenModalTenant() => ShowModalTenant = ShowModalTenant ? false : true;        
        public void ChangeOpenModalProperty() => ShowModalProperty = ShowModalProperty ? false : true;        
        public ReceptionCertificate NewReceptionCertificate { get; set; } = new ReceptionCertificate { CreationDate = DateTime.Now };

        public FormLessor formLessor;
        public FormTenant formTenant;
        public FormProperty formProperty;

        public IEnumerable<string> lessorValid { get; set; }
        public IEnumerable<string> tenantValid { get; set; }
        public IEnumerable<string> propertyValid { get; set; }
        public void SetLessorForm(int IdLessor)
        {
            CurrentLessor = lessors.Find(x => x.IdLessor == IdLessor);            
            ShowModalLessor = false;
            _context.CurrentLessor = CurrentLessor ?? new Lessor();
        }
        public void SetTenantForm(int IdTenant)
        {
            CurrentTenant = tenants.Find(x => x.IdTenant == IdTenant);            
            ShowModalTenant = false;
            _context.CurrentTenant = CurrentTenant ?? new Tenant();
        }
        public void SetPropertyForm(int IdProperty)
        {
            CurrentProperty = properties.Find(x => x.IdProperty == IdProperty);                        
            ShowModalProperty = false;
            _context.CurrentPropertys = CurrentProperty ?? new Property();
        }
        public async void HandlePostCreateCertificates()
        {
            MyProperty = 1000;
            lessorValid = formLessor.LessorEditContext.GetValidationMessages();
            tenantValid = formTenant.TenantEditContext.GetValidationMessages();
            propertyValid = formProperty.PropertyEditContext.GetValidationMessages();

            if (NewReceptionCertificate.IdReceptionCertificate != 0)
            {
                _navigation.NavigateTo("/ReceptionCertificates/Inventory");
                return;
            }

            if (!string.IsNullOrEmpty(NewReceptionCertificate.ContractNumber)) 
            {
                try
                {
                    MyProperty = 10;
                    if (CurrentLessor.IdLessor == 0)
                    {   //Crear nuewvo lessor                    
                        CurrentLessor =  await _lessorService.PostLessorAsync(CurrentLessor);
                        if(CurrentLessor == null)
                        {
                            CurrentLessor = new();
                            return;
                        }
                        _context.LessorList.Add(CurrentLessor);                        
                    }
                    if (CurrentProperty.IdProperty == 0)
                    {   //Crear nuevo property con idLessor                    
                        CurrentProperty.IdLessor = CurrentLessor.IdLessor;
                        CurrentProperty = await _propertyService.PostPropertyAsync(CurrentProperty);
                        if (CurrentProperty == null)
                        {
                            CurrentProperty = new();
                            return;
                        }
                        _context.PropertyList.Add(CurrentProperty);                        
                    }
                    if (CurrentTenant.IdTenant == 0)
                    {   //Crear nuevo tenant                    
                        CurrentTenant = await _tenantService.PostTenantAsync(CurrentTenant);
                        if(CurrentTenant == null)
                        {
                            CurrentTenant = new();
                            return;
                        }
                        _context.TenantList.Add(CurrentTenant);
                    }                                
                    NewReceptionCertificate.IdTenant = CurrentTenant.IdTenant;
                    NewReceptionCertificate.IdProperty = CurrentProperty.IdProperty;
                    NewReceptionCertificate.IdTypeRecord = _context.TypeReceptionCertificate; //For ReceptionCertificate (1)In or (2)Out
                    NewReceptionCertificate.IdAgent = UserId;                    
                    _context.CurrentReceptionCertificate = await _receptionCertificateService.PostReceptionCertificatesAsync(NewReceptionCertificate);
                    _navigation.NavigateTo("/ReceptionCertificates/Inventory");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return;
        }
        protected override async Task OnInitializedAsync()
        {
            tenants = await _tenantService.GetTenantAsync();        
            lessors = await _lessorService.GetLessorAsync();
            properties = await _propertyService.GetPropertyAsync();
            users = await _userService.GetUsersAsync();
            propertyTypes = await _propertyTypeService.GetPropertyTypeAsync();            

            if (_context.CurrentReceptionCertificate != null)
            {
                NewReceptionCertificate = _context.CurrentReceptionCertificate;
                SetPropertyForm(NewReceptionCertificate.IdProperty);
                SetTenantForm(NewReceptionCertificate.IdTenant);
                SetLessorForm(CurrentProperty.IdLessor);
                UserId = NewReceptionCertificate.IdAgent;
                _context.CurrentUser = users.FirstOrDefault(x => x.Id == NewReceptionCertificate.IdAgent);
            }
            else
            {
                CurrentProperty.IdPropertyType = propertyTypes[0].IdPropertyType;

                var authstate = await _getAuthenticationStateAsync.GetAuthenticationStateAsync();
                UserId = authstate.User.Claims.FirstOrDefault(x => x.Type.Equals("sub")).Value;
                _context.CurrentUser = users.FirstOrDefault(x => x.Id == UserId);
            }
        }
    }
}
