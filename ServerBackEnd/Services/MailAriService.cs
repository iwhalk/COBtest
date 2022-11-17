
using System;
using System.IO;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using ApiGateway.Data;
using static System.Net.Mime.MediaTypeNames;
using ApiGateway.Interfaces;
using SharedLibrary;
using ApiGateway.Proxies;
using SharedLibrary.Models;

namespace ApiGateway.Services
{
    public class MailAriService : GenericProxy, IMailAriService
    {
        public MailAriService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse> GetMailAsync(int idProperty, string email)
        {
            Dictionary<string, string> parameters = new();

            if (idProperty != null)
            {
                parameters.Add("idProperty", idProperty.ToString());
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