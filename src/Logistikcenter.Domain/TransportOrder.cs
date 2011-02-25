using System;
using System.Collections.Generic;

namespace Logistikcenter.Domain
{
    public class TransportOrder : TransportBase
    {


        private TransportOrderStatus status;



        public virtual TransportOrderStatus Status
        {
            get { return status; }
            set { status = value; }
        }



        protected TransportOrder()
        { }

        public TransportOrder(Customer customer, IList<TransportUnit> transportUnits)
        {
            this.customer = customer;

            foreach (TransportUnit unit in transportUnits)
            {
                this.TransportUnits.Add(unit);
            }

            this.createdTime = DateTime.Now;//Steria.Utilities.TimeHelper.Now;
            this.status = TransportOrderStatus.New;
        }
    }
}
