using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using UIA_Web.Models;
using System.Collections.Generic;

namespace UIA_Web.Controllers
{

    [Authorize]
    public class FlightController : Controller
    {
        public string GetIPAddress()
        {
            string ipAddress = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            ipAddress = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                return ipAddress;
            }
            return HttpContext.Request.UserHostAddress;
        }
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public FlightController()
        {
        }

        public FlightController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Flight
        public ActionResult Index()
        {
            return View();
        }
        [Route("/Flight/BookingConfirmed")]
        public ActionResult BookingConfirmed()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Book(SearchFlightModel model)
        {
            if (model.Origin == null)
            {
                return JavaScript("window.location = '" + Redirect("/Home/Index").Url + "'");

            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookFlight(SearchFlightModel model)
        {
            try
            {
                using (var ctx = new UIA_Entities())
                {
                    FlightRecord flight_record = new FlightRecord();
                    flight_record.Name = model.Name;
                    flight_record.PassportNumber = model.PassportNumber;
                    flight_record.SeatNo = model.SeatNo;
                    flight_record.Time = model.Time;
                    flight_record.Origin = model.Origin;
                    flight_record.Destination = model.Destination;
                    flight_record.SeatClass = model.Seat_Class;
                    flight_record.Price = model.Price;
                    flight_record.SeatNo = model.SeatNo;
                    flight_record.UserId = model.UserId;
                    flight_record.DateOfBirth = model.DateOfBirth;
                    flight_record.Email = model.Email;
                    flight_record.Id = Guid.NewGuid().ToString("N");
                    flight_record.FlightID = (from table in ctx.Flights where table.fromAirport == model.OriginAirport where table.toAirport == model.DestinationAirport select table.Id).Single();
                    ctx.FlightRecords.Add(flight_record);
                    ctx.SaveChanges();
                }
                using (var ctx = new UIA_Entities())
                {
                    if (model.hasReturn)
                    {
                        FlightRecord flight_record_return = new FlightRecord();
                        flight_record_return.Name = model.Name;
                        flight_record_return.PassportNumber = model.PassportNumber;
                        flight_record_return.SeatNo = model.SeatNo;
                        flight_record_return.Time = model.TimeReturn;
                        flight_record_return.Origin = model.OriginReturn;
                        flight_record_return.Destination = model.DestinationReturn;
                        flight_record_return.SeatClass = model.Seat_Class;
                        flight_record_return.Price = model.PriceReturn;
                        flight_record_return.SeatNo = model.SeatNo;
                        flight_record_return.UserId = model.UserId;
                        flight_record_return.DateOfBirth = model.DateOfBirth;
                        flight_record_return.Email = model.Email;
                        flight_record_return.Id = Guid.NewGuid().ToString("N");
                        flight_record_return.FlightID = (from table in ctx.Flights where table.fromAirport == model.OriginAirportReturn where table.toAirport == model.DestinationAirportReturn select table.Id).Single();
                        ctx.FlightRecords.Add(flight_record_return);
                        ctx.SaveChanges();

                    }
                }
                using (var ctx = new UIA_Entities())
                {
                    PaymentRecord payment = new PaymentRecord();
                    payment.UserId = model.UserId;
                    payment.CCV = model.CCV;
                    payment.PurchaseDate = DateTime.Now;
                    payment.CardHolderName = model.CardHolderName;
                    payment.CardType = model.CardType;
                    payment.CardNumber = model.CardNumber;
                    payment.TotalPrice = model.TotalPrice;
                    payment.PurchaseId = Guid.NewGuid().ToString("N");
                    payment.ExpiryDate = model.ExpiryDate;
                    ctx.PaymentRecords.Add(payment);
                    ctx.SaveChanges();

                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return RedirectToAction("BookingConfirmed");
        }
        public ActionResult SearchResult(SearchFlightModel model)
        {
            if (model.Origin == null)
            {
                return JavaScript("window.location = '" + Redirect("/Home/Index").Url + "'");

            }
            return View(model);
        }
        public static string currency;
        public IPLocation iplocation;
        [HttpPost]
        [ActionName("SearchResult")]
        [ValidateAntiForgeryToken]
        public ActionResult SearchResultPage(SearchFlightModel model)
        {
                if (currency == null)
                {
                    string client_ip = GetIPAddress();
                    if (currency == null)
                    {
                        iplocation = HomeController.GetIPLocation(client_ip);
                        if (currency == null)
                        {
                            currency = "$ ";
                        }
                    }
                }
                var euroCountries = new List<string>();
                euroCountries.AddRange(new String[] { "DE", "DK", "SE", "IT", "NL", "PL", "NO", "FI" });
                if (iplocation.CountryCode == "MY")
                {
                    System.Web.HttpContext.Current.Items["currency"] = "RM ";
                    currency = "RM ";

                }
                else if (euroCountries.Contains(iplocation.CountryCode))
                {
                    System.Web.HttpContext.Current.Items["currency"] = "€ ";
                    currency = "€ ";

                }
                else
                {
                    System.Web.HttpContext.Current.Items["currency"] = "$ ";
                    currency = "$ ";
                }
                using (var ctx = new UIA_Entities())
                {
                    ApplicationUser appUser = UserManager.FindById(User.Identity.GetUserId());
                    model.Name = appUser.Name;
                    model.Email = appUser.Email;
                    model.PhoneNumber = appUser.PhoneNumber;
                    model.UserId = appUser.Id;
                    model.PassportNumber = appUser.PassportNumber;

                    model.DateOfBirth = appUser.DateOfBirth;
                    model.Destination = (from table in ctx.Airports where table.Id == model.DestinationAirport select table.City).Single();
                    model.Origin = (from table in ctx.Airports where table.Id == model.OriginAirport select table.City).Single();
                    model.Price = (from table in ctx.Flights where table.fromAirport == model.OriginAirport where table.toAirport == model.DestinationAirport select table.Price).Single();
                    TimeSpan timeTaken = (from table in ctx.Flights where table.fromAirport == model.OriginAirport where table.toAirport == model.DestinationAirport select table.DepartureTime).Single();
                    model.Time = model.Date.Add(timeTaken);
                    int duration = (from table in ctx.Flights where table.fromAirport == model.OriginAirport where table.toAirport == model.DestinationAirport select table.Duration).Single();
                    model.ArrivalTime = model.Time.AddMinutes(duration);
                    model.Duration = duration.ToString() + " minutes";
                    Console.WriteLine(model.Date);
                    Console.WriteLine(model.Time);
                    model.TotalPrice = model.Price;
                    if (model.hasReturn)
                    {
                        model.hasReturn = true;
                        model.DestinationReturn = model.Origin;
                        model.OriginReturn = model.Destination;
                        model.DestinationAirportReturn = model.OriginAirport;
                        model.OriginAirportReturn = model.DestinationAirport;
                        model.PriceReturn = (from table in ctx.Flights where table.fromAirport == model.OriginAirportReturn where table.toAirport == model.DestinationAirportReturn select table.Price).Single();
                        TimeSpan timeTakenReturn = (from table in ctx.Flights where table.fromAirport == model.OriginAirportReturn where table.toAirport == model.DestinationAirportReturn select table.DepartureTime).Single();
                        model.TimeReturn = model.DateReturn.AddTicks(timeTakenReturn.Ticks);
                        int durationReturn = (from table in ctx.Flights where table.fromAirport == model.OriginAirportReturn where table.toAirport == model.DestinationAirportReturn select table.Duration).Single();
                        model.ArrivalTimeReturn = model.TimeReturn.AddMinutes(durationReturn);
                        model.DurationReturn = durationReturn.ToString() + " minutes";
                        model.TotalPrice += model.PriceReturn;

                    }
            }
            return View(model);
        }

    }
}