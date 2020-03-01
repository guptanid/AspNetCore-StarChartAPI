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
            var celestialObject = _context?.CelestialObjects?.FirstOrDefault(c => c.Id.Equals(id));
            if (celestialObject == null)
            {
                return NotFound();
            }
            AddSatellites(celestialObject);
            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObject = _context?.CelestialObjects?.FirstOrDefault(c => c.Name.Equals(name));

            if (celestialObject == null)
            {
                return NotFound();
            }
            AddSatellites(celestialObject);
            return Ok(celestialObject);
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            if (_context?.CelestialObjects == null || !_context.CelestialObjects.Any())
                return NotFound();

            foreach (var celestialObject in _context.CelestialObjects)
            {
                AddSatellites(celestialObject);
            }
            return Ok(_context.CelestialObjects);
        }

        private void AddSatellites(CelestialObject celestialObject)
        {
            var satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId != null && c.OrbitedObjectId.Equals(celestialObject.Id)).ToList();
            foreach (var satellite in satellites)
            {
                celestialObject.Satellites.Add(satellite);
            }
        }
    }
}
