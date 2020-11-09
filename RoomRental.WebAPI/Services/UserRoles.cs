using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomRental.WebAPI.Services
{
    public static class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string User = nameof(User);
        public const string Guest = nameof(Guest);
    }
}
