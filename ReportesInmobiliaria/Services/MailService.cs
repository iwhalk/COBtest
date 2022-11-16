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
            var userName = _dbContext.AspNetUsers.FirstOrDefault(a => a.Id == IdUser).Name + " " + _dbContext.AspNetUsers.FirstOrDefault(a => a.Id == IdUser).LastName;
            var email = _dbContext.AspNetUsers.FirstOrDefault(a => a.Id == IdUser).Email;
            MimeMessage mimeMessage = new();
            mimeMessage.To.Add(new MailboxAddress(userName, email));
            mimeMessage.Subject = "Registro del Sistema ARI";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<h1>MONO CON LAG</h1>";
            bodyBuilder.Attachments.Add("Acta_Entrega", reporte, new ContentType("application", "pdf"));
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            _mailFactory.MailSender(mimeMessage);

            return Task.FromResult(true);
        }

    }
}
