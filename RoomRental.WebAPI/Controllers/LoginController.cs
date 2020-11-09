using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomRental.Domain.Entities.RoomRental;
using RoomRental.WebAPI.Authentication;
using RoomRental.WebAPI.Services;

namespace RoomRental.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {
        //private IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthenticationManager;

        public LoginController(/*IConfiguration config, */IJwtAuthManager jwtAuthenticationManager, IUserService userService)
        {
            //_config = config;
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] Login request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_userService.IsValidUserCredentials(request.Username, request.Password))
            {
                return Unauthorized();
            }

            var role = _userService.GetUserRole(request.Username);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.Username),
                new Claim(ClaimTypes.Role, role)
            };

            var jwtResult = _jwtAuthenticationManager.GenerateTokens(request.Username, claims, DateTime.Now);
            //_logger.LogInformation($"User [{request.Username}] logged in the system.");
            return Ok(new LoginResult
            {
                UserName = request.Username,
                Role = role,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }

        //[HttpPost]
        //public IActionResult Authenticate([FromBody] Login user)
        //{
        //    var token = _jwtAuthenticationManager.Authenticate(user.Username, user.Password);
        //    if (token != null)
        //        return Ok(token);

        //    return Unauthorized();
        //}
    }

    public class LoginResult
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }

    public class ImpersonationRequest
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }
    }
}
