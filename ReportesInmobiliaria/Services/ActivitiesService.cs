using Microsoft.EntityFrameworkCore;
using ReportesObra.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;
using SixLabors.ImageSharp.ColorSpaces;

namespace ReportesObra.Services
{
    public class ActivitiesService : IActivitiesService
    {
        private readonly ObraDbContext _dbContext;

        public ActivitiesService(ObraDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Activity>?> GetActivitiesAsync(int? idArea)
        {
            IQueryable<Activity> activities = _dbContext.Activities;
            if (idArea != null)
                activities = activities.Where(x => x.IdAreas.Any(y => y.IdArea == idArea));

            System.Linq.Expressions.Expression<Func<Activity, Activity>> selector = x => new Activity
            {
                IdActivity = x.IdActivity,
                ActivityName = x.ActivityName,
                Elements = x.Elements,
                IdAreas = x.IdAreas.Select(y => new Area
                {
                    IdArea = y.IdArea,
                    AreaName = y.AreaName,                    
                }).ToList()                
            };

            return await activities.Select(selector).ToListAsync();
        }

        public async Task<Activity?> GetActivityAsync(int id)
        {
            return await _dbContext.Activities.FirstOrDefaultAsync(x => x.IdActivity == id);
        }

        public async Task<Activity?> CreateActivityAsync(Activity activity)
        {
            await _dbContext.Activities.AddAsync(activity);
            try { await _dbContext.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }
            return activity;
        }

        public async Task<bool> UpdateActivityAsync(Activity activity)
        {
            _dbContext.Entry(activity).State = EntityState.Modified;
            try { await _dbContext.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }
            return true;
        }

        public async Task<bool> DeleteActivityAsync(int id)
        {
            Activity? activity = _dbContext.Activities.FirstOrDefault(x => x.IdActivity == id);
            if (activity == null)
                return false;
            _dbContext.Activities.Remove(activity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
