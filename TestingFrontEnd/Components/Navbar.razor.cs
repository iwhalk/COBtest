using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using System.Collections;

namespace FrontEnd.Components
{
    public partial class Navbar : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly NavigationManager _navigate;

        public Navbar(ApplicationContext context, NavigationManager navigate)
        {
            _context = context;
            _navigate = navigate;
        }

        public bool ShowBurguer { get; set; } = false;
        public void ChageShowBurguer() => ShowBurguer = ShowBurguer ? false : true;
        public void OnLogoClick()
        {
            _context.CurrentReceptionCertificate = null;
            _context.ReceptionCertificateExist = null;
            _navigate.NavigateTo("/");
        }

    }
}
