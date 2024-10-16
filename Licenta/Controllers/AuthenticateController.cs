﻿using Licenta.DataAbstractionLayer;
using Licenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Licenta.Controllers
{
    public class AuthenticateController : Controller
    {
        private DAL dal = new DAL();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string role = dal.Login(model.Email, model.Password);

            if (role == "admin")
            {
                return RedirectToAction("Admins", "Admin");
            }
            else if (role == "customer")
            {
                int customerId = dal.GetCustomerIdByEmail(model.Email);
                TempData["CustomerId"] = customerId;

                return RedirectToAction("Main", "Customer", new { id = customerId });
            }
            else if (role == "gym")
            {
                int gymId = dal.GetGymIdByName(model.Email);
                TempData["GymId"] = gymId;

                return RedirectToAction("GymCoaches", "Gym", new { id = gymId });
            }
            else
            {
                ModelState.AddModelError("", "Invalid login.");
                return View(model);
            }
        }


    }
}