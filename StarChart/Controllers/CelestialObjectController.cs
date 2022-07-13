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
            if (!o.Any())
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

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            this._context.CelestialObjects.Add(celestialObject);
            this._context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject updatedCelestialObject)
        {
            var o = this._context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if (o == null)
                return NotFound();
            o.Name = updatedCelestialObject.Name;
            o.OrbitalPeriod = updatedCelestialObject.OrbitalPeriod;
            o.OrbitedObjectId = updatedCelestialObject.OrbitedObjectId;
            this._context.CelestialObjects.Update(o);
            this._context.SaveChanges();
            return NoContent();
        }


        [HttpPatch("{id}/{name}")]
        public IActionResult Update(int id, string name)
        {
            var o = this._context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if (o == null)
                return NotFound();
            o.Name = name;
            this._context.CelestialObjects.Update(o);
            this._context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var o = this._context.CelestialObjects.Where(x => x.Id == id || x.OrbitedObjectId == id);
            if (!o.Any())
                return NotFound();
            this._context.CelestialObjects.RemoveRange(o);
            this._context.SaveChanges();
            return NoContent();
        }
    }
}
