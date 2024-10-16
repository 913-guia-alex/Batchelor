using Licenta.DataAbstractionLayer;
using Licenta.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Licenta.Controllers
{
    public class ReviewController : Controller
    {
        private DAL dal = new DAL();

        public ActionResult Index()
        {
            List<Review> reviews = dal.GetAllReviews();
            return View(reviews);
        }

        public ActionResult CustomerReview(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            string gymName = dal.GetGymNameById((int)id);

            ViewBag.GymName = gymName;

            Review review = new Review();
            review.idg = (int)id; 

            return View(review);
        }

        public ActionResult GymReviews(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Review> reviews = dal.GetReviewsByGymId((int)id); 

            string gymName = dal.GetGymNameById((int)id); 

            ViewBag.GymName = gymName;
            ViewBag.GymId = id; 

            return View(reviews);
        }


        [HttpPost]
        public ActionResult SaveAddedReview(Review review)
        {
            if (ModelState.IsValid)
            {
                int customerId = dal.GetCustomerIdByEmail(review.customerEmail);

                if (customerId != -1)
                {
                    review.idc = customerId;

                    dal.AddReview(review);

                    return Redirect($"/Gym/AllGymCustomer/{customerId}");
                }
                else
                {
                    ModelState.AddModelError("", "Customer not found. Please check your email.");
                    return View("CustomerReview", review);
                }
            }

            return RedirectToAction("CustomerReview", new { id = review.idg });
        }


    }
}
