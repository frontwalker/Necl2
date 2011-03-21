using System;
using System.ComponentModel.DataAnnotations;

namespace Logistikcenter.Web.Models
{
    public class TransportModel
    {
        [Required]
        public string Origin { get; set; }
        [Required]
        public string Destination { get; set; }
        public int DateRestrictionType { get; set; }
        public int Packages { get; set; }
        public int PackageType { get; set; }
        public int SelectedVolumeType { get; set; }                
        public double Weight { get; set; }        
        public double Volume { get; set; }                
        public DateTime? Date { get; set; }        
        public int Time { get; set; }

        public DateTime MinPickupTime
        {
            get { return DateRestrictionType == 1 ?

                Date.HasValue ? new DateTime(Date.Value.Year, Date.Value.Month, Date.Value.Day, Time, 0, 0) 
                
                : DateTime.Now.AddHours(8)                                                
                : DateTime.Now.AddHours(8); }
        }

        public DateTime MaxDeliveryTime
        {
            get { return DateRestrictionType == 0 ? 
                
                Date.HasValue ? new DateTime(Date.Value.Year, Date.Value.Month, Date.Value.Day, Time, 0, 0) 
                
                : DateTime.Today.AddYears(1)
                : DateTime.Today.AddYears(1); }
        }
    }
}