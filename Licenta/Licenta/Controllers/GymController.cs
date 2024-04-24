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

            DAL dal = new DAL();
            dal.AddNewGym(gym);
            return RedirectToAction("Gyms");
        }
    }
}