using Microsoft.Extensions.Logging;
using RoomRental.Domain.Entities.RoomRental;
using RoomRental.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomRental.WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly RoomRentalContext _context;


        //private readonly IDictionary<string, string> _users = new Dictionary<string, string>
        //{
        //    { "test1", "password1" },
        //    { "test2", "password2" },
        //    { "admin", "securePassword" }
        //};
        // inject your database here for user validation
        public UserService(ILogger<UserService> logger, RoomRentalContext context)
        {
            _logger = logger;
            _context = context;
        }

        public bool IsValidUserCredentials(string userName, string password)
        {
            _logger.LogInformation($"Validating user [{userName}]");
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            return _context.Users.Any(e => e.UserName == userName && e.Password == password);//_users.TryGetValue(userName, out var p) && p == password;
        }

        public bool IsAnExistingUser(string userName, out User result)
        {
            result = _context.Users.FirstOrDefault(e => e.UserName == userName);
            return result != null;
        }

        public string GetUserRole(string userName)
        {
            if (!IsAnExistingUser(userName, out User user))
            {
                return string.Empty;
            }

            switch (user.Role)
            {
                case Role.User:
                    return "User";

                case Role.Administrator:
                    return "Administrator";

                case Role.Guest:
                    return "Guest";
            }

            return string.Empty;
        }
    }
}
