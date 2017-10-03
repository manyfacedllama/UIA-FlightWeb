using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;

namespace UIA_Web.Models
{
    public class IndexViewModel
    {
    
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }
    public class BookingViewModel
    {
        public List<FlightRecord> records { get; set; }

    }
    public class SearchFlightModel
    {
        [Display(Name = "Destination City")]
        public string Destination { get; set; }
        [Display(Name = "Destination Airport")]
        public string DestinationAirport { get; set; }
        [Display(Name = "Origin City")]
        public string Origin { get; set; }
        public string OriginAirport { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Departure Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }
        [Display(Name = "Seat Class")]
        public string Seat_Class { get; set; }
        [Display(Name = "Flight Duration")]
        public string Duration { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Departure Time")]
        public DateTime Time { get; set; }
        [Display(Name = "Price")]
        public int Price { get; set; }
        [Required]
        public bool hasReturn { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Return Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateReturn { get; set; }
        [Display(Name = "Return Trip Flight Duration")]
        public string DurationReturn { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Return Trip Departure Time")]
        public DateTime TimeReturn { get; set; }
        [Display(Name = "Return Trip Price")]
        public int PriceReturn { get; set; }
        [Display(Name = "Return Destination City")]
        public string DestinationReturn { get; set; }
        [Display(Name = "Return Trip Destination Airpot")]
        public string DestinationAirportReturn { get; set; }
        [Display(Name = "Return Trip Origin Airport")]
        public string OriginReturn { get; set; }
        public string OriginAirportReturn { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Return Trip Arrival Time")]
        public DateTime ArrivalTimeReturn { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Arrival Time")]
        public DateTime ArrivalTime { get; set; }
        [Display(Name = "Total Price")]
        public int TotalPrice { get; set; }
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Passort Number")]

        public string PassportNumber { get; set; }
        [Display(Name = "Full Name")]

        public string Name { get; set; }
        [Display(Name = "Phone Number")]

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CCV { get; set; }
        [Display(Name = "Cardholder Name")]
        public string CardHolderName { get; set; }
        [Display(Name = "Credit Card Number")]
        public string CardNumber { get; set; }
        [Display(Name = "Expiry Date")]
        public string ExpiryDate { get; set; }
        [Display(Name = "Card Type")]
        public string CardType { get; set; }
        [Display(Name = "Seat Number")]
        public string SeatNo { get; set; }
        public string UserId { get; set; }
    }
    
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}