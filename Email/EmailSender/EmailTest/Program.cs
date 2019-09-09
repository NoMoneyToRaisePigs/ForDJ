using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.IO;
using MimeKit.Utils;
using System.Text.RegularExpressions;

namespace EmailTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string tempalte = File.ReadAllText(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\Email\EmailSender\EmailSender\EmailTemplate\reminder.html");


            var message = new MimeMessage();
            var bodyBuilder = new BodyBuilder();

            //var x = System.Configuration.ConfigurationManager.AppSettings;
            //string y = x["ab"];


            //Console.ReadKey();
            // from
            message.From.Add(new MailboxAddress("gf", "gf1103@gmail.com"));
            // to
            message.To.Add(new MailboxAddress("to_name", "dingjian412@gmail.com"));
            // reply to
            message.ReplyTo.Add(new MailboxAddress("reply_name", "reply_email@example.com"));

            message.Subject = "subject";

            var imageBags = bodyBuilder.LinkedResources.Add(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\Email\EmailSender\EmailSender\EmailTemplate\bags.png");
            var imageLogo = bodyBuilder.LinkedResources.Add(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\Email\EmailSender\EmailSender\EmailTemplate\logo.png");
            var imageHeart = bodyBuilder.LinkedResources.Add(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\Email\EmailSender\EmailSender\EmailTemplate\heart.png");

            imageBags.ContentId = MimeUtils.GenerateMessageId();
            imageLogo.ContentId = MimeUtils.GenerateMessageId();
            imageHeart.ContentId = MimeUtils.GenerateMessageId();

            tempalte = tempalte.Replace("bags.png", $"cid:{imageBags.ContentId}");
            tempalte = tempalte.Replace("logo.png", $"cid:{imageLogo.ContentId}");
            tempalte = tempalte.Replace("heart.png", $"cid:{imageHeart.ContentId}");
            tempalte = tempalte.Replace("{{username}}", "Ding Jian");
            
            //tempalte = Regex.Replace(tempalte, @"\bags.png\g", $"cid:{imageBags.ContentId}");
            //tempalte = Regex.Replace(tempalte, @"\logo.png\g", $"cid:{imageLogo.ContentId}");
            //tempalte = Regex.Replace(tempalte, @"\heart.png\g", $"cid:{imageHeart.ContentId}");

            bodyBuilder.HtmlBody = tempalte;

            //bodyBuilder.Attachments.Add(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\Email\EmailSender\EmailSender\EmailTemplate\bags.png");
            //bodyBuilder.Attachments.Add(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\Email\EmailSender\EmailSender\EmailTemplate\logo.png");
            //bodyBuilder.Attachments.Add(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\Email\EmailSender\EmailSender\EmailTemplate\heart.png");
            message.Body = bodyBuilder.ToMessageBody();

            

            var client = new SmtpClient();

            client.ServerCertificateValidationCallback = (s, c, h, e) => 
            {
                Console.WriteLine("authen callback");
                return true;
            };
            client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
            //client.Connect("smtp.gmail.com", 578);
            client.Authenticate("gaofan19901103@gmail.com", "wnzhyyqmm");
            client.Send(message);
            client.Disconnect(true);

            Console.WriteLine("xx");
            Console.ReadKey();
        }
    }
}
