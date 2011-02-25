using System.Linq;
using System.Web.Mvc;
using Logistikcenter.Domain;
using Logistikcenter.Services;
using Logistikcenter.Web.Models;

namespace Logistikcenter.Web.Controllers
{
    public class RouteController : Controller
    {
        private readonly IRepository _repository;
        private readonly ITransportOptimizationService _transportOptimizationService;

        public RouteController(IRepository repository, ITransportOptimizationService transportOptimizationService)
        {
            _repository = repository;
            _transportOptimizationService = transportOptimizationService;
        }

        public ActionResult Find(TransportModel transportModel)
        {
            var origin = _repository.Query<Destination>().Where(o => o.Name == transportModel.Origin).SingleOrDefault();
            var destination = _repository.Query<Destination>().Where(d => d.Name == transportModel.Destination).SingleOrDefault();
            
            var cargoDefinition = new CargoDefinition(transportModel.Weight);
            var transportUnit = new TransportUnit(origin, destination, transportModel.MinPickupTime, transportModel.MaxDeliveryTime, cargoDefinition);

            // borde inte kunden endast vara intressant när det ska bokas något?
            var customer = _repository.Query<Customer>().Where(c => c.CompanyName == "DHL").SingleOrDefault();

            var transportRequest = new TransportRequest(customer, transportUnit);

            _transportOptimizationService.LoadData(transportRequest.MinPickupTime, transportRequest.MaxDeliveryTime);
            _transportOptimizationService.MinimizeCost(transportRequest.TransportUnits, 3);

            var model = new RouteModel
                            {                           
                                Packages = transportModel.Packages,
                                PackageType = transportModel.PackageTypes.Where(pt => pt.Value == transportModel.PackageType.ToString()).Select(pt => pt.Text).Single(),
                                DeliveryInformation = transportModel.DateRestrictionTypes.Where(dr => dr.Value == transportModel.DateRestrictionType.ToString()).Select(dr => dr.Text).Single() + " " + transportModel.Date.Value.ToString("yyyy-MM-dd") + " " + transportModel.Time.ToString("00") + ":00",
                                Volume = transportModel.Volume.ToString(),
                                Origin = origin.Name,
                                Destination = destination.Name,
                                Routes = transportRequest.TransportUnits[0].ProposedRoutes
                            };

            return View(model);
        }
    }
}
