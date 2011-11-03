using System.Collections.Generic;

namespace Logistikcenter.Domain
{
    public class ShippingAgent : Customer
    {
        private long customerId;

        public virtual long CustomerId {
            get { return customerId; }
            set { customerId = value; }
        }

        private string email;

        public virtual string Email { get { return email; } set { email = value; } }

        public ShippingAgent() : base(){}
     
    }
    /*
    public class ShippingAgent 
    {
        private string givenname;

        public virtual string Givenname { 
            get { return givenname; } 
            set { givenname = value; } 
        }

        private string surname;

        public virtual string Surname {
            get { return surname;}
            set { surname = value; }
        }

        private long id;

        public virtual long Id {
            get { return id; }
            set { id = value; }
        }

        private string email;

        public virtual string Email{
            get { return email; }
            set { email = value; }
        }

        private CustomerType customerType;

        public virtual CustomerType CustomerType {
            get { return customerType; }
            set { customerType = value; }
        }

        private string username;

        public virtual string Username {
            get { return username; }
            set { username = value; }
        }

        private string companyName;

        public virtual string CompanyName {
            get { return companyName; }
            set { companyName = value; }
        }

        public ShippingAgent() {
            this.customerType = CustomerType.Company;
        }

        public ShippingAgent( long id ) {
            this.id = id;
        }

        public ShippingAgent(string username, string companyname) {
            this.username = username;
            this.companyName = companyname;
            this.customerType = CustomerType.Company;
        }

        public ShippingAgent(string givenname, string surname, string username, string email, string companyname) {
            this.givenname = givenname;
            this.surname = surname;
            this.username = username;
            this.email = email;
            this.companyName = companyname;
            this.customerType = CustomerType.Company;
        }
        
    }*/
}
