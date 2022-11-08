using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Shared.Components
{
    public partial class BurguerComponent : ComponentBase
    {
        public int MyProperty { get; set; }
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