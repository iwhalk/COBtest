﻿using FrontEnd.Interfaces;

namespace FrontEnd.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IGenericRepository _repository;

        public ReportsService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<byte[]> GetReporteReceptionCertificate(int IdReceptionCertificate)
        {
            return await _repository.GetAsync<byte[]>($"api/reportes/ReporteReceptionCertificate?IdReceptionCertificate={IdReceptionCertificate}");
        }
    }
}
