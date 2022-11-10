
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;
using TestingFrontEnd.Interfaces;
using TestingFrontEnd.Stores;


namespace TestingFrontEnd.Pages
{
    public partial class CreateReceptionCertificates : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly ITenantService _tenantService;
        private readonly ILessorService _lessorService;
        private readonly IPropertyService _propertyService;

        public CreateReceptionCertificates(ApplicationContext context, ITenantService tenantService, IPropertyService propertyService, ILessorService lessorService)
        {
            _context = context;
            _tenantService = tenantService;
            _lessorService = lessorService;
            _propertyService = propertyService;
        }

        public bool ShowModalLessor { get; set; } = false;
        public bool ShowModalTenant { get; set; } = false;
        public bool ShowModalProperty { get; set; } = false;
        public Lessor? CurrentLessor { get; set; } = new Lessor();
        public Tenant? CurrentTenant { get; set; } = new Tenant();
        public Property? CurrentProperty { get; set; } = new Property();
        public List<Lessor> lessors { get; set; }
        private List<Tenant> tenants { get; set; }
        private List<Property> properties { get; set; }

        public bool IsFormLessorExit { get; set; } = false;
        public bool IsFormTenantExit { get; set; } = false;
        public bool IsFormPropertyExit { get; set; } = false;

        public void ChangeOpenModalLessor() => ShowModalLessor = ShowModalLessor ? false : true;
        public void ChangeOpenModalTenant() => ShowModalTenant = ShowModalTenant ? false : true;
        public void ChangeOpenModalProperty() => ShowModalProperty = ShowModalProperty ? false : true;

        public void SetLessorForm(int IdLessor)
        {
            CurrentLessor = lessors.Find(x => x.IdLessor == IdLessor);
            IsFormLessorExit = true;
            ShowModalLessor = false;
            _context.CurrentLessor = CurrentLessor;
        }

        public void SetTenantForm(int IdTenant)
        {
            CurrentTenant = tenants.Find(x => x.IdTenant == IdTenant);
            IsFormTenantExit = true;
            ShowModalTenant = false;
            _context.CurrentTenant = CurrentTenant;
        }
        public void SetPropertyForm(int IdProperty)
        {
            CurrentProperty = properties.Find(x => x.IdProperty == IdProperty);            
            IsFormPropertyExit = true;
            ShowModalProperty = false;
            _context.CurrentPropertys = CurrentProperty;
        }
        protected override async Task OnInitializedAsync()
        {
            tenants = await _tenantService.GetTenantAsync();        
            lessors = await _lessorService.GetLessorAsync();
            properties = await _propertyService.GetPropertyAsync();
        }

    }
}
