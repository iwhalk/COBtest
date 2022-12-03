﻿using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using MimeKit;
using SharedLibrary;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ApiGateway.Services
{
    public class MailObraService : GenericProxy, IMailObraService
    {
        public MailObraService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse> GetMailAsync(int idReceptionCertificate, string email)
        {
            Dictionary<string, string> parameters = new();

            if (idReceptionCertificate != null)
            {
                parameters.Add("idReceptionCertificate", idReceptionCertificate.ToString());
            }
            if (!string.IsNullOrEmpty(email))
            {
                parameters.Add("email", email);
            }

            return await GetAsync(path: "SendReceptionCertificate", parameters: parameters);
        }

        public bool MailSender(MimeMessage mimeMessage)
        {
            try
            {
                //From address
                mimeMessage.From.Add(new MailboxAddress("PROSIS", "send1@grupo-prosis.com"));

                //Configuration
                using (var client = new SmtpClient())
                {
                    client.Connect("smtpout.europe.secureserver.net", 465, true);
                    client.Authenticate("send1@grupo-prosis.com", "Pr0s1s");
                    client.Send(mimeMessage);
                    client.Disconnect(true);
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}