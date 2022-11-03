using Microsoft.AspNetCore.Components;

namespace TestingFrontEnd.Pages
{
    public partial class InventoryReceptionCertificates : ComponentBase
    {
        public bool ShowModalRooms { get; set; } = false;
        public void ChangeOpenModalRooms() => ShowModalRooms = ShowModalRooms ? false : true;
        public bool ShowModalComponents { get; set; } = false;
        public void ChangeOpenModalComponents() => ShowModalComponents = ShowModalComponents ? false : true;
    }
}
