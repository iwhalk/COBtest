using Microsoft.AspNetCore.Components;


namespace Shared.Components
{
    public partial class Boton : ComponentBase
    {
        [Parameter]
        public string? ImagePath { get; set; }

        [Parameter]
        public string? RouteLink { get; set; }

        public Boton()
        {            

        }
    }
}