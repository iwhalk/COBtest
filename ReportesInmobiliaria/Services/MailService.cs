using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using ReportesInmobiliaria.Interfaces;
using SharedLibrary.Data;
using ReportesInmobiliaria.Utilities;
using SharedLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ReportesInmobiliaria.Services
{
    public class MailService : ReportesInmobiliaria.Interfaces.IMailService
    {
        private readonly InmobiliariaDbContext _dbContext;
        private readonly MailFactory _mailFactory;

        public MailService(MailFactory mailFactory, InmobiliariaDbContext dbContext)
        {
            _mailFactory = mailFactory;
            _dbContext = dbContext;
        }

        public Task<bool> SendReceptionCertificate(byte[] reporte, string IdUser)
        {
            try
            {
                var user = _dbContext.AspNetUsers.FirstOrDefault(u => u.Id == IdUser);
                var userName = user.Name + " " + user.LastName;
                var email = user.Email;
                MimeMessage mimeMessage = new();
                mimeMessage.To.Add(new MailboxAddress(userName, email));
                mimeMessage.Subject = "Registro del Sistema ARI";

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<p><font size=\"5\">Se ha generado una acta de Entrega Recepción en el Sistema ARI, se adjuntó el archivo correspondiente en este correo.<br>" +
                    "Saludos Cordiales.</font></p>";
                    //"<hr>" +
                    //"<p><font size=\"4\">Archivo adjunto:</font></p>";
                bodyBuilder.Attachments.Add("Acta_Entrega", reporte, new ContentType("application", "pdf"));
                mimeMessage.Body = bodyBuilder.ToMessageBody();
                _mailFactory.MailSender(mimeMessage);
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                return Task.FromResult(false);
            }

        }

    }
}
