using FrontEnd.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;
using FrontEnd.Interfaces;

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

        private List<Tenant> tenants { get; set; }
        private List<Lessor> lessors { get; set; }
        private List<Property> properties { get; set; }

        public void ChangeOpenModalLessor() => ShowModalLessor = ShowModalLessor ? false : true;
        public void ChangeOpenModalTenant() => ShowModalTenant = ShowModalTenant ? false : true;
        public void ChangeOpenModalProperty() => ShowModalProperty = ShowModalProperty ? false : true;

        protected override async Task OnInitializedAsync()
        {
            tenants = await _tenantService.GetTenantAsync();        
            lessors = await _lessorService.GetLessorAsync();
            properties = await _propertyService.GetPropertyAsync();
        }
    }
}
