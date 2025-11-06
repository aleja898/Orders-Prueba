using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CountriesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Countries.AsNoTracking().ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(Country country)
        {
            var countryExist = await _context.Countries.AnyAsync(x => x.Id == country.Id);           

            if (!countryExist)
            {
                return BadRequest("ID de país no coincide con el cuerpo de la solicitud.");
            }
            
            try
            {
                _context.Update(country);
                await _context.SaveChangesAsync();
                return Ok(country); 
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un país con el mismo nombre.");
                }
                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            _context.Remove(country);
            await _context.SaveChangesAsync();
            return NoContent(); 
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Country country)
        {
            try
            {
                _context.Add(country);
                await _context.SaveChangesAsync();

                return Created($"/api/countries/{country.Id}", country);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un país con el mismo nombre.");
                }
                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}