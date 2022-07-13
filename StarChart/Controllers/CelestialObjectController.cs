using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var o = this._context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if (o == null)
                return NotFound();
            o.Satellites = this._context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();  
            return Ok(o);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var o = this._context.CelestialObjects.Where(x => x.Name == name);
            if (o.Any())
                return NotFound();

            foreach (var c in o)
            {
                c.Satellites = this._context.CelestialObjects.Where(x => x.OrbitedObjectId == c.Id).ToList();
            }

            return Ok(o);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var o = this._context.CelestialObjects.ToList();
            if (o == null)
                return NotFound();
            foreach (var c in o)
            {
                c.Satellites = this._context.CelestialObjects.Where(x => x.OrbitedObjectId == c.Id).ToList();
            }
            return Ok(o);
        }
    }
}
