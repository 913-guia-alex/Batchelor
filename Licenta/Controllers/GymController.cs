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
        private DAL dal = new DAL();

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
            Gym gym = dal.GetGym(id);

            if (gym == null)
            {
                return HttpNotFound();
            }

            List<Coach> coaches = dal.GetCoachesByGymId(id);

            ViewBag.Gym = gym;
            ViewBag.Coach = coaches;

            return View();
        }


        public ActionResult GymDetails(int id)
        {
            Gym gym = dal.GetGym(id);

            if (gym == null)
            {
                return HttpNotFound();
            }

            List<Coach> coaches = dal.GetCoachesByGymId(id);

            ViewBag.Coaches = coaches;
            return View(gym);
        }



    }


}