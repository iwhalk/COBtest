using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace TestingFrontEnd.Components
{
    public partial class BurguerComponent : ComponentBase
    {        
        public SignOutSessionStateManager SignOutManager { get; set; }
        public NavigationManager Navigation { get; set; }
        public BurguerComponent(SignOutSessionStateManager _SignOutManager, NavigationManager _Navigation)
        {
            Navigation = _Navigation;
            SignOutManager = _SignOutManager;
        }
        public async void HandleSesionOut()
        {
            await SignOutManager.SetSignOutState();
            Navigation.NavigateTo("Identity/logout");
        }
    }
}
