using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logistikcenter.Domain
{
    class Supplier
    {

         private string firstName;

        public virtual string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        private string lastName;

        public virtual string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private long id;

        public virtual long Id
        {
            get { return id; }
            set { id = value; }
        }

        private CustomerType customerType;

        public virtual CustomerType CustomerType
        {
            get { return customerType; }
            set { customerType = value; }
        }
 

        private string companyName;

        public virtual string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }
        private string username;

        public virtual string Username
        {
            get { return username; }
            set { username = value; }
        }
        

        protected Supplier()
        { 
        
        }

        public Supplier(long id)
        {
            this.id = id;
        }

        public Supplier(string companyName,string userName )
        {
            this.companyName = companyName;
            this.username = userName;
            this.customerType = CustomerType.Company;   
        }

        public Supplier(string companyName, string userName, CustomerType customerType, string firstName, string lastName)
        {
            this.CompanyName = companyName;
            this.Username = userName;
            this.customerType = customerType;
            this.firstName = firstName;
            this.lastName = lastName;
        }

    }
}
