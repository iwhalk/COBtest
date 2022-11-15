using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using TanvirArjel.Blazor.Extensions;

namespace FrontEnd.Components
{
    public partial class ModalTypeReceptionCertificates : ComponentBase
    {
        private readonly NavigationManager _navigationManager;
        private readonly ApplicationContext _context;

        [Parameter]
        public bool ShowModal { get; set; }
        [Parameter]
        public EventCallback OnClick { get; set; }

        public ModalTypeReceptionCertificates(ApplicationContext context, NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            _context = context;
        }

        public void RedirectReceptionIn()
        {
            _context.TypeReceptionCertificate = 1;
            _navigationManager.NavigateTo("/ReceptionCertificates/Create");
        }
        public void RedirectReceptionOut()
        {
            _context.TypeReceptionCertificate = 2;
            _navigationManager.NavigateTo("/ReceptionCertificates/Create");
        }
    }
}
