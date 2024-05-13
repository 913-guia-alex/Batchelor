using Licenta.DataAbstractionLayer;
using Licenta.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Licenta.Controllers
{
    public class ReviewController : Controller
    {
        private DAL dal = new DAL(); // Create an instance of your DAL

        public ActionResult Index()
        {
            List<Review> reviews = dal.GetAllReviews();
            return View(reviews);
        }

        // GET: Review/CustomerReview/{gymId}
        public ActionResult CustomerReview(int? id)
        {
            if (id == null)
            {
                // Handle the case where gymId is not provided
                return RedirectToAction("Index", "Home");
            }

            // Fetch the Gym name from the database using the provided gymId
            string gymName = dal.GetGymNameById((int)id); // Replace GetGymNameById with the appropriate method in your DAL

            // Set the gym name in ViewBag
            ViewBag.GymName = gymName;

            // Assuming you have a model to pass to the view, such as Review
            Review review = new Review();
            review.idg = (int)id; // Set the gym ID in the review model

            return View(review);
        }

        // GET: Review/GymReviews/{gymId}
        public ActionResult GymReviews(int? id)
        {
            if (id == null)
            {
                // Handle the case where gymId is not provided
                return RedirectToAction("Index", "Home");
            }

            // Fetch the reviews for the gym from the database using the provided gymId
            List<Review> reviews = dal.GetReviewsByGymId((int)id); // Replace GetReviewsByGymId with the appropriate method in your DAL

            // Fetch the Gym name from the database using the provided gymId
            string gymName = dal.GetGymNameById((int)id); // Replace GetGymNameById with the appropriate method in your DAL

            // Set the gym name and reviews in ViewBag
            ViewBag.GymName = gymName;
            ViewBag.GymId = id; // Optionally, set the gym ID in ViewBag if needed

            return View(reviews);
        }


        [HttpPost]
        public ActionResult SaveAddedReview(Review review)
        {
            if (ModelState.IsValid)
            {
                // Get the Customer ID based on the provided email
                int customerId = dal.GetCustomerIdByEmail(review.customerEmail);

                // Check if the customer ID is valid
                if (customerId != -1)
                {
                    // Set the customer ID in the review model
                    review.idc = customerId;

                    // Add the review to the database
                    dal.AddReview(review);

                    // Redirect to the appropriate URL
                    return Redirect($"/Gym/AllGymCustomer/{customerId}");
                }
                else
                {
                    // Handle the case where the customer ID is not found
                    // For demonstration purposes, let's redirect back to the same action with an error message
                    ModelState.AddModelError("", "Customer not found. Please check your email.");
                    return View("CustomerReview", review);
                }
            }

            // If the model state is not valid, return to the previous page with errors
            // In this case, since there's no specific view for adding a review separately,
            // you might redirect back to the same action where the review was initially added
            // or handle it based on your application's flow
            return RedirectToAction("CustomerReview", new { id = review.idg });
        }


    }
}
