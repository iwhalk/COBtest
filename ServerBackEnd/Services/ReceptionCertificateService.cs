using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using Shared;
using Shared.Models;
using System.Net.NetworkInformation;

namespace ApiGateway.Services
{
    public class ReceptionCertificateService : GenericProxy, IReceptionCertificateService
    {
        public ReceptionCertificateService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<List<ActasRecepcion>>> GetReceptionCertificateAsync(string? day, string? week, string? month, int? propertyType, int? numberOfRooms, int? lessor, int? tenant, string? delegation, string? agent, int? currentPage, int? rowNumber)
        {
            Dictionary<string, string> parameters = new();
            if (!string.IsNullOrEmpty(day))
            {
                parameters.Add("day", day);
            }
            if (!string.IsNullOrEmpty(week))
            {
                parameters.Add("week", week);
            }
            if (!string.IsNullOrEmpty(month))
            {
                parameters.Add("month", month);
            }
            if (propertyType != null)
            {
                if (propertyType > 0)
                {
                    parameters.Add("propertyType", propertyType.ToString());
                }
            }
            if (numberOfRooms != null)
            {
                if (numberOfRooms > 0)
                {
                    parameters.Add("numberOfRooms", numberOfRooms.ToString());
                }
            }
            if (lessor != null)
            {
                if (lessor > 0)
                {
                    parameters.Add("lessor", lessor.ToString());
                }
            }
            if (tenant != null)
            {
                if (tenant > 0)
                {
                    parameters.Add("tenant", tenant.ToString());
                }
            }
            if (!string.IsNullOrEmpty(delegation))
            {
                parameters.Add("delegation", delegation);
            }
            if (!string.IsNullOrEmpty(agent))
            {
                parameters.Add("agent", agent);
            }
            if (currentPage != null)
            {
                parameters.Add("currentPage", currentPage.ToString());
            }
            if (rowNumber != null)
            {
                parameters.Add("rowNumber", rowNumber.ToString());
            }


            return await GetAsync<List<ActasRecepcion>>(path: "ReceptionCertificate", parameters: parameters);
        }

        public async Task<ApiResponse<ReceptionCertificate>> PostReceptionCertificateAsync(ReceptionCertificate reception)
        {
            return await PostAsync<ReceptionCertificate>(reception, path: "ReceptionCertificate");
        }
    }
}
