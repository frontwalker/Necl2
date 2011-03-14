using System.ComponentModel.DataAnnotations;

namespace Logistikcenter.Web.Areas.Admin.Models
{
    public class ShippingAgentModel
    {        
        public long Id { get; set; }
        [Required]
        [Display(Name = "F�retag")]
        public string CompanyName { get; set; }
        [Display(Name = "F�rnamn")]
        public string FirstName { get; set; }
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Anv�ndarnamn")]
        public string Username { get; set; }
    }
}