using Microsoft.AspNetCore.Components;

namespace FrontEnd.Pages
{
    public partial class InventoryReceptionCertificates : ComponentBase
    {
        public string MaterialSelect { get; set; } = "";
        public string ColorSelect { get; set; } = "";
        public string StatusSelect { get; set; } = "";
        public int Cantaida { get; set; }
        public int M2 { get; set; }

        public bool ShowModalRooms { get; set; } = false;
        public bool ShowModalComponents { get; set; } = false;
        public bool ShowModalCopyValues { get; set; } = false;
        public bool ShowModalGauges { get; set; } = false;
        public bool ShowModalkeys { get; set; } = false;
        public string TypeButtonsInventory { get; set; } = "";

        public void ChangeOpenModalRooms() => ShowModalRooms = ShowModalRooms ? false : true;
        public void ChangeOpenModalComponents() => ShowModalComponents = ShowModalComponents ? false : true;
        public void ChangeOpenModalCopyValues() => ShowModalCopyValues = ShowModalCopyValues ? false : true;
        public void ChangeOpenModalGauges() => ShowModalGauges = ShowModalGauges ? false : true;
        public void ChangeOpenModalKeys() => ShowModalkeys = ShowModalkeys ? false : true;

        public void ChangeButtonsShow(string newButtonsShow)
        {
            TypeButtonsInventory = newButtonsShow;

            if (newButtonsShow == "Material")
                MaterialSelect = "";
            if (newButtonsShow == "Color")
                ColorSelect = "";
            if (newButtonsShow == "EstadoGeneral")
                StatusSelect = "";
        }

        public void SetColor(string newColor) => ColorSelect = newColor;
        public void SetMaterial(string newMaterial) => MaterialSelect = newMaterial;
        public void SetStatus(string newStatus) => StatusSelect = newStatus;
    }
}
