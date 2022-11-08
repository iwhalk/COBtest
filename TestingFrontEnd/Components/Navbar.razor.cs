using Microsoft.AspNetCore.Components;

namespace TestingFrontEnd.Components
{
    public partial class Navbar : ComponentBase
    {
        public bool ShowBurguer { get; set; } = false;
        public void ChageShowBurguer() => ShowBurguer = ShowBurguer ? false : true;

    }
}
