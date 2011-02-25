using System.Collections.Generic;

namespace Logistikcenter.Web.Models
{
    public class RouteModel
    {
        public int Packages { get; set; }
        public string PackageType { get; set; }
        public string Volume { get; set; }
        public string DeliveryInformation { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public IEnumerable<Logistikcenter.Domain.Route> Routes { get; set; } 
    }
}