using ApiGateway.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiGateway.Services
{
    public class UserFindEventHandler : IRequestHandler<UserFindCommand, User>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserFindEventHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> Handle(UserFindCommand findCommand, CancellationToken cancellationToken)
        {
            ApplicationUser applicationUser = new();
            if (findCommand?.Id != null)
                applicationUser = await _userManager.FindByIdAsync(findCommand.Id);
            else if (findCommand?.Email != null)
                applicationUser = await _userManager.FindByEmailAsync(findCommand.Email);
            else if (findCommand?.Username != null)
                applicationUser = await _userManager.FindByNameAsync(findCommand.Username);
            return new User(applicationUser.Id, applicationUser.Email, applicationUser.UserName);
        }
    }

    public class UserFindCommand : IRequest<User>
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
    }
    public record User(string Id, string Email, string? Username)
    {
        [JsonIgnore]
        public bool Succeeded => Id != null && Email != null;
    }
}
