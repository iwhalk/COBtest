using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Pages
{
    public partial class IndexReceptionCertificates : ComponentBase
    {
        private readonly NavigationManager _navigationManager;
        private readonly ApplicationContext _context;
        public bool OpenModal { get; set; } = false;        
        public void ChangeOpenModal() => OpenModal = OpenModal ? false : true;
 
        public IndexReceptionCertificates(NavigationManager navigationManager, ApplicationContext context)
        {
            _navigationManager = navigationManager;
            _context = context;
        }

        public void RedirectRecptionPending()
        {
            _context.NumPage = 0;
            _context.TypeHistoricalOrPending = "Pending";
            _context.ActasRecepcionList = null;
            _context.Completed = false;
            _navigationManager.NavigateTo("/ReceptionCertificates/Historical");
        }

    }
}
