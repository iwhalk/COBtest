﻿
using System;
using System.IO;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using ApiGateway.Data;
using static System.Net.Mime.MediaTypeNames;

namespace ApiGateway.Services
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