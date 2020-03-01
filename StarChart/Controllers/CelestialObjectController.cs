using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects?.FirstOrDefault(c => c.Id.Equals(id));

            if (celestialObject == null)
            {
                return NotFound();
            }

            var satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId != null && c.OrbitedObjectId.Equals(id));

            foreach (var satellite in satellites)
            {
                celestialObject.Satellites.Add(satellite);
            }
            return Ok(celestialObject);
        }
    }
}
