using FrontEnd.Interfaces;
using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using SharedLibrary.Models;

namespace FrontEnd.Components
{
    public partial class ReceptionCertificatesPedingOrHistorical : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly IReceptionCertificateService _reception;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ITenantService _tenantService;
        private readonly ILessorService _lessorService;
        public ReceptionCertificatesPedingOrHistorical(ApplicationContext context, IReceptionCertificateService reception, IPropertyTypeService propertyTypeService, ITenantService tenantService, ILessorService lessorService)
        {
            _context = context;
            _reception = reception;
            _propertyTypeService = propertyTypeService;
            _tenantService = tenantService;
            _lessorService = lessorService;
        }

        [Parameter]
        public string TypeTableReception { get; set; } = "";
        [Parameter]
        public List<ActasRecepcion> ActasRecepcions { get; set; } 
        protected override async Task OnInitializedAsync()
        {    
        }

 
    }
}
