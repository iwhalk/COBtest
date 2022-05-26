using ApiGateway.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Services
{
    public class UserRegisterEventHandler : IRequestHandler<UserCreateCommand, IdentityResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRegisterEventHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(UserCreateCommand createCommand, CancellationToken cancellationToken)
        {
            var entry = new ApplicationUser
            {
                UserName = createCommand.UserName,
                Email = createCommand.Email
            };
            var res = await _userManager.CreateAsync(entry, createCommand.Password);
            var user = await _userManager.FindByNameAsync(createCommand.UserName);
            if(res.Succeeded) res = await _userManager.AddToRoleAsync(user, "User");
            return res;
        }
    }

    public class UserCreateCommand : IRequest<IdentityResult>
    {
        [Required]
        public string Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
