using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirportModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiLab3.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private AirportContext _context;

        public TripsController(AirportContext context) => _context = context;

        // GET: api/<AirportController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trip>>> Get()
        {
            return await _context.Trips.ToListAsync();
        }

        // GET api/<AirportController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            Trip trip = _context.Trips.FirstOrDefault(x => x.TripId == id);
            if (trip == null)
                return NotFound();
            return new ObjectResult(trip);

        }

        // POST api/<AirportController>
        [HttpPost]

        public async Task<ActionResult<Trip>> Post(Trip trip)
        {
            if (trip == null)
                return BadRequest(); 
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();
            return Ok(trip);
        }

        // PUT api/<AirportController>/5
        [HttpPut]
        public async Task<ActionResult<Trip>> Put(Trip trip)
        {
            if (trip == null)
                return BadRequest();
            if (!_context.Trips.Any(x => x.TripId == trip.TripId))
                return NotFound();
            _context.Update(trip);
            await _context.SaveChangesAsync();
            return Ok(trip);
        }

        // DELETE api/<AirportController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Trip>> Delete(int id)
        {
            Trip trip = _context.Trips.FirstOrDefault(x => x.TripId == id);
            if (trip == null)
                return NotFound();
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
            return Ok(trip);
        }
    }
}
