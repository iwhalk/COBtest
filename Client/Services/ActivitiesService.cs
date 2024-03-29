﻿using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;

namespace Obra.Client.Services
{
    public class ActivitiesService : IActivitiesService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public ActivitiesService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<Activity> GetActivityAsync(int id)
        {       
            return await _repository.GetAsync<Activity>(id, path: "api/Activities");
        }

        public async Task<List<Activity>> GetActivitiesAsync(int? idArea)
        {
            Dictionary<string, string> parameters = new();

            if (idArea != null && idArea > 0)
            {
                parameters.Add("idArea", idArea.ToString());
            }

            _context.Activity = await _repository.GetAsync<List<Activity>>(parameters, "api/Activities");            
            return _context.Activity;
        }

        public async Task<Activity> PostActivityAsync(Activity activity)
        {
            return await _repository.PostAsync(activity, path: "api/Activities");
        }
    }
}
