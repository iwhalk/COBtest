using TestingFrontEnd.Interfaces;
using Shared.Models;
using TestingFrontEnd.Stores;

namespace TestingFrontEnd.Services
{
    public class LessorService : ILessorService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public LessorService(IGenericRepository repository, ApplicationContext context)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<List<Lessor>> GetLessorAsync()
        {
            if (_context.Lessor == null)
            {
                var response = await _repository.GetAsync<List<Lessor>>("api/Lessor");

                if (response != null)
                {
                    _context.Lessor = response;
                    return _context.Lessor;
                }
            }

            return _context.Lessor;
        }
        public async Task<Lessor> PostLessorAsync(Lessor lessor)
        {
            return await _repository.PostAsync<Lessor>("api/Lessor", lessor);
        }
    }
}
