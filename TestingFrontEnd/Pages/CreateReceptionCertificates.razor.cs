using Microsoft.AspNetCore.Components;
using Shared.Models;
using TestingFrontEnd.Services;
using TestingFrontEnd.Stores;

namespace TestingFrontEnd.Pages
{
    public partial class CreateReceptionCertificates : ComponentBase
    {
        public bool ShowModalLessor { get; set; } = false;
        public bool ShowModalTenant { get; set; } = false;
        public bool ShowModalProperty { get; set; } = false;

        private List<Tenant> tenants;
        private List<Lessor> lessors;
        private List<Property> properties;

        public void ChangeOpenModalLessor() => ShowModalLessor = ShowModalLessor ? false : true;
        public void ChangeOpenModalTenant() => ShowModalTenant = ShowModalTenant ? false : true;
        public void ChangeOpenModalProperty() => ShowModalProperty = ShowModalProperty ? false : true;

        private readonly ApplicationContext _context;
        private readonly TenantService _tenantService;
        private readonly LessorService _lessorService;
        private readonly PropertyService _propertyService;

        public CreateReceptionCertificates(ApplicationContext context, TenantService tenantService, PropertyService propertyService, LessorService lessorService)
        {
            _context = context;
            _tenantService = tenantService;
            _lessorService = lessorService;
            _propertyService = propertyService;
        }

        protected override async Task OnInitializedAsync()
        {
            tenants = await _tenantService.GetTenantAsync();
            lessors = await _lessorService.GetLessorAsync();
            properties = await _propertyService.GetPropertyAsync();

            //Pruebas
            if (tenants != null || tenants.Count() > 0)
            {
                foreach (var item in tenants)
                {
                    Console.WriteLine(item);
                }
            }

            if (lessors != null || lessors.Count() > 0)
            {
                foreach (var item in lessors)
                {
                    Console.WriteLine(item);
                }
            }

            if (properties != null || properties.Count() > 0)
            {
                foreach (var item in properties)
                {
                    Console.WriteLine(item);
                }
            }         
        }
    }
}
