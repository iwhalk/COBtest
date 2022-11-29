using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using SharedLibrary.Data;
using SharedLibrary.Models;
using Microsoft.EntityFrameworkCore;
using ReportesObra.Utilities;

namespace ReportesObra.Services
{
    public class MailService : Interfaces.IMailService
    {
        private readonly InmobiliariaDbContext _dbContext;
        private readonly MailFactory _mailFactory;

        public MailService(MailFactory mailFactory, InmobiliariaDbContext dbContext)
        {
            _mailFactory = mailFactory;
            _dbContext = dbContext;
        }

        public Task<bool> SendReceptionCertificate(byte[] reporte, string email)
        {
            try
            {
                MimeMessage mimeMessage = new();
                mimeMessage.To.Add(new MailboxAddress("Usuario ARI", email));
                mimeMessage.Subject = "Envío de Acta";

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
