using ApiGateway.Interfaces;
using ApiGateway.Models;
using ApiGateway.Proxies;
using Obra.Client.Pages;
using SharedLibrary;
using SharedLibrary.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace ApiGateway.Services
{
    public class ReportsService : GenericProxy, IReportsService
    {
        public ReportsService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<List<DetalladoDepartamentos>>> PostDataDetallesPorDepartamentosAsync(ActivitiesDetail reporteDetalle)
        {
            return await PostAsync<List<DetalladoDepartamentos>>(reporteDetalle, path: "DataDetallesDepartamento");
        }
        public async Task<ApiResponse<byte[]>> PostReporteDetallesPorDepartamentosAsync(List<DetalladoDepartamentos> detalladoDepartamentos, int? opcion)
        {
            if (opcion != null)
                return await PostAsync<byte[]>(detalladoDepartamentos, path: "ReporteDetalladoPorDepartamento?opcion=" + opcion.ToString());
            else
                return await PostAsync<byte[]>(detalladoDepartamentos, path: "ReporteDetalladoPorDepartamento");
        }
        public async Task<ApiResponse<List<DetalladoActividades>>> PostDataDetallesPorActividadesAsync(ActivitiesDetail reporteDetalle)
        {
            return await PostAsync<List<DetalladoActividades>>(reporteDetalle, path: "DataDetallesActividad");
        }
        public async Task<ApiResponse<byte[]>> PostReporteDetallesPorActividadesAsync(List<DetalladoActividades> detalladoActividades, int? opcion)
        {
            if (opcion != null)
                return await PostAsync<byte[]>(detalladoActividades, path: "ReporteDetalladoPorActividad?opcion=" + opcion.ToString());
            else
                return await PostAsync<byte[]>(detalladoActividades, path: "ReporteDetalladoPorActividad");
        }

        public async Task<ApiResponse<byte[]>> PostReporteEvolucionAsync(ActivitiesDetail reporteDetalle)
        {
            return await PostAsync<byte[]>(reporteDetalle, path: "ReportEvolution");
        }

        public async Task<ApiResponse<byte[]>> PostReporteAvanceCostoAsync(ActivitiesDetail reporteDetalle)
        {
            return await PostAsync<byte[]>(reporteDetalle, path: "ReportAdvanceCost");
        }

        public async Task<ApiResponse<byte[]>> PostReporteAvanceTiempoAsync(ActivitiesDetail reporteDetalle)
        {
            return await PostAsync<byte[]>(reporteDetalle, path: "ReportAdvanceTime");
        }

        //Reports
        public async Task<ApiResponse<List<AparmentProgress>>?> GetProgressByAparmentViewAsync(int? idBuilding, int? idApartment)
        {
            Dictionary<string, string> parameters = new();
            if (idBuilding != null)
            {
                parameters.Add("idBuilding", idBuilding.ToString());
            }
            if (idApartment != null)
            {
                parameters.Add("idAparment", idApartment.ToString());
            }
            return await GetAsync<List<AparmentProgress>?>(path: "ReportProgressByAparmentView", parameters: parameters);
        }
        public async Task<ApiResponse<byte[]>> PostProgressByAparmentPDFAsync(List<AparmentProgress> progressReport, string subTitle)
        {
            return await PostAsync<byte[]>(progressReport, path: "ReportProgressByAparmentPDF?subTitle=" + subTitle);
        }

        public async Task<ApiResponse<List<ActivityProgress>>?> GetProgressByActivityViewAsync(int? idBuilding, int? idActivity)
        {
            Dictionary<string, string> parameters = new();
            if (idBuilding != null)
            {
                parameters.Add("idBuilding", idBuilding.ToString());
            }
            if(idActivity != null)
            {
                parameters.Add("idActivity", idActivity.ToString());
            }
            return await GetAsync<List<ActivityProgress>?>(path: "ReportProgressByActivityView", parameters: parameters);
        }

        public async Task<ApiResponse<byte[]>> PostProgressByActivityPDFAsync(List<ActivityProgress> progressReport, string subTitle)
        {
            return await PostAsync<byte[]>(progressReport, path: "ReportProgressByActivityPDF?subTitle=" + subTitle);
        }

        public async Task<ApiResponse<List<AparmentProgress>>?> GetProgressOfAparmentByActivityViewAsync(int? idBuilding, int? idActivity)
        {
            Dictionary<string, string> parameters = new();
            if (idBuilding != null)
            {
                parameters.Add("idBuilding", idBuilding.ToString());
            }
            if (idActivity != null)
            {
                parameters.Add("idActivity", idActivity.ToString());
            }            
            return await GetAsync<List<AparmentProgress>?>(path: "ReportOfAparmentByActivityView", parameters: parameters);
        }

        public async Task<ApiResponse<byte[]>> PostProgressOfAparmentByActivityPDFAsync(List<AparmentProgress> progressReport, bool all)
        {
            return await PostAsync<byte[]>(progressReport, path: "ReportOfAparmentByActivityPDF?all=" + all.ToString());
        }

        public async Task<ApiResponse<List<ActivityProgressByAparment>>?> GetProgressOfActivityByAparmentViewAsync(int? idBuilding, int? idActivity)
        {
            Dictionary<string, string> parameters = new();
            if (idBuilding != null)
            {
                parameters.Add("idBuilding", idBuilding.ToString());
            }
            if (idActivity != null)
            {
                parameters.Add("idApartment", idActivity.ToString());
            }
            return await GetAsync<List<ActivityProgressByAparment>>(path: "ReportOfActivityByAparmentView", parameters: parameters);
        }
        public async Task<ApiResponse<byte[]>> PostProgressOfActivityByAparmentPDFAsync(List<ActivityProgressByAparment> progressReport, bool all)
        {
            return await PostAsync<byte[]>(progressReport, path: "ReportOfActivityByAparmentPDF?all=" + all.ToString());
        }
    }
}
