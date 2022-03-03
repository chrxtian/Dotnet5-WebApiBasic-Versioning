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
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class HeroesController : ControllerBase
    {
        private DataDbContext _context { get; set; }

        public HeroesController(DataDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        [MapToApiVersion("1.0")]
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

        [HttpGet]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult<List<HeroViewModelV2>>> GetHeroesV2()
        {
            var heroesDb = await _context.Heroes.ToListAsync();
            var heroes = heroesDb.Select(heroes =>
                new HeroViewModelV2
                {
                    Id = heroes.Id,
                    Name = heroes.Name,
                    FullName = $"{heroes.LastName} {heroes.LastName}",
                    Place = heroes.Place
                });
            return Ok(heroes);
        }

        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
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

        //[HttpGet("{id}")]
        //[MapToApiVersion("2.0")]
        //public async Task<ActionResult<HeroViewModelV2>> GetHeroV2(int id)
        //{
        //    var heroDB = await _context.Heroes.FindAsync(id);
        //    if (heroDB == null)
        //    {
        //        return NotFound($"Hero with id '{id}' not found.");
        //    }

        //    var hero = new HeroViewModelV2
        //    {
        //        Id = heroDB.Id,
        //        Name = heroDB.Name,
        //        FullName = $"{heroDB.FirstName} {heroDB.LastName}",
        //        Place = heroDB.Place
        //    };

        //    return Ok(hero);
        //}


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
            return Created($"api/heroes/{heroDb.Id}", heroDb);
        }

        //[HttpPost]
        //[MapToApiVersion("2.0")]
        //public async Task<ActionResult<HeroViewModelV2>> PostV2(NewHeroViewModel hero)
        //{
        //    var heroDb = new Models.Hero
        //    {
        //        FirstName = hero.FirstName,
        //        LastName = hero.LastName,
        //        Place = hero.Place,
        //        Name = hero.Name,
        //    };

        //    _context.Heroes.Add(heroDb);
        //    await _context.SaveChangesAsync();

        //    var heroViewModel = new HeroViewModelV2
        //    {
        //       Id = heroDb.Id,
        //       Name = heroDb.Name,
        //       FullName = $"{heroDb.FirstName} {heroDb.LastName}",
        //       Place = heroDb.Place
        //    };

        //    return Created($"api/heroes/{heroDb.Id}", heroViewModel);
        //}

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
