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
        // GET: Class
        private DAL dal = new DAL(); // Create an instance of your DAL

        public ActionResult Index()
        {
            // Fetch the list of classes from your data source
            List<Classes> classesList = dal.GetAllClasses();

            // Iterate through each class and fetch the coach's name and class type's type
            foreach (var classItem in classesList)
            {
                classItem.CoachName = dal.GetCoach(classItem.idco)?.name; // Fetch coach's name
                classItem.ClassTypeType = dal.GetClassType(classItem.idct)?.type; // Fetch class type's type
            }

            return View(classesList);
        }

        public ActionResult Classes()
        {
            // Fetch the list of classes from your data source
            List<Classes> classesList = dal.GetAllClasses();

            // Iterate through each class and fetch the coach's name and class type's type
            foreach (var classItem in classesList)
            {
                classItem.CoachName = dal.GetCoach(classItem.idco)?.name; // Fetch coach's name
                classItem.ClassTypeType = dal.GetClassType(classItem.idct)?.type; // Fetch class type's type
            }

            return View(classesList);

        }




        public ActionResult AllClasses()
        {
            // Fetch the list of classes from your data source
            List<Classes> classesList = dal.GetAllClasses();

            // Iterate through each class and fetch the coach's name and class type's type
            foreach (var classItem in classesList)
            {
                classItem.CoachName = dal.GetCoach(classItem.idco)?.name; // Fetch coach's name
                classItem.ClassTypeType = dal.GetClassType(classItem.idct)?.type; // Fetch class type's type
            }

            return View(classesList);
        }

        public ActionResult AddClass()
        {
            // Assuming you have methods to fetch class types and coaches from your data source
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
                    // Handle the case when idg is not provided
                    ViewBag.ErrorMessage = "Gym ID is missing.";
                    return View("Error");
                }

                var classTypes = dal.GetAllClassTypes();
                List<Coach> coaches;

                if (idg.HasValue)
                {
                    coaches = dal.GetAllCoachesForGym(idg.Value); // Use idg.Value to access the integer value
                }
                else
                {
                    // Handle the case when idg doesn't have a value
                    ViewBag.ErrorMessage = "Invalid Gym ID provided.";
                    return View("Error");
                }

                // Pass data to the view
                ViewBag.ClassTypes = classTypes;
                ViewBag.Coaches = coaches;
                ViewBag.GymId = idg; // Set ViewBag.GymId to the received idg

                return View("AddSpecificClass");
            }
            catch (Exception ex)
            {
                // Handle exception
                ViewBag.ErrorMessage = "An error occurred while fetching data: " + ex.Message;
                // Log the exception
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

            // Assuming you have methods to fetch class types and coaches from your data source
            List<Models.ClassType> classTypes = dal.GetAllClassTypes();
            List<Coach> coaches = dal.GetAllCoaches();

            ViewBag.ClassType = classTypes;
            ViewBag.Coach = coaches;

            dal.UpdateClass(classes); // Use the existing dal field
            return RedirectToAction("Classes");
        }

        public ActionResult UpdateClassView(int id)
        {
            DAL dal = new DAL();
            Classes classes = dal.GetClass(id);
            ViewBag.ClassType = dal.GetAllClassTypes(); // Populate ViewBag with class types
            ViewBag.Coach = dal.GetAllCoaches();
            return View("UpdateClass", classes);
        }


        [HttpPost]
        public ActionResult SaveAddedClass(Classes classes)
        {
            // Model binding automatically populates the classes parameter
            if (ModelState.IsValid)
            {
                dal.AddNewClass(classes); // Use the existing dal field
                return RedirectToAction("Classes");
            }
            // Handle invalid model state (e.g., return to the same view with validation errors)
            return View("AddClass", classes);
        }


        /*        public ActionResult GymClasses(int id)
                {
                    // Retrieve gym classes by gym id
                    List<Classes> classes = dal.GetClassesByGymId(id);

                    // Pass classes to the view
                    return View(classes);
                }

                public ActionResult GymClasses(int id)
                {
                    // Retrieve coaches by gym id
                    List<Coach> coaches = dal.GetCoachesByGymId(id);

                    // Initialize a list to store classes
                    List<Classes> gymClasses = new List<Classes>();

                    // Iterate through each coach and fetch classes associated with them
                    foreach (var coach in coaches)
                    {
                        // Fetch classes associated with the coach and add them to the gymClasses list
                        List<Classes> classes = dal.GetClassesByCoachId(coach.idco);
                        gymClasses.AddRange(classes);
                    }

                    // Pass the combined list of classes to the view
                    return View(gymClasses);
                }*/

        public ActionResult GymClasses(int id)
        {
            // Retrieve gym classes with coach information by gym id
            List<Classes> classes = dal.GetClassesWithCoachByGymId(id);

            // Pass classes to the view
            return View(classes);
        }


        public ActionResult CustomerClasses(int id)
        {
            // Retrieve gym classes with coach information by gym id
            List<Classes> classes = dal.GetClassesWithCoachByGymId(id);


            return View(classes);
        }

        [HttpGet]
        public ActionResult ClassesCustomer(int idc)
        {
            // Fetch the list of reservations for the given customer ID
            List<Reservation> reservationList = dal.GetReservationsByCustomerId(idc);

            // Iterate through each reservation and fetch additional details
            foreach (var reservation in reservationList)
            {
                reservation.CustomerEmail = dal.GetCustomer(reservation.idc)?.email; // Fetch customer's email
                reservation.ClassName = dal.GetClass(reservation.idcl)?.name; // Fetch gym name
            }

            // Pass the reservation list to the view
            return View(reservationList);

        }



        public ActionResult Enroll(int? classId)
        {
            if (classId == null)
            {
                // Handle case where classId is not provided
                return HttpNotFound();
            }

            // Fetch the class entity from the database
            Classes gymClass = dal.GetClass(classId.Value);

            if (gymClass == null)
            {
                // Handle case where class is not found
                return HttpNotFound();
            }

            // Create a new Reservation object with default values
            Reservation reservation = new Reservation
            {
                idcl = classId.Value,
                Date = DateTime.Today, // Set the default date to today
                Time = DateTime.Now.TimeOfDay, // Set the default time to current time
                ClassName = gymClass.name // Store the class name
            };

            // Pass the reservation model to the view
            return View(reservation);
        }


        // Action method to handle the form submission and save the reservation
        [HttpPost]
        public ActionResult SaveReservation(Reservation reservation, string customerEmail)
        {
            // Get the customer ID based on the provided email
            int customerId = dal.GetCustomerIdByEmail(customerEmail);

            // If customer ID is not found or customer email is not provided, handle the case accordingly
            if (customerId == 0 || string.IsNullOrEmpty(customerEmail))
            {
                // Handle the case where customer ID is not found or email is not provided
                // For example, display an error message to the user
                return RedirectToAction("Index", "Home"); // Redirect to the home page for now
            }

            // Set the customer ID in the reservation
            reservation.idc = customerId;

            // Add the reservation to the database
            dal.AddNewReservation(reservation);

            // Redirect the user to a success page or any other appropriate action
            return RedirectToAction("AllGymCustomer", "Gym", new { id = reservation.idc });
        }






        /* public ActionResult AddGymCard(int customerId, int gymId)
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
         }*/


    }
}