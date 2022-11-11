using FrontEnd.Interfaces;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace FrontEnd.Pages
{
    public partial class PruebasGateway : ComponentBase
    {
        private readonly IReceptionCertificateService _reception;
        public PruebasGateway(IReceptionCertificateService reception)
        {
            _reception = reception;
        }

        private List<ActasRecepcion> ListArea { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("si llego aca");
            ListArea = await _reception.GetReceptionCertificatesAsync(null, null, null, null, null, null, null, null, null, null, null);            
        }
    }
}
