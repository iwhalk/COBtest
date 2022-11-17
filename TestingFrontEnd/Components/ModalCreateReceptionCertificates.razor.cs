using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components
{
    public partial class ModalCreateReceptionCertificates : ComponentBase
    {

        private readonly ApplicationContext _context;
        [Parameter]
        public bool ShowModal { get; set; } = false;

        public string NumberReceptionCertificate { get; set; }

        public ModalCreateReceptionCertificates(ApplicationContext context)
        {
            _context = context;
        }
        protected override async Task OnInitializedAsync()
        {
            NumberReceptionCertificate = _context.CurrentReceptionCertificate.ContractNumber;
        }

    }
}
