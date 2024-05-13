using Licenta.DataAbstractionLayer;
using Licenta.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Licenta.Controllers
{
    public class AdminController : Controller
    {
        private DAL dal = new DAL();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Admins()
        {
            // Handle the admin panel action
            return View("Admins");
        }

        public ActionResult ManageGyms()
        {
            return RedirectToAction("Gyms", "Gym");

        }

        public ActionResult ManageCustomers()
        {
            return RedirectToAction("Customers", "Customer");
        }

    }
}
