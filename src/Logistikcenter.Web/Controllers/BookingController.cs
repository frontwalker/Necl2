using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logistikcenter.Web.Models;

namespace Logistikcenter.Web.Controllers
{
    public class BookingController : Controller
    {
        //
        // GET: /Booking/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(string info)
        {
            var content = System.Convert.FromBase64String(info);
            var booking = System.Text.Encoding.Default.GetString(content);
            var newbooking = Newtonsoft.Json.JsonConvert.DeserializeObject<NewBookingModel>(booking);

            // tanken är att här visa vad som ska bokas och sen kasnke fylla i mer uppgifter, kontrollera att användaren är en kund, och sen boka och räkna ner kapacitet för "leg" som ingår i rutten.

            return View();
        }
    }
}
