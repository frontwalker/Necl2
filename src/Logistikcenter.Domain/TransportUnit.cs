using System;
using System.Collections.Generic;

namespace Logistikcenter.Domain
{
    public class TransportUnit
    {
        protected Int64 id;

        public virtual Int64 Id
        {
            get { return id; }
        }

        protected Destination origin;
        protected Destination destination;

        public virtual Destination Destination
        {
            get { return destination; }
        }
        protected DateTime minPickupTime;

        public virtual DateTime MinPickupTime
        {
            get { return minPickupTime; }
            set { minPickupTime = value; }
        }
        protected DateTime maxDeliveryTime;

        public virtual DateTime MaxDeliveryTime
        {
            get { return maxDeliveryTime; }
            set { maxDeliveryTime = value; }
        }


        public virtual Destination Origin
        {
            get { return origin; }
        }


        protected CargoDefinition cargo;

        public virtual CargoDefinition Cargo
        {
            get { return cargo; }
        }



        protected TransportUnit()
        { }

        private IList<Route> proposedRoutes;

        public virtual IList<Route> ProposedRoutes
        {
            get
            {
                if (proposedRoutes == null)
                {
                    proposedRoutes = new List<Route>();
                }
                return proposedRoutes;

            }
        }


        private Route selectedRoute;

        public virtual Route SelectedRoute
        {
            get
            {
                if (selectedRoute == null)
                {
                    throw new NoRouteSelectedException();
                }
                return selectedRoute;

            }
        }


        public virtual double TotalCost(int transportProposalIndex)
        {
            double cost = 0.0;
            foreach (Leg l in this.ProposedRoutes[transportProposalIndex].Legs)
            {
                cost += l.Cost;
            }

            return cost;
        }


        private void Init(Destination origin, Destination destination, DateTime minPickupTime, DateTime maxDeliveryTime, CargoDefinition cargo)
        {
            this.origin = origin;
            this.destination = destination;
            this.minPickupTime = minPickupTime;
            this.maxDeliveryTime = maxDeliveryTime;
            this.cargo = cargo;

        }

        public virtual void SelectRoute(int routeIndex)
        {
            if (routeIndex >= this.ProposedRoutes.Count)
            {
                throw new IndexOutOfRangeException();
            }

            this.selectedRoute = this.ProposedRoutes[routeIndex];
        }



        public TransportUnit(Destination origin, Destination destination, DateTime minPickupTime, DateTime maxDeliveryTime, CargoDefinition cargo)
        {
            this.Init(origin, destination, minPickupTime, maxDeliveryTime, cargo);
        }
        public TransportUnit(Destination origin, Destination destination, DateTime minPickupTime, DateTime maxDeliveryTime, CargoDefinition cargo, Route selectedRoute)
        {
            this.Init(origin, destination, minPickupTime, maxDeliveryTime, cargo);
            this.selectedRoute = selectedRoute;
        }

        public TransportUnit(Destination origin, Destination destination, DateTime minPickupTime, DateTime maxDeliveryTime, CargoDefinition cargo, IList<Route> proposedRoutes)
        {
            this.Init(origin, destination, minPickupTime, maxDeliveryTime, cargo);

            this.proposedRoutes = proposedRoutes;
        }








    }

    public class NoRouteSelectedException : ApplicationException { }

}
