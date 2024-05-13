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
        private DAL dal = new DAL(); // Create an instance of your DAL

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
                    // Check if a new photo is uploaded
                    if (photo != null && photo.ContentLength > 0)
                    {
                        // Convert the photo file to a byte array
                        byte[] photoData = new byte[photo.ContentLength];
                        photo.InputStream.Read(photoData, 0, photo.ContentLength);

                        // Log the length of the byte array
                        Debug.WriteLine("Received photo data. Length: " + photoData.Length);

                        // Assign the photo byte array to the coach's photo property
                        coach.photo = photoData;
                    }

                    // Call the DAL method to update the coach in the database
                    dal.UpdateCoach(coach);

                    // Redirect to the Coaches view
                    return RedirectToAction("Coaches");
                }
                else
                {
                    // Model validation failed, return to the UpdateCoach view with errors
                    return View("UpdateCoach", coach);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
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
                    // Check if a new photo is uploaded
                    if (photo != null && photo.ContentLength > 0)
                    {
                        // Convert the photo file to a byte array
                        byte[] photoData = new byte[photo.ContentLength];
                        photo.InputStream.Read(photoData, 0, photo.ContentLength);

                        // Log the length of the byte array
                        Debug.WriteLine("Received photo data. Length: " + photoData.Length);

                        // Assign the photo byte array to the coach's photo property
                        coach.photo = photoData;
                    }

                    // Call the DAL method to update the coach in the database
                    dal.UpdateCoach(coach);

                    // Redirect to the Coaches view
                    return RedirectToAction("Coaches");
                }
                else
                {
                    // Model validation failed, return to the UpdateCoach view with errors
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
                        // Convert the photo file to a byte array
                        byte[] photoData = new byte[photo.ContentLength];
                        photo.InputStream.Read(photoData, 0, photo.ContentLength);

                        // Assign the photo byte array to the coach's photo property
                        coach.photo = photoData;
                    }

                    // Call the DAL method to add the coach to the database
                    dal.AddNewCoach(coach);

                    // Redirect to the Coaches view
                    return RedirectToAction("Coaches");
                }
                else
                {
                    // Model validation failed, return to the AddCoach view with errors
                    return View("AddCoach", coach);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                ViewBag.Error = "Error occurred while adding the coach: " + ex.Message;
                return View("AddCoach", coach);
            }
        }




    }
}
