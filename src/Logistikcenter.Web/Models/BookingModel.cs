using System.Collections.Generic;

namespace Logistikcenter.Web.Models
{
    public class BookingModel
    {
        
    }

    public class NewBookingModel
    {
        public NewBookingModel()
        {
            Legs = new List<long>();
        }

        public IList<long> Legs { get; private set; }
        public double Volume { get; set; }
        public double Weight { get; set; }

        public void AddLegs(IEnumerable<long> legs)
        {
            foreach (var leg in legs)
            {
                Legs.Add(leg);
            }
        }

        public int NumeberOfPackages { get; set; }
    }
}