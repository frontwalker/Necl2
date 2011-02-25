using System;
using System.Collections.Generic;

namespace Logistikcenter.Domain
{
    public class Route
    {
        private IList<Leg> legs;
        private double shortage;
        private Int64 id;
        protected TransportUnit transportUnit;

        public virtual string Information
        {
            get
            {
                string infoString = "";
                if (this.Shortage > 0)
                {
                    double availableVolume = this.TransportUnit.Cargo.Volume - this.Shortage;
                    infoString += "OBS! Endast " + availableVolume.ToString() + " ton";
                }
                return infoString; 



            }
        }
        public virtual DateTime DepartureTime
        {
            get 
            { 
                return legs[0].DepartureTime; 
            }
        }
       
        public virtual DateTime ArrivalTime
        {
            get 
            { 
                return legs[legs.Count  -1].ArrivalTime; 
            }
           
        }

        public virtual Destination Origin
        {
            get
            {
                return legs[0].Origin;
            }
        }

        public virtual Destination Destination
        {
            get
            {
                return legs[legs.Count - 1].Destination;
            }

        }


 
        public virtual TimeSpan Duration
        {
            get 
            { 
                return this.ArrivalTime - this.DepartureTime; 
            }
            
        }

        public virtual double Cost
        {
            get 
            {
                double cost = 0;
                foreach (Leg leg in legs)
                {
                    cost += leg.Cost;
                }
                return cost * (this.TransportUnit.Cargo.Volume - this.Shortage) ; 
            
            }
            
        }

        public virtual Int64 Id
        {
            get { return id; }
            set { id = value; }
        }

        public virtual double Shortage
        {
            get { return shortage; }
            set { shortage = value; }
        }

        public virtual IList<Leg> Legs
        {
            get { return legs; }
            set { legs = value; }
        }

        public virtual TransportUnit TransportUnit
        {
            get
            {
                return transportUnit;
            }
        }

        
        protected Route()
        {
            
        }
        public Route(TransportUnit transportUnit, double shortage, List<Leg> legs)
        {
            this.transportUnit = transportUnit;
            this.shortage = shortage;
         
            //make sure that the legs are sorted in departure time order
            legs.Sort(delegate(Leg x, Leg y) { return x.DepartureTime.CompareTo(y.DepartureTime); });
            this.legs = legs; 
        }

        public virtual void ReserveCapacity()
        {
            CargoDefinition cargo = this.TransportUnit.Cargo;

            foreach (Leg leg in this.Legs)
            {
                if(leg.UsedCapacity + cargo.Volume > leg.TotalCapacity)
                {
                    throw new NotEnoughAvailableCapacityException();
                }

                leg.UsedCapacity += cargo.Volume;
            }
        }
    }

    public class NotEnoughAvailableCapacityException : ApplicationException { }
}
