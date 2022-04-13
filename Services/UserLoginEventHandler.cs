using ApiGateway.Data;
using ApiGateway.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiGateway.Services
{
    public class UserLoginEventHandler : IRequestHandler<UserLoginCommand, IdentityAccess>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserLoginEventHandler(SignInManager<ApplicationUser> signInManager, ApplicationDbContext appDbContext, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _dbContext = appDbContext;
            _configuration = configuration;
        }

        public async Task<IdentityAccess> Handle(UserLoginCommand loginCommand, CancellationToken cancellationToken)
        {
            var result = new IdentityAccess
            {
                Succeeded = false
            };
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == loginCommand.Email, cancellationToken);
            if (user == null) return result;

            var response = await _signInManager.CheckPasswordSignInAsync(user, loginCommand.Password, false);

            if (response.Succeeded)
            {
                result.Succeeded = true;
                await GenerateToken(user, result);
                return result;
            }
            return result;

        }

        private async Task GenerateToken(ApplicationUser user, IdentityAccess identity)
        {
            var secretKey = _configuration.GetValue<string>("SecretKey");
            var key = Encoding.ASCII.GetBytes(secretKey);
            var claims = new List<Claim>
            {
                new Claim (ClaimTypes.NameIdentifier, user.Id),
                new Claim (ClaimTypes.Email, user.Email),
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);

            identity.AccessToken = tokenHandler.WriteToken(createdToken);
        }

    }

    public class UserLoginCommand : IRequest<IdentityAccess>
    {
        [Required]
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string UserName { get; set; }
    }

    public class IdentityAccess
    {
        public bool Succeeded { get; set; }
        public string AccessToken { get; set; }
    }

}
