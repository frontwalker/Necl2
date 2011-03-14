using System.ComponentModel.DataAnnotations;

namespace Logistikcenter.Web.Areas.Admin.Models
{
    public class ShippingAgentModel
    {        
        public long Id { get; set; }
        [Required]
        [Display(Name = "Företag")]
        public string CompanyName { get; set; }
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Användarnamn")]
        public string Username { get; set; }
    }
}