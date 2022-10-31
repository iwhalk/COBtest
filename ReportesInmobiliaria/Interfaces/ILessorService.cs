﻿using Shared.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface ILessorService
    {
        Task<List<Lessor>> GetLessors();
    }
}
