using Microsoft.AspNetCore.Components;

namespace FrontEnd.Pages
{
    public partial class Emails : ComponentBase
    {
        public bool ShowModalSend { get; set; }
        public void ChangeOpenModalSend() => ShowModalSend = ShowModalSend ? false : true;
    }
}
