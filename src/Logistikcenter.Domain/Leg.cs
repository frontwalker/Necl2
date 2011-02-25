using System;

namespace Logistikcenter.Domain
{
    public class Leg
    {
        private Destination origin;
        private Destination destination;
        private long id;
        private ShippingAgent carrier;
        private double totalCapacity;
        private double usedCapacity;
        private double cost;
        private DateTime departureTime;
        private System.DateTime arrivalTime;
        private CarrierType carrierType;
        
        private string uniqueIdentifier;

        protected int version;


        public virtual string UniqueIdentifier
        {
            get { return uniqueIdentifier; }
        }


        public virtual CarrierType CarrierType
        {
            get { return carrierType; }
        }

        public virtual string CarrierTypeDisplayName
        {
            get
            {
                switch (this.CarrierType)
                { 
                    case CarrierType.Air: return "Flyg";
                    case CarrierType.Boat: return "Båt";
                    case CarrierType.Rail: return "Tåg";
                    case CarrierType.Truck: return "Bil";
                }
                return "Unknown carrier type";
            }
        }

        public virtual System.DateTime ArrivalTime
        {
            get { return arrivalTime; }
            set { arrivalTime = value; }
        }

        public virtual DateTime DepartureTime
        {
            get { return departureTime; }
        }

        public virtual double Cost
        {
            get { return cost; }
        }

        public virtual double TotalCapacity
        {
            get { return totalCapacity; }
        }

        public virtual double UsedCapacity
        {
            get { return usedCapacity; }
            set { usedCapacity = value; }
        }

        public virtual ShippingAgent Carrier
        {
            get { return carrier; }
        }

        public virtual long Id
        {
            get { return id; }
        }


        public virtual Destination Destination
        {
            get { return destination; }
        }


        public virtual TimeSpan Duration
        {
            get { return (this.ArrivalTime - this.DepartureTime); }
        }

        public virtual Destination Origin
        {
            get { return origin; }
        }





        protected Leg() { }

        public Leg(string uniqueIdentifier,ShippingAgent carrier, CarrierType carrierType,Destination origin,Destination destination ,DateTime departureTime, DateTime arrivalTime, double cost, double totalCapacity)
        {
            this.uniqueIdentifier = uniqueIdentifier;
            this.carrier = carrier;
            this.carrierType = carrierType;
            this.origin = origin;
            this.destination = destination;
            this.departureTime = departureTime;
            this.arrivalTime = arrivalTime;
            this.cost = cost;
            this.totalCapacity = totalCapacity;
            this.UsedCapacity = 0;
           
        }
    }
}
