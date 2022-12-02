//using Obra.Client.Stores;
using Microsoft.AspNetCore.Components;
using System.Collections;

namespace FrontEnd.Components
{
    public partial class Navbar : ComponentBase
    {        
        private readonly NavigationManager _navigate;
        public Navbar(NavigationManager navigate)
        {           
            _navigate = navigate;
        }                
    }
}
