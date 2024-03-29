﻿using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IActivitiesService
    {
        Task<ApiResponse<Activity>> GetActivityAsync(int id);
        Task<ApiResponse<List<Activity>>> GetActivitiesAsync(int? idArea);
        Task<ApiResponse<Activity>> PostActivityAsync(Activity activity);
    }
}
