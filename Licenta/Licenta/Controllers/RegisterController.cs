using Licenta.DataAbstractionLayer;
using Licenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Licenta.Controllers
{
    public class RegisterController : Controller
    {
        private DAL dal = new DAL();

        // GET: Authenticate
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            bool registrationSuccess = dal.registercustomer(customer);

            if (registrationSuccess)
            {
                // Redirect to the Documents view upon successful login
                return RedirectToAction("Login", "Authenticate");
            }

            ModelState.AddModelError("", "Registration failed. Please check your input and try again."); // Replace with an appropriate registration error message
            return View(customer);
        }
    }
}