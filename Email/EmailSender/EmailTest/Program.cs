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
            SendUpdateEmail("834172555@qq.com");

            Console.ReadKey();          
        }

        static void SendConfirmationEmail(string receiver)
        {
            string logoImgCID = Guid.NewGuid().ToString();
            string heartImgCID = Guid.NewGuid().ToString();
            string bagImgCID = Guid.NewGuid().ToString();
            string martixImgCID = Guid.NewGuid().ToString();
            string doorstepImgCID = Guid.NewGuid().ToString();


            string template = File.ReadAllText(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\ForDJ\Email\EmailSender\EmailSender\MetaTemplate\Confirmation\raw.html"); //put your path here

            ConfirmationEmailModal confirmationModal = new ConfirmationEmailModal
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


            TemplateEngine<ConfirmationEmailModal> templateEngine = new TemplateEngine<ConfirmationEmailModal>(confirmationModal, template);
            string merged = templateEngine.GetTemplate();
            bodyBuilder.HtmlBody = merged;

            var message = new MimeMessage
            {
                Body = bodyBuilder.ToMessageBody(),
                Subject = "test Sub"
            };
            message.From.Add(new MailboxAddress("gf", "gf1103@gmail.com"));
            message.To.Add(new MailboxAddress("to_name", receiver));

            IEmailSender sender = new TestEmailSender();
            sender.SendEmail(message);
        }

        static void SendUpdateEmail(string receiver)
        {
            string logoImgCID = Guid.NewGuid().ToString();
            string heartImgCID = Guid.NewGuid().ToString();
            string errorImgCID = Guid.NewGuid().ToString();
            string starFillImgCID = Guid.NewGuid().ToString();
            string starStripeImgCID = Guid.NewGuid().ToString();

            string template = File.ReadAllText(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\ForDJ\Email\EmailSender\EmailSender\MetaTemplate\Update\raw.html"); //put your path here

            UpdateEmailModal updateModal = new UpdateEmailModal
            {
                UserName = "Ding Jian",
                LegendInfo = "Melamine cups are not recyclable",
                LogoImg = $"cid:{logoImgCID}",
                HeartImg = $"cid:{heartImgCID}",
                ErrorImg = $"cid:{errorImgCID}",
                StarFillImg = $"cid:{starFillImgCID}",
                StarStripeImg = $"cid:{starStripeImgCID}",
                NegativeRate = 2,
                PositiveRate = 3,
                Recyclables = new RecycleItem[]
            {
                    new RecycleItem { Name = "e-waste", Percent = 80, Weight = 0.85},
                    new RecycleItem { Name = "aluminium", Percent = 12, Weight = 0.075},
                    new RecycleItem { Name = "papers", Percent = 55, Weight = 0.58},
            },
                UnRecyclables = new RecycleItem[]
            {
                    new RecycleItem { Name = "Melamine cups" },
                    new RecycleItem { Name = "Dispers" },
                    new RecycleItem { Name = "Scotch Tape" },
            },
                TotalWeight = 3.2
            };

            var bodyBuilder = new BodyBuilder();

            var imageLogo = bodyBuilder.LinkedResources.Add(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\ForDJ\Email\EmailSender\EmailSender\MetaTemplate\Image\logo.png");
            var imageHeart = bodyBuilder.LinkedResources.Add(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\ForDJ\Email\EmailSender\EmailSender\MetaTemplate\Image\heart.png");
            var imageError = bodyBuilder.LinkedResources.Add(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\ForDJ\Email\EmailSender\EmailSender\MetaTemplate\Image\error.png");
            var starFill = bodyBuilder.LinkedResources.Add(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\ForDJ\Email\EmailSender\EmailSender\MetaTemplate\Image\Star_fill.png");
            var starStripe = bodyBuilder.LinkedResources.Add(@"C:\Users\Fan\Documents\gaofan folder\Reset.net\DotNet\ForDJ\Email\EmailSender\EmailSender\MetaTemplate\Image\Star_stripe.png");


            imageLogo.ContentId = logoImgCID;
            imageHeart.ContentId = heartImgCID;
            imageError.ContentId = errorImgCID;
            starFill.ContentId = starFillImgCID;
            starStripe.ContentId = starStripeImgCID;

            TemplateEngine<UpdateEmailModal> templateEngine = new TemplateEngine<UpdateEmailModal>(updateModal, template);
            string merged = templateEngine.GetTemplate();
            bodyBuilder.HtmlBody = merged;

            var message = new MimeMessage
            {
                Body = bodyBuilder.ToMessageBody(),
                Subject = "test Sub"
            };
            message.From.Add(new MailboxAddress("gf", "gf1103@gmail.com"));
            message.To.Add(new MailboxAddress("to_name", receiver));

            IEmailSender sender = new TestEmailSender();
            sender.SendEmail(message);
        }
    }
}
