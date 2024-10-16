using Licenta.DataAbstractionLayer;
using Licenta.Models;
using System;
using System.Web.Mvc;

namespace Licenta.Controllers
{
    public class PasswordController : Controller
    {
        private DAL dal = new DAL();

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool success = dal.ResetPassword(model.Email, model.NewPassword);

                if (success)
                {
                    ViewBag.SuccessMessage = "Password reset successfully!";
                    return RedirectToAction("Login", "Authenticate");
                }
                ModelState.AddModelError("", "Email not found. Try another email"); 
                
            }

            return View(model);
        }
    }
}
