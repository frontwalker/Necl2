using System;
using System.Collections.Generic;

namespace Logistikcenter.Domain
{
    public class TransportRequest : TransportBase
    {
        private TransportRequestStatus status;

        

        protected TransportRequest()
        { }

        private void Init(Customer customer, IList<TransportUnit> transportUnits)
        {
            this.customer = customer;
            this.createdTime = DateTime.Now; //Steria.Utilities.TimeHelper.Now;
            this.status = TransportRequestStatus.New;
            this.transportUnits = transportUnits;
        }

        public TransportRequest(Customer customer, IList<TransportUnit> transportUnits)
        {
            this.Init(customer, transportUnits);
        }

        public TransportRequest(Customer customer, TransportUnit transportUnit)
        {
            List<TransportUnit> units = new List<TransportUnit>();

            units.Add(transportUnit);
            this.Init(customer, units);
        }

        public virtual TransportRequestStatus Status
        {
            get { return status; }
            set { status = value; }

        }

        public virtual DateTime MinPickupTime
        {
            get
            {
                DateTime result = DateTime.MaxValue;

                foreach (TransportUnit transportUnit in this.TransportUnits)
                {
                    if (transportUnit.MinPickupTime < result)
                    {
                        result = transportUnit.MinPickupTime;
                    }
                }

                return result;
            }
        }

        public virtual DateTime MaxDeliveryTime
        {
            get
            {
                DateTime result = DateTime.MinValue;

                foreach (TransportUnit transportUnit in this.TransportUnits)
                {
                    if (transportUnit.MaxDeliveryTime > result)
                    {
                        result = transportUnit.MaxDeliveryTime;
                    }
                }

                return result;
            }
        }

        public virtual double TotalCost(int transportProposalIndex)
        {
            double cost = 0.0;

            foreach (TransportUnit unit in this.TransportUnits)
            {
                cost += unit.TotalCost(transportProposalIndex);
            }

            return cost;
        }



        public virtual TransportOrder Accept(int transportProposalIndex)
        {
            //decrease available capacity on used legs
            foreach (TransportUnit transportUnit in this.TransportUnits)
            {
                //select route
                transportUnit.SelectRoute(transportProposalIndex);


                //reserve capacity 
                transportUnit.SelectedRoute.ReserveCapacity();
            }

            //set status to accepted
            this.Status = TransportRequestStatus.Accepted;

            //create the transport order
            return new TransportOrder(this.Customer, this.TransportUnits);
        }


    }
}
