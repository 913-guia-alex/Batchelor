using Licenta.DataAbstractionLayer;
using Licenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Licenta.Controllers
{
    public class CustomerController : Controller
    {

        private DAL dal = new DAL(); // Create an instance of your DAL

        public ActionResult Index()
        {
            List<Customer> customers = dal.GetAllCustomers();
            return View(customers);
        }

        public ActionResult Main(int id)
        {
            // Retrieve customer details from the database based on the provided ID
            Customer customer = dal.GetCustomer(id);

            if (customer == null)
            {
                // Handle the case where the customer is not found
                return HttpNotFound();
            }

            // Retrieve gym cards associated with the customer
            List<GymCard> gymCards = dal.GetGymCardsByCustomerId(id);

            // Pass both customer and gym cards to the view
            ViewBag.Customer = customer;
            ViewBag.GymCards = gymCards;

            return View();
        }

        public ActionResult AddGymCard(int customerId, int gymId)
        {
            // Fetch the gym and customer entities from the database
            Gym gym = dal.GetGym(gymId);
            Customer customer = dal.GetCustomer(customerId);

            if (gym == null || customer == null)
            {
                // Handle case where gym or customer is not found
                return HttpNotFound();
            }

            // Calculate the gym card price based on the gym
            int gymCardPrice = CalculateGymCardPrice(gym);

            // Set the current date as the made date
            DateTime madeDate = DateTime.Now;

            // Set the expiration date as current date + 1 month
            DateTime expirationDate = madeDate.AddMonths(1);

            // Set ViewBag properties for customer email and gym name
            ViewBag.CustomerEmail = customer.email;
            ViewBag.GymName = gym.name;

            // Create a new GymCard object
            GymCard gymCard = new GymCard
            {
                idc = customerId,
                idg = gymId,
                price = gymCardPrice,
                madeDate = madeDate,
                expirationDate = expirationDate
            };

            // Pass the gym card model to the view
            return View(gymCard);
        }


        [HttpPost]
        public ActionResult SaveAddedGymCard(GymCard gymCard)
        {
            // Add the gym card to the database
            dal.AddGymCardToDatabase(gymCard);

            // Redirect the user to Gym/AllGymCustomer/{CustomerId}
            return RedirectToAction("AllGymCustomer", "Gym", new { id = gymCard.idc });
        }

        private int CalculateGymCardPrice(Gym gym)
        {
            // Example: Price calculation based on gym name
            switch (gym.name)
            {
                case "REVOgym":
                    return 120;
                case "Gym XYZ":
                    return 150;
                default:
                    return 100; // Default price for other gyms
            }
        }





        public ActionResult Favourite(int id)
        {
            Customer customer = dal.GetCustomer(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            List<Favourite> favourites = dal.GetFavouriteGymsByCustomerId(id);

            ViewBag.Customer = customer;
            ViewBag.Favourites = favourites;

            return View();
        }

        [HttpPost]
        public ActionResult RemoveFromFavorites(int favouriteId)
        {
            bool removed = dal.RemoveFromFavorites(favouriteId);

            if (removed)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, errorMessage = "Failed to remove gym from favorites." });
            }
        }


         [HttpPost]
        public ActionResult AddToFavourite(int customerId, int gymId)
        {
            // Check if the gym is already in the customer's favorite list
            if (dal.IsGymInFavorites(customerId, gymId))
            {
                // Gym is already in favorites, return a message or handle as needed
                return Json(new { success = false, message = "Gym is already in favorites." });
            }
            else
            {
                // Add the gym to the customer's favorite list
                dal.AddToFavourite(customerId, gymId);
                return Json(new { success = true });
            }
        }


        public ActionResult Customers()
        {
            List<Customer> customers = dal.GetAllCustomers();
            return View(customers);

        }

        public ActionResult AddCustomer()
        {
            return View("AddCustomer");
        }

        public ActionResult DeleteCustomerView(int id)
        {
            return View("DeleteCustomer", id);
        }

        [HttpPost]
        public ActionResult DeleteCustomer(int id)
        {
            DAL dal = new DAL();
            dal.DeleteCustomer(id);
            return RedirectToAction("Customers");
        }

        public ActionResult UpdateCustomerView(int id)
        {
            DAL dal = new DAL();
            Customer customer = dal.GetCustomer(id);
            return View("UpdateCustomer", customer);
        }

        public ActionResult UpdateCustomer(Customer customer)
        {
            DAL dal = new DAL();
            customer.surname = Request.Params["surname"];
            customer.lastname = Request.Params["lastname"];
            customer.gender = Request.Params["gender"];
            customer.email = Request.Params["email"];
            dal.UpdateCustomer(customer);
            return RedirectToAction("Customers");
        }

        public ActionResult SaveAddedCustomer()
        {
            Customer customer = new Customer();
            customer.surname = Request.Params["surname"];
            customer.lastname = Request.Params["lastname"];
            customer.age = int.Parse(Request.Params["age"]);
            customer.gender = Request.Params["gender"];
            customer.email = Request.Params["email"];
            customer.password = Request.Params["password"];


            DAL dal = new DAL();
            dal.AddNewCustomer(customer);
            return RedirectToAction("Customers");
        }

    }
}