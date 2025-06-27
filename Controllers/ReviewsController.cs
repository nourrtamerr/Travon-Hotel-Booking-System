using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCBookingFinal_YARAB_.Data;
using MVCBookingFinal_YARAB_.Models;
using Stripe;

namespace MVCBookingFinal_YARAB_.Controllers
{
    public class ReviewsController(IReviewsService ReviewContext,IHotelService HotelContext, IUserService _userService, UserManager<AppUser> _userManager, ApplicationDbContext _context) : Controller
    {

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }
            else
            {

                var user = await _userManager.FindByIdAsync(userId);


                var userOldTrips = _context.ReservationRooms
                    .Where(
                    r => r.UserId == userId
                    && r.Reservation.reservationStatus == ReservationStatus.Confirmed
                    && (DateTime.Now > r.Reservation.CheckOutDate)
                    
                    )
                    .Include(r => r.Room).ThenInclude(r => r.Hotel).ThenInclude(h=>h.city)
                    .Include(r => r.Reservation)
                    .Include(r => r.Room).ThenInclude(r => r.Hotel).ThenInclude(h=>h.Reviewed)
                    .ToList();
               

                List<MyReviewReservationVM> x = new List<MyReviewReservationVM>();
                foreach(var item in userOldTrips)
                {
                    MyReviewReservationVM neww = new MyReviewReservationVM
                    {
                        HotelId = item.Room.HotelId,
                        HotelName = item.Room.Hotel.Name,
                        CityName = item.Room.Hotel.city.Name,
                        ReservationId = item.ReservationId,
                        CheckInDate = item.Reservation.CheckInDate,
                        CheckOutDate = item.Reservation.CheckOutDate,
                        //UserReview = item.Room.Hotel.Reviewed.Any(r => r.UserId == userId) ? item.Room.Hotel.Reviewed.FirstOrDefault(r => r.UserId == userId) : null
                        UserReview = item.Room.Hotel.Reviewed
                        .Where(r => r.UserId == userId && !r.isDeleted)
                        .OrderByDescending(r => r.ReviewDate)
                        .FirstOrDefault()
                    };

                    x.Add(neww);
                }
                

                bool? res = _context.ReservationRooms?.Any(r => r.UserId == userId);

               
                ViewBag.HasAnyReservations = res;
                //ViewBag.ReviewedHotels = Reviewed ?? new List<ReservationRoom>();

                ViewBag.Usertr = x;


                ViewBag.ProfilePicture = user.ProfilePicture;
                ViewBag.FirstName = user.FirstName;
                ViewBag.LasttName = user.LastName;
                ViewBag.Email = user.Email;

                var allReviews = ReviewContext.GetAllReview();
                return View(allReviews);

            }


            

            //}
            //    var user = _userManager.GetUserId(User);
            //    if (string.IsNullOrEmpty(user))
            //    {
            //        return Unauthorized();
            //    }

            //    var userReviews = ReviewContext.GetReviewByUser(user);

