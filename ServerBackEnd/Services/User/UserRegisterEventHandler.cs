﻿using ApiGateway.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using ApiGateway.Services;
using ApiGateway.Service;
using ApiGateway.Data;

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
            MailService mailService = new MailService();
            
            
            var entry = new ApplicationUser
            {
                UserName = createCommand.UserName,
                Email = createCommand.Email
            };
            var password = GenerarPassword();
            Email email = new Email()
            {
                Name = createCommand.UserName,
                To = createCommand.Email,
                Subject = "Password secreto",
                Body = password,
            };
            mailService.MailSender(email);
            var res = await _userManager.CreateAsync(entry, password);
            //_emailservice.sendemail(bodi{ passworf = passwor})
            var user = await _userManager.FindByNameAsync(createCommand.UserName);
            if(res.Succeeded) res = await _userManager.AddToRoleAsync(user, "User");
            return res;
        }

        private static string GenerarPassword()
        {
            string contraseña = string.Empty;
            string[] letras = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "ñ", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
            Random EleccionAleatoria = new Random();

            for (int i = 0; i < 8; i++)
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
        public string UserName { get; set; }
    }
}
