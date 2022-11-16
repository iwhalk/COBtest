using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Mail;

namespace ReportesInmobiliaria.Services
{
    public class MailService
    {
        public bool MailSender(MimeMessage mimeMessage)
        {
            try
            {
                //From address
                mimeMessage.From.Add(new MailboxAddress("PROSIS", "send1@grupo-prosis.com"));

                //Configuration
                using (var client = new MailKit.Net.Smtp.SmtpClient())
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
