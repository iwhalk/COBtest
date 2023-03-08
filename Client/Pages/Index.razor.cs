using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Obra.Client.Interfaces;
using SharedLibrary.Models;

namespace Obra.Client.Pages
{
    public partial class Index : ComponentBase
    {
        private readonly IObjectAccessService _accessService;
        public Index(IObjectAccessService accessService)
        {
            _accessService = accessService;
        }

        //public Index()
        //{
        
        //}

        //protected override async Task OnInitializedAsync()
        //{
        //    var res = _accessService.GetObjectAccess();
        //}
    }
}
