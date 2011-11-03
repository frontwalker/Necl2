using System.Collections.Generic;
using Logistikcenter.Domain;

namespace Logistikcenter.Web.Models
{
    public class BookingModel
    {
        
    }

    public class NewBookingModel
    {
        public NewBookingModel()
        {
            Legs = new List<Leg>(); //new List<long>(); 
        }

        public IList<Leg> Legs { get; private set; }
        public double Volume { get; set; }
        public double Weight { get; set; }

        public void AddLegs(IEnumerable<Leg> legs)
        {
            foreach (var leg in legs)
            {
                Legs.Add(leg);
            }
        }

        public int NumberOfPackages { get; set; }
    }
}