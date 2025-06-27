using System;
using MVCBookingFinal_YARAB_.Models;

namespace MVCBookingFinal_YARAB_.ViewModels
{
    public class MyReviewReservationVM
    {
        public int ReservationId { get; set; }
        public string HotelName { get; set; }
        public string CityName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public Review? UserReview { get; set; } // null if not reviewed
        public int HotelId { get; set; }
    }
} 