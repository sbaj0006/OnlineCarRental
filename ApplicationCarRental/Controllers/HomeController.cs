using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net.Mail;
using Postal;
using ApplicationCarRental.ViewModel;
using ApplicationCarRental.Extensions;
using ApplicationCarRental.Models;
using ApplicationCarRental.Utils;

namespace ApplicationCarRental.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string search = null)
        {
            var thumbnails = new List<ThumbnailModel>().GetCarThumbnail(ApplicationDbContext.Create(), search);
            var count = thumbnails.Count() / 4;
            var model = new List<ThumbnailBoxViewModel>();

            for (int i = 0; i <= count; i++)
            {
                model.Add(new ThumbnailBoxViewModel
                {
                    Thumbnails = thumbnails.Skip(i * 4).Take(4)
                });
            }
            return View(model);
        }

        public ActionResult Locations()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult EmailAttachment()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult EmailAttachment(string[] email)
        {
            // if (!string.IsNullOrEmpty(email))
            // {
            if (Request.Files.Count > 0)
            {

                dynamic emailToSend = new Email("EmailWithAttachment");

                byte[] data = new byte[0];
                data = new byte[Request.Files[0].ContentLength];

                var fileName = Request.Files[0].FileName;
                var extension = Path.GetExtension(Request.Files[0].FileName);
                Request.Files[0].InputStream.Read(data, 0, Request.Files[0].ContentLength);

                MemoryStream stream = null;
                try
                {
                  //  for (int i = 0; i <= email.Length; i++)
                   // {

                        stream = new MemoryStream(data, true);
                        stream.Position = 0;
                        emailToSend.Attach(new Attachment(stream, fileName + extension));
                        emailToSend.To = email[0];
                        MailMessage message = new MailMessage();
                        MailAddress bcc = new MailAddress(email[1]);
                        message.Bcc.Add(bcc);
                        //emailToSend.To = email1;
                        emailToSend.Send();
                   // }
                }
                finally
                {
                    if (stream != null) stream.Dispose();

                }
            }

            return View();
        }


        public ActionResult Send_Email()
        {
            return View(new SendEmailViewModel());
        }

        [HttpPost]
        public ActionResult Send_Email(SendEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    String toEmail = model.ToEmail;
                    String subject = model.Subject;
                    String contents = model.Contents;

                    EmailSender es = new EmailSender();
                    es.Send(toEmail, subject, contents);

                    ViewBag.Result = "Email has been send.";

                    ModelState.Clear();

                    return View(new SendEmailViewModel());
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }
    }
}