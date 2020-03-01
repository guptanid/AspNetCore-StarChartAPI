using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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
            var celestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (celestialObject == null)
            {
                return NotFound();
            }
            var satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId != null && c.OrbitedObjectId == celestialObject.Id);
            if (satellites != null && satellites.Any())
                celestialObject.Satellites.AddRange(satellites);
            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context?.CelestialObjects.Where(c => c.Name == name).ToList();

            if (celestialObjects == null)
            {
                return NotFound();
            }
            foreach (var celestialObject in celestialObjects)
            {
                var satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId != null && c.OrbitedObjectId == celestialObject.Id);
                if (satellites != null && satellites.Any())
                    celestialObject.Satellites.AddRange(satellites);
            }
            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            if (_context.CelestialObjects == null || !_context.CelestialObjects.Any())
                return NotFound();

            foreach (var celestialObject in _context.CelestialObjects)
            {
                var satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId != null && c.OrbitedObjectId == celestialObject.Id);
                if (satellites != null && satellites.Any())
                    celestialObject.Satellites.AddRange(satellites);
            }
            return Ok(_context.CelestialObjects);
        }

        private void AddSatellites(CelestialObject celestialObject)
        {

        }
    }
}
