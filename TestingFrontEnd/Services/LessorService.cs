using TestingFrontEnd.Interfaces;
using Shared.Models;

namespace TestingFrontEnd.Services
{
    public class LessorService : ILessorService
    {
        private readonly IGenericRepository _repository;
        public LessorService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Lessor>> GetLessorAsync()
        {
            return await _repository.GetAsync<List<Lessor>>("api/Lessor/Get");
        }
        public async Task<Lessor> PostLessorAsync(Lessor lessor)
        {
            return await _repository.PostAsync<Lessor>("api/Lessor/Post", lessor);
        }
    }
}
