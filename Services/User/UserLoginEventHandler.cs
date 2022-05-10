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
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace ApiGateway.Services
{
    public class UserLoginEventHandler : IRequestHandler<UserLoginCommand, IdentityAccess>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserLoginEventHandler(SignInManager<ApplicationUser> signInManager, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<IdentityAccess> Handle(UserLoginCommand loginCommand, CancellationToken cancellationToken)
        {
            var result = new IdentityAccess
            {
                Succeeded = false
            };

            if (loginCommand.Email == null && loginCommand.UserName == null)
            {
                result.Error = "invalid_request";
                result.ErrorDescription = "email/user_name requerido";
                return result;
            }
            ApplicationUser? user = null;
            if (loginCommand.Email != null) user = await _userManager.FindByEmailAsync(loginCommand.Email);
            if (loginCommand.UserName != null && user == null) user = await _userManager.FindByNameAsync(loginCommand.UserName);
            if (user == null)
            {
                result.Error = "invalid_request";
                result.ErrorDescription = "usuario o password invalido";
                return result;
            }

            if (loginCommand.RefreshToken != null && !loginCommand.RefreshToken.Equals(user.RefreshToken) || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                result.Error = "unauthorized_client";
                result.ErrorDescription = "refresh_token invalido";
                return result;
            }
            if (loginCommand.Password != null)
            {
                if (!(await _signInManager.CheckPasswordSignInAsync(user, loginCommand.Password, false)).Succeeded)
                {
                    result.Error = "invalid_request";
                    result.ErrorDescription = "usuario o password invalido";
                    return result;
                }
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            }

            result.Succeeded = true;
            result.Scope = loginCommand.Scope;
            GenerateJwtToken(user, result);
            GenerateRefreshToken(user, result);
            await _userManager.UpdateAsync(user);
            return result;
        }

        private void GenerateJwtToken(ApplicationUser user, IdentityAccess identity)
        {
            var secretKey = _configuration.GetValue<string>("SecretKey");
            var key = Encoding.ASCII.GetBytes(secretKey);
            var claims = new List<Claim>
            {
                new Claim (ClaimTypes.NameIdentifier, user.Id),
                new Claim (ClaimTypes.Email, user.NormalizedEmail),
                new Claim (ClaimTypes.Name, user.NormalizedUserName),
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

            identity.TokenType = "Bearer";
            identity.Expires = (int)Math.Truncate((tokenDescriptor.Expires - DateTime.Now).Value.TotalSeconds);
            identity.AccessToken = tokenHandler.WriteToken(createdToken);
        }

        private static void GenerateRefreshToken(ApplicationUser user, IdentityAccess identity)
        {
            using var rngCryptoServiceProvider = RandomNumberGenerator.Create();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            var refreshToken = Convert.ToBase64String(randomBytes);

            user.RefreshToken = refreshToken;
            identity.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;
            identity.RefreshToken = refreshToken;
        }
    }

    public class UserLoginCommand : IRequest<IdentityAccess>
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string? Password { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Scope { get; set; }
        [JsonIgnore]
        public string? RefreshToken { get; set; }
    }

    public class IdentityAccess
    {
        [JsonIgnore]
        public bool Succeeded { get; set; }

        [JsonPropertyName("access_token")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Expires { get; set; }

        [JsonPropertyName("scope")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Scope { get; set; }

        [JsonPropertyName("error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Error { get; set; }

        [JsonPropertyName("error_description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorDescription { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }
        [JsonIgnore]
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }

}
