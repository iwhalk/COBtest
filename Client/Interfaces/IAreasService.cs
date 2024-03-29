﻿using SharedLibrary.Models;
using SharedLibrary;

namespace Obra.Client.Interfaces
{
    public interface IAreasService
    {
        Task<Area> GetAreaAsync(int id);
        Task<List<Area>> GetAreasAsync();
        Task<Area> PostAreaAsync(Area area);
    }
}
