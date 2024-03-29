﻿using SharedLibrary;
using SharedLibrary.Models;

namespace Obra.Client.Interfaces
{
    public interface IActivitiesService
    {
        Task<Activity> GetActivityAsync(int id);
        Task<List<Activity>> GetActivitiesAsync(int? idArea = null);
        Task<Activity> PostActivityAsync(Activity activity);
    }
}
