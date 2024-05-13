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
            // Handle the admin panel action
            return View("Calories");
        }

        [HttpPost]
        public ActionResult Calories(CaloriesCalculator model)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                // If the model is not valid, return the same view with validation errors
                return View("Calories", model);
            }

            // Calculate necessary calories based on the user's input
            double calories;
            if (model.Gender == "Male")
            {
                calories = (12 * model.Weight) + (6.25 * model.Height) - (5 * model.Age) + 5;
            }
            else
            {
                calories = (10 * model.Weight) + (6.25 * model.Height) - (5 * model.Age) - 161;
            }

            // Handle the goal to adjust calories if needed
            if (model.Goal == "Lose Weight")
            {
                // Adjust calories for losing weight
                calories -= 200; // For example, subtract 500 calories for weight loss
            }
            else if (model.Goal == "Gain Weight")
            {
                // Adjust calories for gaining weight
                calories += 200; // For example, add 500 calories for weight gain
            }

            // Redirect to the CaloriesResult action with the calculated calories
            return RedirectToAction("CaloriesResult", new { calories = calories });
        }

        public ActionResult CaloriesResult(double? calories)
        {
            // Check if calories is null, if so, assign a default value (0 in this case)
            double caloriesValue = calories ?? 0;

            // Pass the calculated calories to the view
            ViewBag.Calories = caloriesValue;

            // Return the CaloriesResult view
            return View("CaloriesResult");
        }

        [HttpPost]
        public ActionResult SendEmailWithPDF(string email)
        {
            // Generate PDF
            byte[] pdfBytes = GeneratePDF();

            // Send PDF as an attachment to the provided email
            SendEmailWithAttachment(email, pdfBytes);

            // Redirect back to the calories result page
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
            // Your SMTP server settings from App.config or Web.config
            string hostName = ConfigurationManager.AppSettings["SMTPServer"];
            string userName = ConfigurationManager.AppSettings["SMTPUserName"];
            string password = ConfigurationManager.AppSettings["SMTPPassword"];
            int portNumber = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);

            string senderEmail = "alexguia10@yahoo.com";

            MailMessage mailMessage = new MailMessage(senderEmail, email);
            mailMessage.Subject = "Your Custom PDF";
            mailMessage.Body = "Please find attached the PDF you requested.";

            // Attach the PDF
            MemoryStream stream = new MemoryStream(attachmentBytes);
            mailMessage.Attachments.Add(new Attachment(stream, "C:\\Users\\Alex\\Desktop\\Licenta\\LicentaPDF.pdf"));

            // Initialize SMTP client with settings from App.config or Web.config
            SmtpClient smtpClient = new SmtpClient(hostName, portNumber);
            smtpClient.Credentials = new NetworkCredential(userName, password);
            smtpClient.EnableSsl = true;

            // Send email
            smtpClient.Send(mailMessage);
        }
    }
}
