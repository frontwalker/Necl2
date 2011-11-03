using System.Web.Mvc;
using System.Web.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logistikcenter.Web.Services;
using Logistikcenter.Domain;
using Logistikcenter.Web.Areas.ShippingAgentContent.Models;


namespace Logistikcenter.Web.Areas.ShippingAgentContent.Controllers
{
    [Authorize(Roles = "ShippingAgent")]    
    public class LegController : Controller
    {
        private readonly IRepository _repository;
        private readonly IUserService _userService;

        public LegController(IRepository repository, IUserService userService) {
            _repository = repository;
            _userService = userService;
        }

        public ActionResult Index() {
            string name = Membership.GetUser().UserName;
            if (name == null)
                name = "shippingangent";

            var agents = _repository.Query<ShippingAgent>().Where(s => s.Username == name);
            var legs = _repository.Query<Leg>()
                                    .Where(s => s.Carrier.Username == name)
                                    .Select(s => new LegModel { id = s.Id,
                                                                origin = s.Origin,
                                                                destination = s.Destination,
                                                                departureTime = s.DepartureTime,
                                                                arrivalTime = s.ArrivalTime,
                                                                carrier = s.Carrier,
                                                                carrierType = s.CarrierType,
                                                                cost = s.Cost,
                                                                totalCapacity = s.TotalCapacity,
                                                                usedCapacity = s.UsedCapacity,
                                                                uniqueIdentifier = s.UniqueIdentifier
                                                               }
                                           );
                                    
            return View(legs);
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        public ActionResult Create(LegModel legModel) {
            try {
                if (!ModelState.IsValid) {
                    return View(legModel);
                }

                string name = Membership.GetUser().UserName;
                var agent = _repository.Query<ShippingAgent>().Where(s => s.Username == name).Single();

               // var theOrigin = _repository.Query<Destination>().Where(s => s.Name == legModel.origin.Name).Select(s => new Destination { Id = s.Id, Name = s.Name }).Single();
               // var theDestination = _repository.Query<Destination>().Where(s => s.Name == legModel.destination.Name).Select(s => new Destination { Id = s.Id, Name = s.Name }).Single();

                var theOrigin = new Destination("Ankeborg");
                var theDestination = new Destination("Gåseborg");
                System.DateTime dtime = System.DateTime.Now;
                System.DateTime atime = System.DateTime.Now;
                atime.AddHours(5);

                var test = _repository.Query<Destination>().Where(s => s.Name == theOrigin.Name).AsEnumerable();
                if (test.AsEnumerable().Count() == 0)
                {
                    try
                    {
                        _repository.Save(theOrigin);
                    }
                    catch { }
                }
                else {
                    theOrigin = test.First();
                }

                test = _repository.Query<Destination>().Where(s => s.Name == theDestination.Name).AsEnumerable();
                if (test.AsEnumerable().Count() == 0)
                {
                    try
                    {
                        _repository.Save(theDestination);
                    }
                    catch { }
                }
                else {
                    theDestination = test.First();
                }

                Leg leg = new Leg(  "ABC123",
                                    agent,
                                    CarrierType.Truck,          //Hard coded for now
                                    theOrigin,                  //should be drop downs
                                    theDestination,
                                    dtime,     //should be date pickers
                                    atime,
                                    500,
                                    legModel.totalCapacity);
                if (leg == null)
                    throw new System.Exception();

                try
                {
                    _repository.Save(leg);
                }
                catch (System.Exception e){
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch (System.Exception e)
            {
                return View();
            }
        }


        public ActionResult Edit(int d) {
            var leg = _repository.Query<Leg>().Where(s => s.Id == d).Single();
            var legModel = new LegModel()
            {
                arrivalTime = leg.ArrivalTime,
                carrier = leg.Carrier,
                carrierType = leg.CarrierType,
                cost = leg.Cost,
                departureTime = leg.DepartureTime,
                destination = leg.Destination,
                id = leg.Id,
                origin = leg.Origin,
                totalCapacity = leg.TotalCapacity,
                uniqueIdentifier = leg.UniqueIdentifier,
                usedCapacity = leg.UsedCapacity
            };


            return View(legModel);
        }

        [HttpPost]
        public ActionResult Edit(int d, LegModel legModel) {

            try {
                var leg = _repository.Query<Leg>().Where(s => s.Id == legModel.id).Single();

                leg.Origin = legModel.origin;
                leg.TotalCapacity = legModel.totalCapacity;
                leg.ArrivalTime = legModel.arrivalTime;
                leg.Carrier = legModel.carrier;
                leg.Cost = legModel.cost;
                leg.CarrierType = legModel.carrierType;
                leg.DepartureTime = legModel.departureTime;
                leg.Destination = legModel.destination;
                leg.UniqueIdentifier = legModel.uniqueIdentifier;
                leg.DepartureTime = legModel.departureTime;



                _repository.Update(leg);

                return RedirectToAction("Index");
            }
            catch { 
                return View(); 
            }
        }

    }
}