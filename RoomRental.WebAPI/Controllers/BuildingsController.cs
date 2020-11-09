using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RoomRental.Domain.Entities.RoomRental;
using RoomRental.Persistence;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace RoomRental.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BuildingsController : ControllerBase
    {
        private readonly RoomRentalContext _context;

        public BuildingsController(RoomRentalContext context)
        {
            _context = context;
        }

        // GET: api/Buildings
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Building>>> GetBuildings()
        {
            return await _context.Buildings.ToListAsync();
        }

        // GET: api/Buildings/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Building>> GetBuilding(int id)
        {
            var building = await _context.Buildings.FindAsync(id);

            if (building == null)
            {
                return NotFound();
            }

            return building;
        }

        // PUT: api/Buildings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutBuilding(int id, Building building)
        {
            if (id != building.Id)
            {
                return BadRequest();
            }

            _context.Entry(building).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuildingExists(id))
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

        // POST: api/Buildings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Building>> PostBuilding(Building building)
        {
            if (building.Id != 0)
            {
                return BadRequest();
            }

            if (building.CreateDate != DateTime.Now)
                building.CreateDate = DateTime.Now;

            //List<string> errors = new List<string>();
            //foreach (PropertyInfo prop in typeof(Building).GetProperties().Where(e => e.CustomAttributes.Any(e => e.AttributeType.Name.Contains("Required"))))
            //{
            //    string value = prop.GetValue(building)?.ToString();
            //    if (value is null || string.IsNullOrWhiteSpace(value) || value == "0")
            //    {
            //        errors.Add($"A value for column/property \"{prop.Name}\" is required.");
            //    }
            //}
            //if (errors.Any())
            //{
            //    return BadRequest(errors.Aggregate("", (accumulator, next) => accumulator + next + "\n"));
            //}

            _context.Buildings.Add(building);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest();
            }


            return Created($"/api/Buildings", new { Id = building.Id });
        }

        // DELETE: api/Buildings/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Building>> DeleteBuilding(int id)
        {
            var building = await _context.Buildings.FindAsync(id);
            if (building == null)
            {
                return NotFound();
            }

            try
            {
                _context.Buildings.Remove(building);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            

            return building;
        }

        private bool BuildingExists(int id)
        {
            return _context.Buildings.Any(e => e.Id == id);
        }
    }
}
