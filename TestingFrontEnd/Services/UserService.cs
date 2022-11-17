using FrontEnd.Interfaces;
using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;

namespace FrontEnd.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _context;
        private readonly IGenericRepository _repository;
        public UserService(ApplicationContext context, IGenericRepository repository)
        {
            _context = context;
            _repository = repository;
        }
        public async Task<List<AspNetUser>> GetUsersAsync()
        {
            if (_context.ListUserAsp == null)
            {
                var response = await _repository.GetAsync<List<AspNetUser>>("api/AspNetUser");

                if (response != null)
                {
                    _context.ListUserAsp = response;
                    return _context.ListUserAsp;
                }
            }
            return _context.ListUserAsp;
        }
    }
}
