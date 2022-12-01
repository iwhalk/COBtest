using Obra.Client.Interfaces;
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

        public async Task<List<Activity>> GetActivitiesAsync()
        {
            if (_context.Activity == null)
            {
                var response = await _repository.GetAsync<List<Activity>>("api/Activities");

                if (response != null)
                {
                    _context.Activity = response;
                    return _context.Activity;
                }
            }

            return _context.Activity;
        }

        public async Task<Activity> PostActivityAsync(Activity activity)
        {
            return null;
            //return await _repository.PostAsync("api/Activities", activity);
        }
    }
}
