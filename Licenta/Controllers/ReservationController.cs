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
        private DAL dal = new DAL(); 

        public ActionResult Index()
        {
            List<Reservation> reservationList = dal.GetAllReservations();

            foreach (var reservastion in reservationList)
            {
                reservastion.CustomerEmail = dal.GetCustomer(reservastion.idc)?.email; 
                reservastion.ClassName = dal.GetClass(reservastion.idcl)?.name; 
            }

            return View(reservationList);
        }

        public ActionResult Reservations()
        {
            List<Reservation> reservationList = dal.GetAllReservations();

            foreach (var reservastion in reservationList)
            {
                reservastion.CustomerEmail = dal.GetCustomer(reservastion.idc)?.email; 
                reservastion.ClassName = dal.GetClass(reservastion.idcl)?.name; 
            }

            return View(reservationList);

        }

        public ActionResult AllReservations()
        {
            List<Reservation> reservationList = dal.GetAllReservations();

            foreach (var reservastion in reservationList)
            {
                reservastion.CustomerEmail = dal.GetCustomer(reservastion.idc)?.email;
                reservastion.ClassName = dal.GetClass(reservastion.idcl)?.name; 
            }

            return View(reservationList);
        }

        public ActionResult AddReservation()
        {
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

            List<Customer> customers = dal.GetAllCustomers();
            List<Classes> classes = dal.GetAllClasses();

            ViewBag.CustomerEmail = customers;
            ViewBag.ClassName = classes;

            dal.UpdateReservation(reservation); 
            return RedirectToAction("Reservations");
        }

        public ActionResult UpdateReservationView(int id)
        {
            DAL dal = new DAL();
            Reservation reservation = dal.GetReservation(id);
            ViewBag.CustomerEmail = dal.GetAllCustomers(); 
            ViewBag.ClassName = dal.GetAllClasses();
            return View("UpdateReservation", reservation);
        }


        [HttpPost]
        public ActionResult SaveAddedReservation(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                dal.AddNewReservation(reservation); 
                return RedirectToAction("Reservations");
            }
            return View("AddReservation", reservation);
        }



    }
}