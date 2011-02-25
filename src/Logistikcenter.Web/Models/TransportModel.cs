using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Logistikcenter.Web.Models
{
    public class TransportModel
    {
        [Required]
        [Display(Name = "Från:")]
        public string Origin { get; set; }
        [Required]
        [Display(Name = "Till:")]
        public string Destination { get; set; }
        [Display(Name = "Leverans:")]
        public int DateRestrictionType { get; set; }
        [Display(Name = "Antal kollin:")]
        public int Packages { get; set; }
        [Display(Name = "Kollityp:")]
        public int PackageType { get; set; }
        [Display(Name = "Vikt eller volym:")]
        public int SelectedVolumeType { get; set; }
        [Display(Name = "Vikt:")]
        public double Weight { get; set; }
        [Display(Name = "Volym:")]
        public double Volume { get; set; }        
        [Display(Name="Datum:")]        
        public DateTime? Date { get; set; }
        [Display(Name = "Tid:")]
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

        public IEnumerable<SelectListItem> PackageTypes
        {
            get
            {
                return new List<SelectListItem>
                           {
                               new SelectListItem {Text = "Paket", Value = "0", Selected = true},
                               new SelectListItem {Text = "Kolli", Value = "1", Selected = false},
                               new SelectListItem {Text = "Pall", Value = "2", Selected = false}
                           };
            }
        }

        public IEnumerable<SelectListItem> DateRestrictionTypes
        {
            get
            {
                return new List<SelectListItem>
                           {
                               new SelectListItem {Text = "Godset skall vara framme senast", Value = "0", Selected = true},
                               new SelectListItem {Text = "Godset skall skickas efter", Value = "1", Selected = false},                               
                           };
            }
        }

        public IEnumerable<SelectListItem> Hours
        {
            get
            {
                for (int i=0; i < 24; i++)
                {
                    yield return new SelectListItem {Text = i.ToString("00") + ":00", Value = i.ToString()};
                }
            }
        }

        public string[] SelectedVolumeTypes
        {
            get { return new[] { "Volym m3", "Flakmeter", "Pallplats" }; }
        }
    }
}