using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RoomRental.Domain.Entities.RoomRental
{
    public class Room : BaseClass
    {
        public Room() : base()
        {
            Reservations = new HashSet<Reservation>();
        }
        [Required]
        public int NumberOfRooms { get; set; }
        [Required]
        public decimal PricePerDay { get; set; }
        [Required]
        public int LandlordUserId { get; set; }
        public User LandlordUser { get; set; }
        [Required]
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public ICollection<Reservation> Reservations { get; set; }

    }
}
