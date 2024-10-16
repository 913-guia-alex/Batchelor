using Licenta.DataAbstractionLayer;
using Licenta.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Windows.Media.Imaging;

namespace Licenta.Controllers
{
    public class CoachController : Controller
    {
        private DAL dal = new DAL(); 

        public ActionResult Index()
        {
            List<Coach> coaches = dal.GetAllCoaches();
            return View(coaches);
        }

        public ActionResult Coaches()
        {
            List<Coach> coaches = dal.GetAllCoaches();
            return View(coaches);
        }

        public ActionResult AllCoaches()
        {
            List<Coach> coaches = dal.GetAllCoaches();
            return View(coaches);
        }

        public ActionResult AddCoach()
        {
            var gyms = dal.GetAllGyms();
            ViewBag.Gyms = gyms;
            return View("AddCoach");
        }

        public ActionResult DeleteCoachView(int id)
        {
            return View("DeleteCoach", id);
        }


        [HttpPost]
        public ActionResult DeleteCoach(int id)
        {
            DAL dal = new DAL();
            dal.DeleteCoach(id);
            return RedirectToAction("Coaches");
        }

        public ActionResult UpdateCoachView(int id)
        {
            DAL dal = new DAL();
            Coach coach = dal.GetCoach(id);
            return View("UpdateCoach", coach);
        }


        [HttpPost]
        public ActionResult UpdateCoach(Coach coach, HttpPostedFileBase photo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (photo != null && photo.ContentLength > 0)
                    {
                        byte[] photoData = new byte[photo.ContentLength];
                        photo.InputStream.Read(photoData, 0, photo.ContentLength);

                        Debug.WriteLine("Received photo data. Length: " + photoData.Length);

                        coach.photo = photoData;
                    }

                    dal.UpdateCoach(coach);

                    return RedirectToAction("Coaches");
                }
                else
                {
                    return View("UpdateCoach", coach);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error occurred while updating the coach: " + ex.Message;
                return View("UpdateCoach", coach);
            }
        }




        public ActionResult UpdateCoachGymView(int id)
        {
            DAL dal = new DAL();
            Coach coach = dal.GetCoach(id);
            return View("UpdateCoach", coach);
        }


        [HttpPost]
        public ActionResult UpdateCoachGym(Coach coach, HttpPostedFileBase photo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (photo != null && photo.ContentLength > 0)
                    {
                        byte[] photoData = new byte[photo.ContentLength];
                        photo.InputStream.Read(photoData, 0, photo.ContentLength);

                        Debug.WriteLine("Received photo data. Length: " + photoData.Length);

                        coach.photo = photoData;
                    }

                    dal.UpdateCoach(coach);

                    return RedirectToAction("Coaches");
                }
                else
                {
                    return View("UpdateCoachGym", coach);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                ViewBag.Error = "Error occurred while updating the coach: " + ex.Message;
                return View("UpdateCoachGym", coach);
            }
        }


        [HttpPost]
        public ActionResult SaveAddedCoach(Coach coach, HttpPostedFileBase photo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (photo != null && photo.ContentLength > 0)
                    {
                        byte[] photoData = new byte[photo.ContentLength];
                        photo.InputStream.Read(photoData, 0, photo.ContentLength);

                        coach.photo = photoData;
                    }

                    dal.AddNewCoach(coach);

                    return RedirectToAction("Coaches");
                }
                else
                {
                    return View("AddCoach", coach);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error occurred while adding the coach: " + ex.Message;
                return View("AddCoach", coach);
            }
        }




    }
}
