using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using SharedLibrary;
using SharedLibrary.Models;
using SharedLibrary.Models;
using System.Net.NetworkInformation;

namespace ApiGateway.Services
{
    public class ReceptionCertificateService : GenericProxy, IReceptionCertificateService
    {
        public ReceptionCertificateService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<List<ActasRecepcion>>> GetReceptionCertificateAsync(string? startDay, string? endDay, int? certificateType, int? propertyType, int? numberOfRooms, int? lessor, int? tenant, string? delegation, string? agent, int? currentPage, int? rowNumber)
        {
            Dictionary<string, string> parameters = new();

            if (!string.IsNullOrEmpty(startDay))
            {
                parameters.Add("startDay", startDay);
            }
            if (!string.IsNullOrEmpty(endDay))
            {
                parameters.Add("endDay", endDay);
            }
            if (certificateType != null)
            {
                if (certificateType > 0)
                {
                    parameters.Add("certificateType", certificateType.ToString());
                }
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
                if (delegation.Equals("0") == false)
                {
                    parameters.Add("delegation", delegation);
                }
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
        public async Task<ApiResponse<ReceptionCertificate>> PutReceptionCertificateAsync(ReceptionCertificate reception)
        {
            return await PutAsync<ReceptionCertificate>(id: reception.IdReceptionCertificate, reception, path: "ReceptionCertificate");
        }
    }
}
