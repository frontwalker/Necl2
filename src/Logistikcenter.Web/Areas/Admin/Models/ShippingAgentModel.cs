using System.ComponentModel.DataAnnotations;

namespace Logistikcenter.Web.Areas.Admin.Models
{
    public class ShippingAgentModel
    {        
        public long Id { get; set; }
        [Required]       
        public string CompanyName { get; set; }        
        public string FirstName { get; set; }        
        public string LastName { get; set; }
        [Required]        
        public string Username { get; set; }
        [Required]        
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
    }
}