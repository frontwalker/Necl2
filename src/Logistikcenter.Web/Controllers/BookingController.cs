using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logistikcenter.Web.Models;
using Logistikcenter.Domain;
using Logistikcenter.Services;

namespace Logistikcenter.Web.Controllers
{
    public class BookingController : Controller
    {

        private IRepository _session;
        //
        // GET: /Booking/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(string info, string transportunit)
        {
            var content = System.Convert.FromBase64String(info);
            var booking = System.Text.Encoding.Default.GetString(content);
            var newbooking = Newtonsoft.Json.JsonConvert.DeserializeObject<NewBookingModel>(booking);

            var myTransportunit = Newtonsoft.Json.JsonConvert.DeserializeObject<TransportUnit>(System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(transportunit)));

            // tanken är att här visa vad som ska bokas och sen kanske fylla i mer uppgifter, kontrollera att användaren är en kund, och sen boka och räkna ner kapacitet för "leg" som ingår i rutten.

            ViewBag.route = "init ";
            
           // var session = _session.Query<Leg>.

            foreach (var leg in newbooking.Legs) {
                ViewBag.route += leg.Origin + " - " + leg.Destination;
                if (leg.UsedCapacity + newbooking.Volume <= leg.TotalCapacity)
                {
                    leg.UsedCapacity += newbooking.Volume;
                    
                }
                else throw new System.Exception();

                _session.Update(newbooking);
            }

            
            


            return View();
        }
    }
}
