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
            celestialObject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId != null && c.OrbitedObjectId == celestialObject.Id).ToList();
            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context?.CelestialObjects.Where(c => c.Name == name).ToList();

            if (celestialObjects == null || !celestialObjects.Any())
            {
                return NotFound();
            }
            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId != null && c.OrbitedObjectId == celestialObject.Id).ToList();
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
                celestialObject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId != null && c.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(_context.CelestialObjects);
        }
        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }
        [HttpPut("{id}")]
        public IActionResult Update (int id, CelestialObject celestialObject)
        {
           var oldCelestialObject =  _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (oldCelestialObject == null)
                return NotFound();
            oldCelestialObject.Name = celestialObject.Name;
            oldCelestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            oldCelestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.Update(oldCelestialObject);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
