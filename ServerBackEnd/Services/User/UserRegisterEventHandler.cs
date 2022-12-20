using ApiGateway.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using MimeKit;
//using ApiGateway.Interfaces;

namespace ApiGateway.Services.User
{
    public class UserRegisterEventHandler : IRequestHandler<UserCreateCommand, IdentityResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IMailObraService _mail;

        public UserRegisterEventHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(UserCreateCommand createCommand, CancellationToken cancellationToken)
        {
            createCommand.UserName = createCommand.Email;
            var entry = new ApplicationUser
            {
                UserName = createCommand.UserName,
                Email = createCommand.Email,
                Name = createCommand.Name ?? "",
                LastName = createCommand.LastName ?? "",
            };
            var password = GenerarPassword();

            MimeMessage mimeMessage = new();
            mimeMessage.To.Add(new MailboxAddress(createCommand.UserName, createCommand.Email));
            mimeMessage.Subject = "Registro del Sistema ARI";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<h1>Se ha registrado en Sistema ARI para la generación de Actas Entrega y Recepción de Inmuebles.</h1>" +
            "<hr>" +
            "<p><font size=\"5\">Su contraseña de cuenta es: <mark><strong>" + password + "</strong></mark></font></p>" +
            "<hr>" +
            "<p><font size=\"5\"> Para iniciar sesión entre en la siguiente liga e ingrese su correo electrónico y la contraseña proporcionada:</font></p>" +
            "<font size=\"5\"><a href=\"https://sof2245.com/\">Sof2245</a></font>";
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            //mailService.MailSender(email);
            var res = await _userManager.CreateAsync(entry, password);
            //_emailservice.sendemail(bodi{ passworf = passwor})
            var user = await _userManager.FindByNameAsync(createCommand.UserName);
            if (res.Succeeded)
            {
                res = await _userManager.AddToRoleAsync(user, "User");
                //_mail.MailSender(mimeMessage);
            }
            return res;
        }

        private static string GenerarPassword()
        {
            string contraseña = string.Empty;
            string[] letras = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "ñ", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
            Random EleccionAleatoria = new Random();

            for (int i = 0; i < 12; i++)
            {
                int LetraAleatoria = EleccionAleatoria.Next(0, 100);
                int NumeroAleatorio = EleccionAleatoria.Next(0, 9);

                if (LetraAleatoria < letras.Length)
                {
                    contraseña += letras[LetraAleatoria];
                }
                else
                {
                    contraseña += NumeroAleatorio.ToString();
                }
            }
            return contraseña;
        }
    }

    public class UserCreateCommand : IRequest<IdentityResult>
    {
        public string? Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string? UserName { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
