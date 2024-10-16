using Licenta.DataAbstractionLayer;
using Licenta.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Licenta.Controllers
{
    public class ClassController : Controller
    {
        private DAL dal = new DAL();

        public ActionResult Index()
        {
            List<Classes> classesList = dal.GetAllClasses();

            foreach (var classItem in classesList)
            {
                classItem.CoachName = dal.GetCoach(classItem.idco)?.name;
                classItem.ClassTypeType = dal.GetClassType(classItem.idct)?.type;
            }

            return View(classesList);
        }

        public ActionResult Classes()
        {
            List<Classes> classesList = dal.GetAllClasses();

            foreach (var classItem in classesList)
            {
                classItem.CoachName = dal.GetCoach(classItem.idco)?.name;
                classItem.ClassTypeType = dal.GetClassType(classItem.idct)?.type;
            }

            return View(classesList);
        }

        public ActionResult AllClasses()
        {
            List<Classes> classesList = dal.GetAllClasses();

            foreach (var classItem in classesList)
            {
                classItem.CoachName = dal.GetCoach(classItem.idco)?.name;
                classItem.ClassTypeType = dal.GetClassType(classItem.idct)?.type;
            }

            return View(classesList);
        }

        public ActionResult AddClass()
        {
            List<Models.ClassType> classTypes = dal.GetAllClassTypes();
            List<Coach> coaches = dal.GetAllCoaches();

            ViewBag.ClassType = classTypes;
            ViewBag.Coach = coaches;

            return View("AddClass");
        }

        public ActionResult AddSpecificClass(int? idg)
        {
            try
            {
                if (idg == null)
                {
                    ViewBag.ErrorMessage = "Gym ID is missing.";
                    return View("Error");
                }

                var classTypes = dal.GetAllClassTypes();
                List<Coach> coaches;

                if (idg.HasValue)
                {
                    coaches = dal.GetAllCoachesForGym(idg.Value);
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid Gym ID provided.";
                    return View("Error");
                }

                ViewBag.ClassTypes = classTypes;
                ViewBag.Coaches = coaches;
                ViewBag.GymId = idg;

                return View("AddSpecificClass");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while fetching data: " + ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult DeleteClass(int id)
        {
            DAL dal = new DAL();
            dal.DeleteClass(id);
            return RedirectToAction("Classes");
        }

        public ActionResult DeleteClassView(int id)
        {
            return View("DeleteClass", id);
        }

        [HttpPost]
        public ActionResult UpdateClass(Classes classes)
        {
            List<Models.ClassType> classTypes = dal.GetAllClassTypes();
            List<Coach> coaches = dal.GetAllCoaches();

            ViewBag.ClassType = classTypes;
            ViewBag.Coach = coaches;

            dal.UpdateClass(classes);
            return RedirectToAction("Classes");
        }

        public ActionResult UpdateClassView(int id)
        {
            DAL dal = new DAL();
            Classes classes = dal.GetClass(id);
            ViewBag.ClassType = dal.GetAllClassTypes();
            ViewBag.Coach = dal.GetAllCoaches();
            return View("UpdateClass", classes);
        }

        [HttpPost]
        public ActionResult SaveAddedClass(Classes classes)
        {
            if (ModelState.IsValid)
            {
                dal.AddNewClass(classes);
                return RedirectToAction("Classes");
            }
            return View("AddClass", classes);
        }

        public ActionResult GymClasses(int id)
        {
            List<Classes> classes = dal.GetClassesWithCoachByGymId(id);
            return View(classes);
        }

        public ActionResult CustomerClasses(int id)
        {
            List<Classes> classes = dal.GetClassesWithCoachByGymId(id);
            return View(classes);
        }

        [HttpGet]
        public ActionResult ClassesCustomer(int idc)
        {
            List<Reservation> reservationList = dal.GetReservationsByCustomerId(idc);

            foreach (var reservation in reservationList)
            {
                var customer = dal.GetCustomer(reservation.idc);
                var gymClass = dal.GetClass(reservation.idcl);

                reservation.CustomerEmail = customer?.email;
                reservation.ClassName = gymClass?.name;
                reservation.ClassDate = gymClass?.date ?? DateTime.MinValue;
                reservation.ClassTime = gymClass?.time ?? TimeSpan.Zero;
            }

            return View(reservationList);
        }

        public ActionResult Enroll(int? classId)
        {
            if (classId == null)
            {
                return HttpNotFound();
            }

            Classes gymClass = dal.GetClass(classId.Value);

            if (gymClass == null)
            {
                return HttpNotFound();
            }

            Reservation reservation = new Reservation
            {
                idcl = classId.Value,
                Date = DateTime.Today,
                Time = DateTime.Now.TimeOfDay,
                ClassName = gymClass.name
            };

            return View(reservation);
        }

        [HttpPost]
        public ActionResult SaveReservation(Reservation reservation, string customerEmail)
        {
            int customerId = dal.GetCustomerIdByEmail(customerEmail);

            if (customerId == 0 || string.IsNullOrEmpty(customerEmail))
            {
                return RedirectToAction("Index", "Home");
            }

            reservation.idc = customerId;
            dal.AddNewReservation(reservation);
            return RedirectToAction("AllGymCustomer", "Gym", new { id = reservation.idc });
        }
    }
}
