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
        private readonly NavigationManager _navigationManager;
        private readonly IReceptionCertificateService _reception;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ITenantService _tenantService;
        private readonly ILessorService _lessorService;
        public ReceptionCertificatesPedingOrHistorical(ApplicationContext context, NavigationManager navigationManager, IReceptionCertificateService reception, IPropertyTypeService propertyTypeService, ITenantService tenantService, ILessorService lessorService)
        {
            _context = context;
            _reception = reception;
            _navigationManager = navigationManager;
            _propertyTypeService = propertyTypeService;
            _tenantService = tenantService;
            _lessorService = lessorService;
        }    

        [Parameter]
        public string TypeTableReception { get; set; } = "";
        [Parameter]
        public List<ActasRecepcion> ActasRecepcions { get; set; }
        [Parameter]
        public EventCallback<int> PreviewPdf { get; set; }
        [Parameter]
        public EventCallback<int> RedirectReception { get; set; }


    }
}
