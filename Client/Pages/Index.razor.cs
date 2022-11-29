using Client.Interfaces;
using Client.Stores;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;
using System.Reflection.Metadata.Ecma335;

namespace Client.Pages
{
    public partial class Index : ComponentBase
    {
        private readonly NavigationManager _navigationManager;
        private readonly ApplicationContext _context;
        public Index(NavigationManager navigationManager, ApplicationContext context)
        {
            _navigationManager = navigationManager;
            _context = context;
        }

        protected override async Task OnInitializedAsync()
        {
        }
    }
}