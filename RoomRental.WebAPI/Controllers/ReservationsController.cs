using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomRental.Domain.Entities.RoomRental;
using RoomRental.Persistence;

namespace RoomRental.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly RoomRentalContext _context;

        public ReservationsController(RoomRentalContext context)
        {
            _context = context;
        }

        // GET: api/Reservations
        [HttpGet]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            User user = User.Identity.Name.GetUser(_context);

            if (user is null)
                return NotFound();

            var result = await _context.Reservations
                .Include(e => e.LandlordUser).ToListAsync();

            foreach (var reservation in result)
            {
                var room = await _context.Rooms.FirstOrDefaultAsync(e => e.Id == reservation.RoomId);
                room.Reservations = null;

                var building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == room.BuildingId);
                if (building != null)
                {
                    building.Rooms = null;
                }
                room.Building = building;
                reservation.Room = room;
            }

            if (user.Role != Role.Administrator)
            {
                result = result.Where(e => e.LandlordUserId == user.Id || e.TenantUserId == user.Id).ToList();
            }

            return result;
        }

        // GET: api/Reservations/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            User user = User.Identity.Name.GetUser(_context);
            var reservation = await _context.Reservations.Include(e => e.TenantUser).FirstOrDefaultAsync(e => e.Id == id);

            if (reservation == null || user is null)
            {
                return NotFound();
            }

            int userId = user.Id;
            Role userRole = user.Role;

            if (userId == -1 || (userRole != Role.Administrator && reservation.LandlordUserId != userId && reservation.TenantUserId != userId))
                return Forbid();

            var room = await _context.Rooms.FirstOrDefaultAsync(e => e.Id == reservation.RoomId);
            room.Reservations = null;

            var building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == room.BuildingId);
            if (building != null)
            {
                building.Rooms = null;
            }
            room.Building = building;
            reservation.Room = room;

            return reservation;
        }

        // PUT: api/Reservations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return BadRequest();
            }

            var entity = _context.Reservations.FirstOrDefault(e => e.Id == id);
            User user = User.Identity.Name.GetUser(_context);

            if (entity is null || user is null)
            {
                return NotFound();
            }

            int userId = user.Id;
            Role userRole = user.Role;

            if (userId == -1 || (userRole != Role.Administrator && reservation.LandlordUserId != userId && reservation.TenantUserId != userId))
                return Forbid();

            //_context.Entry(reservation).State = EntityState.Modified;

            entity.DurationInDays = reservation.DurationInDays;
            entity.IsConfirmed = reservation.IsConfirmed;
            if (user.Role == Role.Administrator)
            {
                entity.LandlordUserId = reservation.LandlordUserId;
                entity.TenantUserId = reservation.TenantUserId;
                entity.RoomId = reservation.RoomId;
            }
            entity.Price = reservation.Price;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }

            return NoContent();
        }

        // POST: api/Reservations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {
            if (reservation.CreateDate != DateTime.Now)
                reservation.CreateDate = DateTime.Now;

            User user = User.Identity.Name.GetUser(_context);
            if (user is null)
                return Unauthorized();

            if (user.Role != Role.Administrator && (reservation.LandlordUserId != user.Id && reservation.TenantUserId != user.Id))
            {
                return Forbid();
            }

            try
            {
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }


            return Created($"/api/Reservations/", new { Id = reservation.Id });
        }

        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult<Reservation>> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            User user = User.Identity.Name.GetUser(_context);

            if (reservation == null || user is null)
            {
                return NotFound();
            }

            int userId = user.Id;
            Role userRole = user.Role;

            if (userId == -1 || (userRole != Role.Administrator && reservation.LandlordUserId != userId && reservation.TenantUserId != userId))
                return Forbid();

            try
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return reservation;
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
