using Licenta.DataAbstractionLayer;
using Licenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Licenta.Controllers
{
    public class ReservationController : Controller
    {
        private DAL dal = new DAL(); // Create an instance of your DAL

        public ActionResult Index()
        {
            // Fetch the list of classes from your data source
            List<Reservation> reservationList = dal.GetAllReservations();

            // Iterate through each class and fetch the coach's name and class type's type
            foreach (var reservastion in reservationList)
            {
                reservastion.CustomerEmail = dal.GetCustomer(reservastion.idc)?.email; // Fetch coach's name
                reservastion.ClassName = dal.GetClass(reservastion.idcl)?.name; // Fetch class type's type
            }

            return View(reservationList);
        }

        public ActionResult Reservations()
        {
            // Fetch the list of classes from your data source
            List<Reservation> reservationList = dal.GetAllReservations();

            // Iterate through each class and fetch the coach's name and class type's type
            foreach (var reservastion in reservationList)
            {
                reservastion.CustomerEmail = dal.GetCustomer(reservastion.idc)?.email; // Fetch coach's name
                reservastion.ClassName = dal.GetClass(reservastion.idcl)?.name; // Fetch class type's type
            }

            return View(reservationList);

        }

        public ActionResult AllReservations()
        {
            // Fetch the list of classes from your data source
            List<Reservation> reservationList = dal.GetAllReservations();

            // Iterate through each class and fetch the coach's name and class type's type
            foreach (var reservastion in reservationList)
            {
                reservastion.CustomerEmail = dal.GetCustomer(reservastion.idc)?.email; // Fetch coach's name
                reservastion.ClassName = dal.GetClass(reservastion.idcl)?.name; // Fetch class type's type
            }

            return View(reservationList);
        }

        public ActionResult AddReservation()
        {
            // Assuming you have methods to fetch class types and coaches from your data source
            List<Customer> customers = dal.GetAllCustomers();
            List<Classes> classes = dal.GetAllClasses();

            ViewBag.CustomerEmail = customers;
            ViewBag.ClassName = classes;

            return View("AddReservation");
        }



        [HttpPost]

        public ActionResult DeleteReservation(int id)
        {
            DAL dal = new DAL();
            dal.DeleteReservation(id);
            return RedirectToAction("Reservations");
        }


        public ActionResult DeleteReservationView(int id)
        {
            return View("DeleteReservation", id);
        }

        [HttpPost]
        public ActionResult UpdateReservation(Reservation reservation)
        {

            // Assuming you have methods to fetch class types and coaches from your data source
            List<Customer> customers = dal.GetAllCustomers();
            List<Classes> classes = dal.GetAllClasses();

            ViewBag.CustomerEmail = customers;
            ViewBag.ClassName = classes;

            dal.UpdateReservation(reservation); // Use the existing dal field
            return RedirectToAction("Reservations");
        }

        public ActionResult UpdateReservationView(int id)
        {
            DAL dal = new DAL();
            Reservation reservation = dal.GetReservation(id);
            ViewBag.CustomerEmail = dal.GetAllCustomers(); // Populate ViewBag with class types
            ViewBag.ClassName = dal.GetAllClasses();
            return View("UpdateReservation", reservation);
        }


        [HttpPost]
        public ActionResult SaveAddedReservation(Reservation reservation)
        {
            // Model binding automatically populates the classes parameter
            if (ModelState.IsValid)
            {
                dal.AddNewReservation(reservation); // Use the existing dal field
                return RedirectToAction("Reservations");
            }
            // Handle invalid model state (e.g., return to the same view with validation errors)
            return View("AddReservation", reservation);
        }

/*        public ActionResult ClassesCustomer(int idc)
        {
            // Fetch the list of reservations for the given customer ID
            List<Reservation> reservationList = dal.GetReservationsByCustomerId(idc);

            // Iterate through each reservation and fetch additional details
            foreach (var reservation in reservationList)
            {
                reservation.CustomerEmail = dal.GetCustomer(idc)?.email; // Fetch customer's email
                reservation.ClassName = dal.GetGym(reservation.idc)?.name; // Fetch gym name
            }

            // Pass the reservation list to the view
            return View(reservationList);
        }*/


    }
}