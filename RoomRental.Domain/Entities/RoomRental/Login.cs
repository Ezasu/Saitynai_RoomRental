using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RoomRental.Domain.Entities.RoomRental
{
    public class Login
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
