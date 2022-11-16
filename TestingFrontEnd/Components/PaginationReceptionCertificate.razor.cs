using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components
{
    public partial class PaginationReceptionCertificate : ComponentBase
    {
        public enum PaginationAction
        { 
            FirstPage,
            EndPage,
            NextPage,
            PreviewPage
        }
        private readonly ApplicationContext _context;

        [Parameter]
        public int CurrentPage { get; set; }
        [Parameter]
        public int NumberPage { get; set; }
        [Parameter]
        public int MaxNumberPages { get; set; }

        [Parameter]
        public EventCallback<PaginationAction> ChangePagination { get; set; }
        [Parameter]
        public EventCallback<int> ChangePaginationForNumber { get; set; }

        public PaginationReceptionCertificate(ApplicationContext context)
        {
            _context = context;
        }    
    }
}
