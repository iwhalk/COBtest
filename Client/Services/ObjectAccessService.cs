using Microsoft.AspNetCore.Components.Authorization;
using Obra.Client.Interfaces;
using SharedLibrary.Models;

namespace Obra.Client.Services
{
    public class ObjectAccessService : IObjectAccessService
    {
        private readonly AuthenticationStateProvider _getAuthenticationStateAsync;
        private readonly IProgressReportService _progressReportService;
        private readonly IStatusesService _statusService;
        public ObjectAccessUser AccessUser { get; set; }
        public List<Status> Statuses { get; set; }
        //public static ObjectAccessUser AccessUser;

        public ObjectAccessService(AuthenticationStateProvider getAuthenticationStateAsync, IProgressReportService progressReportService, IStatusesService statusesService)
        {
            _getAuthenticationStateAsync = getAuthenticationStateAsync;
            _progressReportService = progressReportService;
            _statusService = statusesService;
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

        public async Task<List<Status>> GetStatuses()
        {
            if (Statuses == null)
            {
                Statuses = await _statusService.GetStatusesAsync();
            }
            return Statuses;
        }
    }
}
