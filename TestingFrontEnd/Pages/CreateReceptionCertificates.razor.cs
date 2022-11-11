using FrontEnd.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;
using FrontEnd.Interfaces;
using FrontEnd.Components.Propertys;
using FrontEnd.Components.Tenants;
using FrontEnd.Components.Lessors;
using SharedLibrary.Models;
using Shared.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace FrontEnd.Pages
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
        public int MyProperty { get; set; }

        public void ChangeOpenModalLessor() => ShowModalLessor = ShowModalLessor ? false : true;
        public void ChangeOpenModalTenant() => ShowModalTenant = ShowModalTenant ? false : true;
        public void ChangeOpenModalProperty() => ShowModalProperty = ShowModalProperty ? false : true;

        public ReceptionCertificate NewCreateReceptionCertificate { get; set; } = new ReceptionCertificate();

        public FormLessor formLessor;
        public FormTenant formTenant;
        public FormProperty formProperty;

        public void SetLessorForm(int IdLessor)
        {
            CurrentLessor = lessors.Find(x => x.IdLessor == IdLessor);            
            ShowModalLessor = false;
            _context.CurrentLessor = CurrentLessor;
        }
        public void SetTenantForm(int IdTenant)
        {
            CurrentTenant = tenants.Find(x => x.IdTenant == IdTenant);            
            ShowModalTenant = false;
            _context.CurrentTenant = CurrentTenant;
        }
        public void SetPropertyForm(int IdProperty)
        {
            CurrentProperty = properties.Find(x => x.IdProperty == IdProperty);                        
            ShowModalProperty = false;
            _context.CurrentPropertys = CurrentProperty;
        }
        public async void HandlePostCreateCertificates()
        {
            MyProperty = 1000;
            if (formLessor.LessorEditContext.Validate() && formTenant.TenantEditContext.Validate() && formProperty.PropertyEditContext.Validate())
            {
                try
                {
                    MyProperty = 10;
                    if (CurrentLessor.IdLessor == 0)
                    {
                        //Crear nuewvo lessor                    
                        await _lessorService.PostLessorAsync(CurrentLessor);
                        await _lessorService.GetLessorAsync();
                    }
                    if (CurrentProperty.IdProperty == 0)
                    {
                        //Crear nuevo property con idLessor                    
                        CurrentProperty.IdLessor = CurrentLessor.IdLessor;
                        await _propertyService.PostPropertyAsync(CurrentProperty);
                        await _propertyService.GetPropertyAsync();
                    }
                    if (CurrentTenant.IdTenant == 0)
                    {
                        //Crear nuevo tenant                    
                        await _tenantService.PostTenantAsync(CurrentTenant);
                        await _tenantService.GetTenantAsync();
                    }
                    NewCreateReceptionCertificate.IdTenant = CurrentTenant.IdTenant;
                    NewCreateReceptionCertificate.IdProperty = CurrentProperty.IdProperty;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
        }
        protected override async Task OnInitializedAsync()
        {
            tenants = await _tenantService.GetTenantAsync();        
            lessors = await _lessorService.GetLessorAsync();
            properties = await _propertyService.GetPropertyAsync();
        }
    }
}
