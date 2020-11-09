using System;
using System.Collections.Generic;
using System.Text;

namespace RoomRental.Domain.Entities.RoomRental
{
    public class User : BaseClass
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
