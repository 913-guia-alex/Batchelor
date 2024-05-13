using Licenta.DataAbstractionLayer;
using Licenta.Models;
using System;
using System.Web.Mvc;

namespace Licenta.Controllers
{
    public class PasswordController : Controller
    {
        private DAL dal = new DAL();

        // GET: Password/ResetPassword
        public ActionResult ResetPassword()
        {
            return View();
        }

        // POST: Password/ResetPassword
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Call the DAL method to reset the password
                bool success = dal.ResetPassword(model.Email, model.NewPassword);

                if (success)
                {
                    ViewBag.SuccessMessage = "Password reset successfully!";
                    return RedirectToAction("Login", "Authenticate");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to reset the password. Please check your email and try again.");
                }
            }

            return View(model);
        }
    }
}
