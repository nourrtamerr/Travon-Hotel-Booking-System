using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using MVCBookingFinal_YARAB_.IRepositories;
using MVCBookingFinal_YARAB_.Models;
using MVCBookingFinal_YARAB_.Repositories;
using Newtonsoft.Json;
using Stripe;
using System.Diagnostics.Metrics;
using System.Security.Claims;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MVCBookingFinal_YARAB_.Controllers
{
    public class ReservationController(IRoomService roomService, IFavoritesService favoritesService, IHotelService hotelservice, UserManager<AppUser> usermanager, IReservationService reservationservice) : Controller
    {

        // GET: ReservationController
        public ActionResult Index()
        {
            return View();
        }
        public IActionResult nextPage(int pagenum)
        {
            SearchViewModel myvm = JsonConvert.DeserializeObject<SearchViewModel>(TempData.Peek("myviewmodel").ToString());
            //TempData.Keep("myviewmodel");
            //        int totalItems = hotelservice.GetAllFilteredCount(
            //    city: vm.CountryOrCity,
            //    country: vm.CountryOrCity,
            //    vm: vm
            //);
            //ViewBag.TotalItems = totalItems;
            ViewBag.PageSize = 10;
            ViewBag.PageNum = pagenum;

            int totalItems = hotelservice.GetAllFilteredCount(city: myvm.CountryOrCity, country: myvm.CountryOrCity, vm: myvm).Count();

            ViewBag.TotalItems = totalItems;


            var hotelsquery = hotelservice.GetAllFilteredPaginated(PerPage: 10,

                pagenum: pagenum,
                city: myvm.CountryOrCity,
                country: myvm.CountryOrCity,
                vm: myvm
                );
            //ViewBag.TotalItems = hotelsquery.Count();

            //TempData["myviewmodel"] = JsonConvert.SerializeObject(myvm);
            return View("ViewHotels", hotelsquery);
        }
        public IActionResult PicturePress(string countryorcity)
        {

            var hotelsquery = hotelservice.GetAllFilteredPaginated(new SearchViewModel()
            {
                AdultsNumber = 0,
                ChildrenNumber = 0,
                CheckInDate = DateTime.Now.AddYears(99),
                CheckOutDate = DateTime.Now.AddYears(199),
                CountryOrCity = null,
                NumberOfRooms = 0,

            },
                PerPage: 10,
                pagenum: 0,
                city: countryorcity,
                country: countryorcity
                );
            return View("ViewRooms", hotelsquery.Take(5));
        }




        public IActionResult ViewHotels([FromQuery] SearchViewModel vm, int pagenum = 0)
        {

            if (vm.CheckInDate > vm.CheckOutDate)
            {
                return RedirectToAction("index", "home");
            }
            ViewBag.PageNum = pagenum;
            TempData["myviewmodel"] = JsonConvert.SerializeObject(vm);
            ViewBag.PageSize = 10;

            int totalItems = hotelservice.GetAllFilteredCount(city: vm.CountryOrCity, country: vm.CountryOrCity, vm: vm).Count();

            ViewBag.TotalItems = totalItems;



            var hotelsquery = hotelservice.GetAllFilteredPaginated(PerPage: 10,
                //PerPage: 10,
                pagenum: pagenum,
                city: vm.CountryOrCity,
                country: vm.CountryOrCity,
                vm: vm
                );
            var userId = usermanager.GetUserId(User);
            var favoriteHotelIds = favoritesService.GetFavoritesByUser(userId).Select(f => f.HotelId).ToList();
            ViewBag.FavoriteHotelIds = favoriteHotelIds;

            return View(hotelsquery);
        }



        public IActionResult FilterHotels(HotelFilterViewModel model)
        {

            //int amenitiesBitmask = 0;
            //if (Request.Form["Amenities"].Count > 0)
            //{
            //    foreach (var val in Request.Form["Amenities"])
            //    {
            //        amenitiesBitmask |= int.Parse(val);
            //    }
            //    model.Amenities = (AmenityEnum)amenitiesBitmask;
            //}

            SearchViewModel myvm = JsonConvert.DeserializeObject<SearchViewModel>(TempData.Peek("myviewmodel").ToString());

            int totalItems = hotelservice.GetAllFilteredCount(city: myvm.CountryOrCity, country: myvm.CountryOrCity, vm: myvm).Count();

            ViewBag.TotalItems = totalItems;
            ViewBag.PageNum = 0;
            var hotelsquery = hotelservice.GetAllFilteredPaginated(PerPage: 10,
                pagenum: 0,
                city: myvm.CountryOrCity,
                country: myvm.CountryOrCity,
                vm: myvm
                ).Where(h => (h.Ameneties.Amenities & model.Amenities) == model.Amenities)
                .OrderByDescending(h => model.Sorting == HotelsortBy.StarsRating ? h.starRating :
                model.Sorting == HotelsortBy.ReviewsRating ?
                (h.Reviewed.Count() == 0 ? 0 : h.Reviewed.Sum(r => r.Rating) / h.Reviewed.Count()) :
                h.id);
            //ViewBag.TotalItems = hotelsquery.Count();
            ViewBag.Sorting = model.Sorting;
            ViewBag.Amenities = model.Amenities;

            var selectedAmenities = Enum.GetValues(typeof(AmenityEnum))
    .Cast<AmenityEnum>()
    .Where(a =>model.Amenities.HasFlag(a))
    .Select(a => (int)a)
    .ToList();
            ViewBag.SelectedAmenities = selectedAmenities;
            return View(nameof(ViewHotels), hotelsquery);
        }

        public IActionResult GoToHotel(int id, RoomFilterationViewModel vm = null, bool checkfilteration = false)
        {
            SearchViewModel myvm;

            if (TempData.Peek("myviewmodel") is not null)
            {
                myvm = JsonConvert.DeserializeObject<SearchViewModel>(TempData.Peek("myviewmodel")?.ToString());
            }
            else
            {
                myvm = new()
                {
                    AdultsNumber = 1,
                    NumberOfRooms = 1,
                    ChildrenNumber = 0,
                    CountryOrCity = null
                };


            }
            //TempData.Keep("myviewmodel");
            var result = roomService.GetCombinationsByHotelId(id, myvm);
            ViewBag.AllHotels = result;
            if (result.Count() == 0)
            {
                return RedirectToAction(nameof(Index), "home");
            }
            ViewBag.Hotel = result.FirstOrDefault().FirstOrDefault().Hotel;
            //ViewBag.CanMatchAmenties = true;
            //TempData["myviewmodel"] = JsonConvert.SerializeObject(myvm);
            if (!checkfilteration)
            {

                ViewBag.HasCombinations = true;
                return View(result);

            }
            else
            {
                var query = result.Where(combination => (combination.Sum(r => r.PricePerNight) >= (int)vm.minPrice) && combination.Sum(r => r.PricePerNight) <= (int)vm.maxPrice);
                IEnumerable<List<Room>> query2;
                switch (vm.Sorting)
                {
                    case sortBy.LowPrice:
                        query2 = query.OrderBy(combination => combination.Sum(r => r.PricePerNight));
                        break;
                    case sortBy.HighPrice:
                        query2 = query.OrderByDescending(combination => combination.Sum(r => r.PricePerNight));
                        break;
                    case sortBy.OurTopPicks:
                        query2 = query;
                        break;
                    default:
                        query2 = query;
                        break;

                }

                //var serializedCombinations = JsonConvert.SerializeObject(query2);


                //ViewBag.SerializedCombinations = serializedCombinations;
                if (query2.Count() == 0)
                {
                    ViewBag.HasCombinations = false;
                }
                else
                {
                    ViewBag.HasCombinations = true;
                }
                return View(query2);

            }
        }

        // GET: ReservationController/Details/5
        [Authorize]
        //public async Task<ActionResult> Reserve(string RoomsCombination
        [HttpPost]
        public async Task<ActionResult> Reserve(string RoomsCombination)
        {
            if (RoomsCombination == "temp")
            {
                ModelState.AddModelError("", "reservation failed, probably incorrect date");
                RoomsCombination = (string)TempData["FailedReserve"];

            }
            List<Room> rooms = JsonConvert.DeserializeObject<List<Room>>(RoomsCombination);

            SearchViewModel myvm = JsonConvert.DeserializeObject<SearchViewModel>(TempData.Peek("myviewmodel").ToString());

            ViewBag.MyHotel = rooms.FirstOrDefault().Hotel;

            DraftReservation draft = new()
            {

            };

            if (myvm.CheckInDate != default)
            {
                draft.CheckInDate = myvm.CheckInDate;
            }
            if (myvm.CheckOutDate != default)
            {
                draft.CheckOutDate = myvm.CheckOutDate;
            }

            await reservationservice.savedraft(draft, User.FindFirstValue(ClaimTypes.NameIdentifier), rooms);
            string promoCode;
            if (draft.UsedPromoCode == null)
            {
                promoCode = "";
            }
            else
            {
                promoCode = reservationservice.GETCODEText(draft.UsedPromoCode.Id);

            }
            ReservationViewModel vm = new()
            {
                CheckInDate = draft.CheckInDate,
                CheckOutDate = draft.CheckOutDate,
                amenity = draft.amenity == null ? 0 : draft.amenity,
                Plan = draft.mealPlan == null ? 0 : draft.mealPlan,
                PromoCode = promoCode,
                rooms = RoomsCombination

            };

            return View(vm);
        }
        [Authorize]
        public async Task<ActionResult> MyReservations()

        {
            var id = User.FindFirstValue(ClaimTypes.Email);

            var user = await usermanager.FindByEmailAsync(id);
            ViewBag.ProfilePicture = user.ProfilePicture;
            ViewBag.FirstName = user.FirstName;
            ViewBag.LasttName = user.LastName;
            ViewBag.Email = user.Email;

            var reservations = await reservationservice.MyReservations(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(reservations);
        }

        public async Task<ActionResult> CancelReservation(int id)
        {
            reservationservice.CancelReservation(id);
            //reservation.reservationStatus = ReservationStatus.Canceled;
            //reservation.CancellationDate = DateTime.Now;
            return RedirectToAction(nameof(MyReservations));

        }

        public IActionResult ContinuePayment(int id)
        {
            var reservation = reservationservice.getById(id);
            if (reservation is null)
            {
                return RedirectToAction("index", "Home");
            }
            reservationservice.Delete(User.FindFirstValue(ClaimTypes.NameIdentifier));
            InvoiceConfirmationViewModel invoice = new()
            {
                Tax = 10,
                Reservation = reservation,
                ReservationId = reservation.Id

            };
            TempData["ReservationId"] = reservation.Id;
            return View(nameof(InvoiceConfirmation), invoice);
        }





        [HttpGet]
        [Authorize]
        public async Task<ActionResult> ViewMyDraft()
        {
            //var user=await usermanager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var id = User.FindFirstValue(ClaimTypes.Email);

            var user = await usermanager.FindByEmailAsync(id);
            ViewBag.ProfilePicture = user.ProfilePicture;
            ViewBag.FirstName = user.FirstName;
            ViewBag.LasttName = user.LastName;
            ViewBag.Email = user.Email;
            var draft = reservationservice.getUserReservation(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ReservationViewModel model = new();

            if (draft is not null)
            {
                var hotel = draft.Reserved.FirstOrDefault().Reserved.Hotel; ;
                ViewBag.MyHotel = hotel;

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                };


                model.rooms = JsonSerializer.Serialize(draft.Reserved.Select(r => r.Reserved), options);
                model.CheckInDate = draft.CheckInDate;
                model.CheckOutDate = draft.CheckOutDate;
                model.amenity = draft.amenity == null ? 0 : draft.amenity;
                model.Plan = draft.mealPlan == null ? 0 : draft.mealPlan;
                string text;
                if (draft.UsedPromoCodeId != null)
                {
                    text = reservationservice.GETCODEText((int)draft.UsedPromoCodeId);
                }
                else
                {
                    text = "";

                }
                model.PromoCode = text == "" ? "" : text;
            }
            else
            {
                //return RedirectToAction("Index", "Home");
                ViewBag.DraftMessage = "You have no draft reservations.";
                return View("Reserve", model); // Return the same view, but with a message

            }

            return View("Reserve", model);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ConfirmReserve(ReservationViewModel vm)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            };
            List<Room> rooms = JsonConvert.DeserializeObject<List<Room>>(vm.rooms);
            ViewBag.MyHotel = rooms.FirstOrDefault().Hotel;

            if (!ModelState.IsValid)
            {


                return RedirectToAction("Index", "Home");
            }
            if (vm.CheckInDate is null || vm.CheckOutDate is null)
            {

                return RedirectToAction("Index", "Home");
            }

            foreach (var room in rooms)
            {
                if (room.Reserved.Where(r => r.Reservation.reservationStatus != ReservationStatus.Canceled).Any(r => r.Reservation.CheckInDate >= vm.CheckInDate && r.Reservation.CheckInDate <= vm.CheckOutDate)
                 || room.Reserved.Where(r => r.Reservation.reservationStatus != ReservationStatus.Canceled).Any(r => r.Reservation.CheckInDate <= vm.CheckInDate && r.Reservation.CheckOutDate >= vm.CheckInDate))
                {
                    ModelState.AddModelError("", "Room will be unavailable in those timelines");

                    return RedirectToAction("Index", "Home");
                }
            }

            var user = await usermanager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var reservation = reservationservice.SaveReservation(vm, rooms, User.FindFirstValue(ClaimTypes.NameIdentifier));
            reservationservice.Delete(User.FindFirstValue(ClaimTypes.NameIdentifier));
            InvoiceConfirmationViewModel invoice = new()
            {
                Tax = 10,
                Reservation = reservation
            };
            TempData["ReservationId"] = reservation.Id;
            return View(nameof(InvoiceConfirmation), invoice);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> SaveinDraft(ReservationViewModel vm)
        {
            List<Room> rooms = JsonConvert.DeserializeObject<List<Room>>(vm.rooms);
            ViewBag.MyHotel = rooms.FirstOrDefault().Hotel;
            DraftReservation draft = new()
            {
                //Reserved = rooms
            };

            if (vm.CheckInDate != default)
            {
                draft.CheckInDate = vm.CheckInDate;
            }
            if (vm.CheckOutDate != default)
            {
                draft.CheckOutDate = vm.CheckOutDate;
            }
            string promoCode;
            if (draft.UsedPromoCode == null)
            {
                promoCode = "";
            }
            else
            {
                promoCode = reservationservice.GETCODEText(draft.UsedPromoCode.Id);

            }
            draft.amenity = vm.amenity;
            draft.mealPlan = vm.Plan;

            draft.UsedPromoCodeId = reservationservice.GETCODE(vm.PromoCode)?.Id;
            await reservationservice.savedraft(draft, User.FindFirstValue(ClaimTypes.NameIdentifier), rooms);
            return View("Reserve", vm);
        }

        // GET: ReservationController/Create
        public ActionResult InvoiceConfirmation()
        {
            return View();
        }

    }
}
