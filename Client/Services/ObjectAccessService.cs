using Microsoft.AspNetCore.Components.Authorization;
using Obra.Client.Interfaces;
using SharedLibrary.Models;

namespace Obra.Client.Services
{
    public class ObjectAccessService : IObjectAccessService
    {
        private readonly AuthenticationStateProvider _getAuthenticationStateAsync;
        private readonly IProgressReportService _progressReportService;
        public ObjectAccessUser AccessUser { get; set; }
        //public static ObjectAccessUser AccessUser;

        public ObjectAccessService(AuthenticationStateProvider getAuthenticationStateAsync, IProgressReportService progressReportService)
        {
            _getAuthenticationStateAsync = getAuthenticationStateAsync;
            _progressReportService = progressReportService;
        }

        public async Task<ObjectAccessUser> GetObjectAccess()
        {
            if (AccessUser == null)
            {
                var authstate = await _getAuthenticationStateAsync.GetAuthenticationStateAsync();
                string idSupervisor = authstate.User?.Claims?.FirstOrDefault(x => x.Type.Equals("sub"))?.Value;
                AccessUser = await _progressReportService.GetObjectAccessAsync(idSupervisor);
            }
            return AccessUser;
        }
    }
}
