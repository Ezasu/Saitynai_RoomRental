using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RoomRental.Domain.Entities.RoomRental
{
    public class Reservation : BaseClass
    {
        [Required]
        public bool IsConfirmed { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int DurationInDays { get; set; }
        [Required]
        public int RoomId { get; set; }

        [Required]
        public int TenantUserId { get; set; }
        [Required]
        public int LandlordUserId { get; set; }
        public User TenantUser { get; set; }
        public User LandlordUser { get; set; }
        public Room Room { get; set; }
    }
}
