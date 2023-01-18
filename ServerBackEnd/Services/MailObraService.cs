using ApiGateway.Interfaces;
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
                mimeMessage.From.Add(new MailboxAddress("PROSIS", "desarrollo@grupo-prosis.com"));

                //Configuration
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.sendgrid.net", 465, true);
                    client.Authenticate("apikey", "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");
                    var temp = client.Send(mimeMessage);
                    Console.WriteLine(temp);
                    client.Disconnect(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
