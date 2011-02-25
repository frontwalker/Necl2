using System;
using System.Collections.Generic;

namespace Logistikcenter.Domain
{
    public class TransportBase
    {
    
        protected int version;


        protected Int64 id;

        public virtual Int64 Id
        {
            get { return id; }
        }



    
        protected DateTime createdTime;

        public virtual DateTime CreatedTime
        {
            get { return createdTime; }
        }

        protected Customer customer;
        public virtual Customer Customer
        {
            get { return customer; }
        }


        protected IList<TransportUnit> transportUnits;
        public virtual IList<TransportUnit> TransportUnits
        {
            get 
            {
                if (transportUnits == null)
                {
                    transportUnits = new List<TransportUnit>();
                }
                return transportUnits; 
            
            }
        }



    }
}
