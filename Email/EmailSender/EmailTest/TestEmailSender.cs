using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Mime = MailKit.Net.Smtp;


namespace EmailTest
{
    public class TestEmailSender : EmailSender.IEmailSender
    {
        public void SendEmail(MailMessage mailMessage)
        {

        }

        public void SendEmail(MimeMessage mimeMessage)
        {
            var client = new Mime.SmtpClient();

            client.ServerCertificateValidationCallback = (s, c, h, e) =>
            {
                Console.WriteLine("authen callback");
                return true;
            };

            client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
            //client.Connect("smtp.gmail.com", 578);
            client.Authenticate("gaofan19901103@gmail.com", "wnzhyyqmm");
            client.Send(mimeMessage);
            client.Disconnect(true);
        }
    }
}
