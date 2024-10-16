using Licenta.DataAbstractionLayer;
using Licenta.Models;
using System;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Web.Mvc;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Configuration;

namespace Licenta.Controllers
{
    public class CalculatorController : Controller
    {
        private DAL dal = new DAL();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Calories()
        {
            return View("Calories");
        }

        [HttpPost]
        public ActionResult Calories(CaloriesCalculator model)
        {
            if (!ModelState.IsValid)
            {
                return View("Calories", model);
            }

            double calories;
            if (model.Gender == "Male")
            {
                calories = (12 * model.Weight) + (6.25 * model.Height) - (5 * model.Age) + 5;
            }
            else
            {
                calories = (10 * model.Weight) + (6.25 * model.Height) - (5 * model.Age) - 161;
            }

            if (model.Goal == "Lose Weight")
            {
                calories -= 200; 
            }
            else if (model.Goal == "Gain Weight")
            {
                calories += 200; 
            }

            return RedirectToAction("CaloriesResult", new { calories = calories });
        }

        public ActionResult CaloriesResult(double? calories)
        {
            double caloriesValue = calories ?? 0;

            ViewBag.Calories = caloriesValue;

            return View("CaloriesResult");
        }

        [HttpPost]
        public ActionResult SendEmailWithPDF(string email)
        {
            byte[] pdfBytes = GeneratePDF();

            SendEmailWithAttachment(email, pdfBytes);

            return RedirectToAction("CaloriesResult");
        }

        private byte[] GeneratePDF()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                document.Add(new Paragraph("This is a test PDF."));

                document.Close();
                return ms.ToArray();
            }
        }

        private void SendEmailWithAttachment(string email, byte[] attachmentBytes)
        {
            string hostName = ConfigurationManager.AppSettings["SMTPServer"];
            string userName = ConfigurationManager.AppSettings["SMTPUserName"];
            string password = ConfigurationManager.AppSettings["SMTPPassword"];
            int portNumber = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);

            string senderEmail = "alexguia10@yahoo.com";

            MailMessage mailMessage = new MailMessage(senderEmail, email);
            mailMessage.Subject = "Your Custom PDF";
            mailMessage.Body = "Please find attached the PDF you requested.";

            MemoryStream stream = new MemoryStream(attachmentBytes);
            mailMessage.Attachments.Add(new Attachment(stream, "C:\\Users\\Alex\\Desktop\\Licenta\\LicentaPDF.pdf"));

            SmtpClient smtpClient = new SmtpClient(hostName, portNumber);
            smtpClient.Credentials = new NetworkCredential(userName, password);
            smtpClient.EnableSsl = true;

            smtpClient.Send(mailMessage);
        }
    }
}
