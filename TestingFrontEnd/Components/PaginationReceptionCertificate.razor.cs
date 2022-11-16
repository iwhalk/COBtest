using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components
{
    public partial class PaginationReceptionCertificate : ComponentBase
    {
        public enum PaginationAction
        { 
            FirstPage,
            EndPage,
        }

        [Parameter]
        public int CurrentPage { get; set; }
        [Parameter]
        public int NumberPage { get; set; }
        [Parameter]
        public int RowNumberForPage { get; set; }

        [Parameter]
        public EventCallback<PaginationAction> ChangePagination { get; set; }
    }
}
