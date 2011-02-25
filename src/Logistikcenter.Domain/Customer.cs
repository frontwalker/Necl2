using System;

namespace Logistikcenter.Domain
{
    public class Customer
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
        private string password;

        public virtual string Password
        {
            get { return password; }
            set { password = value; }
        }

        protected Customer()
        { 
        
        }

        public Customer(long id)
        {
            this.id = id;
        }

        public Customer(string companyName,string userName,string password)
        {
            this.companyName = companyName;
            this.username = userName;
            this.password = password;
            this.customerType = CustomerType.Company;
        }

        public virtual void Authenticate(string password)
        {
            //invalid password?
            if (this.Password != password)
            {
                throw new BadPasswordException();
            }

        }





    }
    public class BadPasswordException : ApplicationException
    { }
}
