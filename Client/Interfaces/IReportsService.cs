﻿using SharedLibrary.Models;
using SharedLibrary;
using Obra.Client.Models;

namespace Obra.Client.Interfaces
{
    public interface IReportsService
    {
        //Nuevos EP's para las vistas de detalles por departamento y detalles por actividad
        //Detalles por departamento
        Task<List<DetalladoDepartamentos>> PostDataDetallesDepartamentos(ActivitiesDetail reporteDetalle);
        Task<byte[]> PostReporteDetallesPorDepartamento(List<DetalladoDepartamentos> detalladoDepartamentos, int? opcion);
        //Destalles por actividad
        Task<List<DetalladoActividades>> PostDataDetallesActividades(ActivitiesDetail reporteDetalle);
        Task<byte[]> PostReporteDetallesPorActividadesAsync(List<DetalladoActividades> detalladoActividades, int? opcion);

        Task<byte[]> PostReporteDetallesAsync(ActivitiesDetail reporteDetalle);
        Task<byte[]> PostReporteDetallesPorActividadAsync(ActivitiesDetail reporteDetalle);
        //MetodosReportes
        Task<List<AparmentProgress>?> GetProgressByAparmentDataViewAsync(int? idAparment);
        Task<byte[]> PostProgressByAparmentPDFAsync(List<AparmentProgress> progressReportList);
        Task<List<ActivityProgress>?> GetProgressByActivityDataViewAsync(int? idBuilding, int? idActivity);
        Task<byte[]> PostProgressByActivityPDFAsync(List<ActivityProgress> progressReportList);
        Task<List<AparmentProgress>?> GetProgressOfAparmentByActivityDataViewAsync(int? idActivity);
        Task<byte[]> PostProgressOfAparmentByActivityPDFAsync(List<AparmentProgress> progressReportList);
        Task<List<ActivityProgressByAparment>?> GetProgressOfActivityByAparmentDataViewAsync(int? idActivity);
        Task<byte[]> PostProgressOfActivityByParmentPDFAsync(List<ActivityProgressByAparment> progressReportList);
    }
}
