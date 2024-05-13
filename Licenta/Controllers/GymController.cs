using Licenta.DataAbstractionLayer;
using Licenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Licenta.Controllers
{
    public class GymController : Controller
    {
        private DAL dal = new DAL(); // Create an instance of your DAL

        public ActionResult Index()
        {
            List<Gym> gyms = dal.GetAllGyms(); 
            return View(gyms); 
        }

        public ActionResult Gyms()
        {
            List<Gym> gyms = dal.GetAllGyms();
            return View(gyms);

        }

        public ActionResult AllGyms()
        {
            List<Gym> gyms = dal.GetAllGyms();
            return View(gyms);
        }

        public ActionResult AllGymCustomer()
        {
            List<Gym> gyms = dal.GetAllGyms();
            ViewBag.AllGyms = gyms;
            return View();
        }


        /*        public ActionResult AllGymCustomer()
                {
                    // Fetch gyms from the data source
                    List<Gym> gyms = dal.GetAllGyms();

                    // Pass the gyms and customer ID to the view
                    ViewBag.AllGyms = gyms;
                    ViewBag.CustomerId = getCustomerId(); // Pass the customer ID to the view

                    return View();
                }

                // Method to fetch the customer ID
                private int getCustomerId()
                {
                    // Extract customer ID from the session or any other source
                    int customerId = 0; // Initialize the customer ID variable

                    // Check if the session contains the customer ID
                    if (Session["CustomerId"] != null)
                    {
                        // If the session contains the customer ID, retrieve it
                        customerId = (int)Session["CustomerId"];
                    }
                    else
                    {
                        // If the session does not contain the customer ID, handle the case accordingly
                        // For example, redirect the user to the login page
                        // Or display a message indicating that the user is not logged in
                    }

                    return customerId;
                }*/




        public ActionResult AddGym()
        {
            return View("AddGym");
        }

        public ActionResult DeleteGymView(int id)
        {
            return View("DeleteGym", id);
        }

        [HttpPost]
        public ActionResult DeleteGym(int id)
        {
            DAL dal = new DAL();
            dal.DeleteGym(id);
            return RedirectToAction("Gyms");
        }

        public ActionResult UpdateGymView(int id)
        {
            DAL dal = new DAL();
            Gym gym = dal.GetGym(id);
            return View("UpdateGym", gym);
        }

        public ActionResult UpdateGym(Gym gym)
        {
            DAL dal = new DAL();
            gym.name = Request.Params["name"];
            gym.adress = Request.Params["adress"];
            gym.openHour = Request.Params["openHour"];
            gym.closeHour = Request.Params["closeHour"];
            gym.password = Request.Params["password"];
            dal.UpdateGym(gym);
            return RedirectToAction("Gyms");
        }

        public ActionResult SaveAddedGym()
        {
            Gym gym = new Gym();
            //doc.Id = int.Parse(Request.Params["docID"]);
            gym.name = Request.Params["name"];
            gym.adress = Request.Params["adress"];
            gym.openHour = Request.Params["openHour"];
            gym.closeHour = Request.Params["closeHour"];
            gym.rating = float.Parse(Request.Params["rating"]);
            gym.password = Request.Params["password"];


            DAL dal = new DAL();
            dal.AddNewGym(gym);
            return RedirectToAction("Gyms");
        }



        public ActionResult GymCoaches(int id)
        {
            // Retrieve customer details from the database based on the provided ID
            Gym gym = dal.GetGym(id);

            if (gym == null)
            {
                // Handle the case where the customer is not found
                return HttpNotFound();
            }

            // Retrieve gym cards associated with the customer
            List<Coach> coaches = dal.GetCoachesByGymId(id);

            // Pass both customer and gym cards to the view
            ViewBag.Gym = gym;
            ViewBag.Coach = coaches;

            return View();
        }


        public ActionResult GymDetails(int id)
        {
            // Retrieve gym details from the database based on the provided ID
            Gym gym = dal.GetGym(id);

            if (gym == null)
            {
                // Handle the case where the gym is not found
                return HttpNotFound();
            }

            // Retrieve coaches associated with the gym
            List<Coach> coaches = dal.GetCoachesByGymId(id);

            // Pass the gym and coaches to the view
            ViewBag.Coaches = coaches;
            return View(gym);
        }



    }


}