            //    return View(userReviews);

                
            
        }


        [Authorize]
        public async Task< IActionResult> MyReviews()
        {
            var id = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(id);


            


            ViewBag.ProfilePicture = user.ProfilePicture;
            ViewBag.FirstName = user.FirstName;
            ViewBag.LasttName = user.LastName;
            ViewBag.Email = user.Email;
            var allReviews = ReviewContext.GetReviewByUser(User.FindFirstValue(ClaimTypes.NameIdentifier));


            return View("Index",allReviews);
            
        }
      
     

        // GET: Reviews/Details/5
        public IActionResult GetTopReviews()
        {
            var reviews = ReviewContext.GetAllReview();
            return View("Index",reviews);
          
        }
        
        public IActionResult Details(int id)
        {
            var review = ReviewContext.GetReviewById(id);
            var user = _userManager.Users.FirstOrDefault(u => u.Id == review.UserId);
            var hotel = HotelContext.GetById(review.HotelId);
            ViewBag.Hotel = hotel.Name;


            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }



        // GET: Reviews/Create
      

        [Authorize]
        public IActionResult Create(int id)
        {
            ViewBag.Hotel = HotelContext.GetById(id);
            return View();
        }






        // POST: Reviews/Create
        [HttpPost]
        //[Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReviewViewModel review)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }
           

            if (ModelState.IsValid)
            {

                ReviewContext.CreateReview(review, userId);
                return RedirectToAction(nameof(Index));
            }
            //ViewBag.Hotel = new SelectList(HotelContext.GetAll(), "id", "Name");

            ViewBag.Hotel = HotelContext.GetById(review.HotelId);
         
            return View(review);
        }





        // GET: Reviews/Edit/5
        //[Authorize(Roles = "User")]
        [ServiceFilter(typeof(AuthorFilter))]
        public IActionResult Edit(int id)
        {
            var review = ReviewContext.GetReviewById(id);
            ReviewViewModel reviewViewModel = new()
            {
                //Id = id,
                Rating = review.Rating,
                Description = review.Description,
                //ReviewDate = review.ReviewDate
            };
          
            if (review == null)
            {
                return NotFound();
            }
            return View(reviewViewModel);

        }






        // POST: Reviews/Edit/5
        [HttpPost]
		//[Authorize(Roles = "User")]
		[ServiceFilter(typeof(AuthorFilter))]
		[ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,HotelId,UserId,Rating,Description,ReviewDate")] ReviewViewModel reviewVM)
        {
            try
            {
                ReviewContext.UpdateReview(reviewVM);
            }

            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(reviewVM);
            }

            return RedirectToAction(nameof(Index));

        }









        // GET: Reviews/Delete/5
        [Authorize]
        [ServiceFilter(typeof(AuthorAndAdminFilter))]
        public IActionResult Delete(int id)
        {
            var review = ReviewContext.GetReviewById(id);
            var hotel = HotelContext.GetById(review.HotelId);
            var user = _userManager.Users.FirstOrDefault(u => u.Id == review.UserId);
            ViewBag.UserName = user.UserName;
            ViewBag.Hotel = hotel.Name;


            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }







        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
		[ServiceFilter(typeof(AuthorAndAdminFilter))]
		[ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var review = ReviewContext.GetReviewById(id);
            if (review != null)
            {
                ReviewContext.DeleteReview(id);
            }

            return RedirectToAction(nameof(Index));
        }











        [Authorize]
        public async Task<IActionResult> MyTripsAndReviews()
        {
            var userId = _userManager.GetUserId(User);

            // 1. Get all past confirmed reservations for this user
            var pastRooms = _context.ReservationRooms
                .Include(rr => rr.Room).ThenInclude(r => r.Hotel)
                .Include(rr => rr.Reservation)
                .Where(rr => rr.UserId == userId
                    && rr.Reservation.reservationStatus == ReservationStatus.Confirmed
                    && rr.Reservation.CheckOutDate < DateTime.Now)
                .ToList();

            // 2. Get distinct hotels from these rooms
            var pastHotelIds = pastRooms.Select(rr => rr.Room.Hotel.id).Distinct().ToList();
            var pastHotels = _context.Hotels.Where(h => pastHotelIds.Contains(h.id)).ToList();

            // 3. Get user's reviews
            var userReviews = ReviewContext.GetReviewByUser(userId);

            // 4. Hotels not yet reviewed by user
            var reviewedHotelIds = userReviews.Select(r => r.HotelId).ToList();
            var hotelsToReview = pastHotels.Where(h => !reviewedHotelIds.Contains(h.id)).ToList();

            // Pass both lists to the view (use a ViewModel)
            var vm = new MyTripsAndReviewsViewModel
            {
                HotelsToReview = hotelsToReview,
                UserReviews = userReviews
            };

            return View(vm);
        }

    }

}
