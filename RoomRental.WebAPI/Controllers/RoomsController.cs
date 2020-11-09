using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class RoomsController : ControllerBase
    {
        private readonly RoomRentalContext _context;

        public RoomsController(RoomRentalContext context)
        {
            _context = context;
        }

        // GET: api/Rooms
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            var rooms = await _context.Rooms.Include(e => e.LandlordUser).ToListAsync();

            foreach (var room in rooms)
            {
                var building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == room.BuildingId);
                if (building != null)
                {
                    building.Rooms = null;
                    room.Building = building;
                }
            }

            return rooms;
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.Rooms.Include(e => e.LandlordUser).FirstOrDefaultAsync(e => e.Id == id);

            if (room == null)
            {
                return NotFound();
            }

            var building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == room.BuildingId);
            if (building != null)
            {
                building.Rooms = null;
                room.Building = building;
            }


            return room;
        }

        // PUT: api/Rooms/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> PutRoom(int id, Room room)
        {
            if (id != room.Id)
            {
                return BadRequest();
            }

            var entity = _context.Rooms.FirstOrDefault(e => e.Id == id);
            User user = User.Identity.Name.GetUser(_context);

            if (entity is null || user is null)
            {
                return NotFound();
            }

            int userId = user.Id;
            Role userRole = user.Role;

            if (userRole != Role.Administrator && entity.LandlordUserId != userId)
                return Forbid();

            //_context.Entry(room).State = EntityState.Modified;

            if (userRole == Role.Administrator)
            {
                entity.BuildingId = room.BuildingId;
                entity.LandlordUserId = room.LandlordUserId;
            }
            entity.NumberOfRooms = room.NumberOfRooms;
            entity.PricePerDay = room.PricePerDay;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
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

        // POST: api/Rooms
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult<Room>> PostRoom(Room room)
        {
            if (room.CreateDate != DateTime.Now)
                room.CreateDate = DateTime.Now;

            if (room.BuildingId == 0)
            {
                return BadRequest("Building Id was not specified.");
            }

            if (await _context.Buildings.FirstOrDefaultAsync(e => e.Id == room.BuildingId) == null)
            {
                return NotFound("Building was not found.");
            }

            User user = User.Identity.Name.GetUser(_context);
            if (user is null)
            {
                return Unauthorized();
            }

            if (user.Role != Role.Administrator && (room.LandlordUserId != 0 && room.LandlordUserId != user.Id))
            {
                return Forbid();
            }

            if (room.LandlordUserId == 0)
            {
                if (user.Role == Role.Administrator)
                {
                    return BadRequest("LandLordUserId is required");
                }
                room.LandlordUserId = user.Id;
            }

            try
            {
                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Created($"/api/Rooms", new { Id = room.Id });
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult<Room>> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            User user = User.Identity.Name.GetUser(_context);

            if (user is null)
            {
                return Unauthorized();
            }

            if (user.Role != Role.Administrator && room.LandlordUserId != user.Id)
            {
                return Forbid();
            }

            try
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }

   

            return room;
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("/api/Buildings/{buildingId:int}/Rooms")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Room>>> GetRoomsByBuilding(int buildingId)
        {
            var rooms = await _context.Rooms.Include(e => e.LandlordUser).Where(e => e.BuildingId == buildingId).ToListAsync();

            if (rooms?.Any() ?? true)
                return NotFound();

            foreach (var room in rooms)
            {
                var building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == room.BuildingId);
                if (building != null)
                {
                    building.Rooms = null;
                    room.Building = building;
                }
            }

            return rooms;
        }

        [HttpGet]
        [Route("/api/Buildings/{buildingId:int}/Rooms/{roomId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Room>> GetRoomByBuilding(int buildingId, int roomId)
        {
            var room = await _context.Rooms.Include(e => e.LandlordUser).FirstOrDefaultAsync(e => e.BuildingId == buildingId && e.Id == roomId);

            if (room is null)
                return NotFound();

            var building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == room.BuildingId);
            if (building != null)
            {
                building.Rooms = null;
                room.Building = building;
            }

            return room;
        }

        [HttpPost]
        [Route("/api/Buildings/{buildingId:int}/Rooms")]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult<Room>> PostRoomForBuulding(int buildingId, Room room)
        {
            if (buildingId == 0 && room.BuildingId == 0)
                return BadRequest();

            room.BuildingId = buildingId;

            Building foundBuulding = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == buildingId);

            if (foundBuulding is null)
                return NotFound("Building was not found.");

            User user = User.Identity.Name.GetUser(_context);
            if (user is null)
                return Unauthorized();

            room.BuildingId = buildingId;

            if (room.CreateDate != DateTime.Now)
                room.CreateDate = DateTime.Now;

            if (user.Role != Role.Administrator && (room.LandlordUserId != 0 && room.LandlordUserId != user.Id))
            {
                return Forbid();
            }

            if (room.LandlordUserId == 0)
            {
                if (user.Role == Role.Administrator)
                {
                    return BadRequest("LandLordUserId is required");
                }
                room.LandlordUserId = user.Id;
            }

            try
            {
                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Created($"/api/Buildings/{buildingId}/Rooms", new { Id = room.Id });
        }

        [HttpDelete]
        [Route("/api/Buildings/{buildingId:int}/Rooms/{roomId:int}")]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult<Room>> DeleteRoomByBuilding(int buildingId, int roomId)
        {
            var building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == buildingId);
            if (building is null)
                return NotFound("Building was not found.");

            var room = await _context.Rooms.FirstOrDefaultAsync(e => e.Id == roomId && e.BuildingId == buildingId);
            if (room is null)
            {
                return NotFound("Room was not found.");
            }

            User user = User.Identity.Name.GetUser(_context);
            if (user is null)
                return Unauthorized();

            if (user.Role != Role.Administrator && room.LandlordUserId != user.Id)
                return Forbid();

            try
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }



            return room;
        }
    }
}
