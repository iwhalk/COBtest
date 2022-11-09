using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;

namespace TestingFrontEnd.Components.Lessors
{
    public partial class FormLessor : ComponentBase
    {
        [Parameter]
        public EventCallback OpenModalLessor { get; set; }
        [Parameter]
        public Lessor? CurrentLessor { get; set; }
        
        public void HandlePost()
        {

            var l = new Lessor();
            l.Street = "'";

            
        }
    }
}
