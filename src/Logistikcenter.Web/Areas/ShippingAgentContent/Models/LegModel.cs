using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logistikcenter.Domain;
using System.ComponentModel.DataAnnotations;

namespace Logistikcenter.Web.Areas.ShippingAgentContent.Models
{
    public class LegModel
    {
        [Required]
        public Destination origin;
        [Required]
        public Destination destination;
        public long id;
        [Required]
        public ShippingAgent carrier;
        [Required]
        public double totalCapacity;
        public double usedCapacity;
        [Required]
        public double cost;
        [Required]
        public DateTime departureTime;
        [Required]
        public System.DateTime arrivalTime;
        [Required]
        public CarrierType carrierType;

        public string uniqueIdentifier;

    }
}