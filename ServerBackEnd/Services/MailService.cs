
using System;
using System.IO;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using ApiGateway.Data;
using static System.Net.Mime.MediaTypeNames;

namespace ApiGateway.Service
{
    public class MailService
    {
        public bool MailSender(Email email)
        {
            try
            {

                var message = new MimeMessage();
                //From address
                message.From.Add(new MailboxAddress("PROSIS", "send1@grupo-prosis.com"));
                //To address
                message.To.Add(new MailboxAddress(email.Name,email.To));
                //Subject
                message.Subject = email.Subject;
                //Body
                message.Body = new TextPart("plain")
                {
                    Text = email.Body
                };
                //Configuration

                using (var client = new SmtpClient())
                {
                    client.Connect("smtpout.europe.secureserver.net", 465, true);
                    client.Authenticate("send1@grupo-prosis.com", "Pr0s1s");
                    client.Send(message);
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