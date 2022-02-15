using Api.Data;
using Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class HeroesController : ControllerBase
    {
        private DataDbContext _context { get; set; }

        public HeroesController(DataDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<List<HeroViewModel>>> Get()
        {
            var heroesDb = await _context.Heroes.ToListAsync();
            var heroes = heroesDb.Select(heroes => 
                new HeroViewModel
                {
                    Id = heroes.Id,
                    Name = heroes.Name,
                    LastName = heroes.LastName,
                    FirstName = heroes.FirstName,
                    Place = heroes.Place
                });
            return Ok(heroes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HeroViewModel>> Get(int id)
        {
            var heroDB = await _context.Heroes.FindAsync(id);
            if (heroDB == null)
            { 
                return NotFound($"Hero with id '{id}' not found.");
            }

            var hero = new HeroViewModel 
            {
                Id=heroDB.Id,
                Name = heroDB.Name, 
                FirstName = heroDB.FirstName, 
                Place = heroDB.Place, 
                LastName = heroDB.LastName
            };

            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<HeroViewModel>> Post(HeroViewModel hero)
        {
            var heroDb = new Models.Hero
            {
                Id = hero.Id,
                FirstName = hero.FirstName,
                LastName = hero.LastName,
                Place = hero.Place,
                Name = hero.Name,
            };
            
            _context.Heroes.Add(heroDb);
            await _context.SaveChangesAsync();
            return Created($"api/heroes/{hero.Id}", hero);
        }

        [HttpPut]
        public async Task<ActionResult<HeroViewModel>> Put(HeroViewModel hero)
        {
            if (hero == null)
            {
                return BadRequest();
            }

            var dbHero = await _context.Heroes.FindAsync(hero.Id);
            if (dbHero == null)
            {
                return NotFound();
            }

            dbHero.Id = hero.Id;
            dbHero.Name = hero.Name;
            dbHero.FirstName = hero.FirstName;
            dbHero.LastName = hero.LastName;
            dbHero.Place = hero.Place;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var hero = await _context.Heroes.FindAsync(id);
            if (hero == null)
            {
                return NotFound($"Hero with id '{id}' not found.");
            }
            _context.Heroes.Remove(hero);
            await _context.SaveChangesAsync();
            return Ok(hero);
        }

    }
}
