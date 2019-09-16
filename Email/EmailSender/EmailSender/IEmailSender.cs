using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    public interface IEmailSender
    {
        void SendEmail(MimeMessage mimeMessage);

        void SendEmail(MailMessage mailMessage);
    }
}
