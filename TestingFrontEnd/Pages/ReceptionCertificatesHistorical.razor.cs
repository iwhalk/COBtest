using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using FrontEnd.Interfaces;

namespace FrontEnd.Pages
{
    public partial class ReceptionCertificatesHistorical : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly IReceptionCertificateService _reception;

        public ReceptionCertificatesHistorical(ApplicationContext context, IReceptionCertificateService reception)
        {
            _context = context;
            _reception = reception;
        }

        private List<ActasRecepcion> actasRecepcions { get; set; }

        protected override async Task OnInitializedAsync()
        {
            actasRecepcions = await _reception.GetReceptionCertificatesAsync(null, null, null, null, null, null, null, null, null, null, null);
        }
    }
}
