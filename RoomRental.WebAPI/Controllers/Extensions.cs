using RoomRental.Application.Interfaces;
using RoomRental.Domain.Entities.RoomRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomRental.WebAPI.Controllers
{
    public static class Extensions
    {
        public static User GetUser(this string name, IRoomRentalContext dbContext)
        {
            return dbContext.Users.FirstOrDefault(e => e.UserName == name);
        }
    }
}
