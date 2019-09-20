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
using EmailSender.Modal;
using EmailSender;

namespace EmailTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string logoImgCID = Guid.NewGuid().ToString();
            string heartImgCID = Guid.NewGuid().ToString();
            string bagImgCID = Guid.NewGuid().ToString();
            string martixImgCID = Guid.NewGuid().ToString();
            string doorstepImgCID = Guid.NewGuid().ToString();

            string template = File.ReadAllText(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\ForDJ\Email\EmailSender\EmailSender\MetaTemplate\Confirmation\raw.html"); //put your path here

            ConfirmationEmailModal modal = new ConfirmationEmailModal
            {
                UserName = "Ding Jian",
                DayOfWeek = DateTime.Today.DayOfWeek.ToString(),
                TimeOfDay = DateTime.Now.Hour.ToString(),
                FirstCollectionDate = DateTime.Now.ToString("dd/MM/yyyy"),
                LogoImg = $"cid:{logoImgCID}",
                HeartImg = $"cid:{heartImgCID}",
                BagImg = $"cid:{bagImgCID}",
                MatrixImg = $"cid:{martixImgCID}",
                DoorstepImg = $"cid:{doorstepImgCID}",
                FooterTitle = "your company footer",
                FooterEmail = "yourCompany@xxx.com"
            };


            var bodyBuilder = new BodyBuilder();

            var imageLogo = bodyBuilder.LinkedResources.Add(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\ForDJ\Email\EmailSender\EmailSender\MetaTemplate\Image\logo.png");
            var imageHeart = bodyBuilder.LinkedResources.Add(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\ForDJ\Email\EmailSender\EmailSender\MetaTemplate\Image\heart.png");
            var imageBag = bodyBuilder.LinkedResources.Add(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\ForDJ\Email\EmailSender\EmailSender\MetaTemplate\Image\bags.png");
            var imageMatrix = bodyBuilder.LinkedResources.Add(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\ForDJ\Email\EmailSender\EmailSender\MetaTemplate\Image\matrix.png");
            var imageDoorstep = bodyBuilder.LinkedResources.Add(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\ForDJ\Email\EmailSender\EmailSender\MetaTemplate\Image\doorstep.png");

            imageLogo.ContentId = logoImgCID;
            imageHeart.ContentId = heartImgCID;
            imageBag.ContentId = bagImgCID;
            imageMatrix.ContentId = martixImgCID;
            imageDoorstep.ContentId = doorstepImgCID;

            TemplateEngine<ConfirmationEmailModal> templateEngine = new TemplateEngine<ConfirmationEmailModal>(modal, template);
            string merged = templateEngine.GetTemplate();
            bodyBuilder.HtmlBody = merged;

            var message = new MimeMessage
            {
                Body = bodyBuilder.ToMessageBody(),
                Subject = "test Sub"
            };
            message.From.Add(new MailboxAddress("gf", "gf1103@gmail.com"));
            message.To.Add(new MailboxAddress("to_name", "dingjian412@gmail.com"));

            IEmailSender sender = new TestEmailSender();
            sender.SendEmail(message);

            Console.ReadKey();          
        }
    }
}
