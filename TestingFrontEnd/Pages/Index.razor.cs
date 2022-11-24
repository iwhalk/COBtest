using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using System.Reflection.Metadata.Ecma335;

namespace FrontEnd.Pages
{
    public partial class Index : ComponentBase
    {
        private readonly NavigationManager _navigationManager;
        private readonly ApplicationContext _context;
        public Index(NavigationManager navigationManager, ApplicationContext context)
        {
            _navigationManager = navigationManager;
            _context = context;
        }

        public void RedirectRecptionHistorical()
        {            
            _context.NumPage = 0;
            _context.TypeHistoricalOrPending = "Historical";
            _context.ActasRecepcionList = null;
            _context.Completed = true;
            _navigationManager.NavigateTo("/ReceptionCertificates/Historical");
        }
    }
}
