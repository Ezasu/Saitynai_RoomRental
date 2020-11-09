using RoomRental.Domain.Entities.RoomRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomRental.WebAPI.Services
{
    public interface IUserService
    {
        bool IsAnExistingUser(string userName, out User user);
        bool IsValidUserCredentials(string userName, string password);
        string GetUserRole(string userName);
    }
}
