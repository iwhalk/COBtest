using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components
{
    public partial class ModalCreateReceptionCertificates : ComponentBase
    {

        private readonly ApplicationContext _context;
        private readonly NavigationManager _navigationManager;
        [Parameter]
        public bool ShowModal { get; set; } = false;

        public string NumberReceptionCertificate { get; set; }

        public ModalCreateReceptionCertificates(ApplicationContext context, NavigationManager navigationManager)
        {
            _context = context;
            _navigationManager = navigationManager;
        }
        protected override async Task OnInitializedAsync()
        {
            NumberReceptionCertificate = _context.CurrentReceptionCertificate.ContractNumber;
        }

        private void RedirectToHome()
        {
            _context.CurrentReceptionCertificate = null;
            _context.CurrentLessor = null;
            _context.CurrentPropertys = null;
            _context.CurrentTenant = null;
            _navigationManager.NavigateTo("/");
        }

    }
}
