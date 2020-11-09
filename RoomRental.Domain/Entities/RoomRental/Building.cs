using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RoomRental.Domain.Entities.RoomRental
{
    public class Building : BaseClass
    {
        public Building() : base()
        {
            Rooms = new HashSet<Room>();
        }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public int Address { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}
