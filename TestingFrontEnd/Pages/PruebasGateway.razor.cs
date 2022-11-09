using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;
using TestingFrontEnd.Interfaces;

namespace TestingFrontEnd.Pages
{
    public partial class PruebasGateway : ComponentBase
    {
        private readonly ILessorService _lessorService;
        public PruebasGateway(ILessorService lessorService)
        {
            _lessorService = lessorService;
        }

        private List<Lessor> ListArea { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("si llego aca");
            ListArea = await _lessorService.GetLessorAsync();            
        }
    }
}
