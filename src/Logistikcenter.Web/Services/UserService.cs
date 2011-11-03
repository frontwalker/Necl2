using System.Web.Security;

namespace Logistikcenter.Web.Services
{
    public interface IUserService
    {
        void AddShippingAgent(string username, string password);
        void AddShippingAgent(string username, string password, string email);
        void Remove(string username);
    }

    public class UserService : IUserService
    {
        public void AddShippingAgent(string username, string password)
        {
            Membership.CreateUser(username, password);
            Roles.AddUserToRole(username, "ShippingAgent");
        }

        public void AddShippingAgent(string username, string password, string email) 
        {
            Membership.CreateUser(username, password, email);
            Roles.AddUserToRole(username, "ShippingAgent");
        }

        public void Remove(string username)
        {
            if (Membership.GetUser(username) != null)                
                Membership.DeleteUser(username);
        }
    }
}