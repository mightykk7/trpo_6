using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using pract6.Models;

namespace pract6.Controllers
{
    [Route("api/v1/[controller]")]
    public class VacanciesController : Controller
    {
        private readonly WebApiDatabaseContext _context;
        public VacanciesController(WebApiDatabaseContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll([FromQuery] string? city, [FromQuery] decimal? minSalary)
        {
            var vacancies = _context.Vacancies.ToList();
            if (city != null)
                vacancies = vacancies.Where(p => p.City == city).ToList();
            if (minSalary.HasValue)
                vacancies = vacancies.Where(p => p.Salary >= minSalary.Value).ToList();
            return Ok(vacancies);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var vacancies = _context.Vacancies.Find(id);
            if (vacancies == null)
                return NotFound();
            return Ok(vacancies);
        }
        [HttpPost]
        public IActionResult Create([FromBody] Vacancy vacancy)
        {
            _context.Vacancies.Add(vacancy);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = vacancy.Id }, vacancy);
        }
        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] Vacancy putvacancy) 
        {
            if (id != putvacancy.Id)
                return BadRequest();
            _context.Vacancies.Update(putvacancy);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpPatch("{id:int}")]
        public IActionResult Patch([FromRoute]int id, [FromBody] Vacancy patchvacancy)
        {

            var vacancy = _context.Vacancies.Find(id);
            if (vacancy == null)
                return NotFound();
            if (patchvacancy.Title != null)
                vacancy.Title = patchvacancy.Title;
            if (patchvacancy.Company != null)
                vacancy.Company = patchvacancy.Company;
            if (patchvacancy.Salary >=0)
                vacancy.Salary = patchvacancy.Salary;
            if (patchvacancy.City != null)
                vacancy.City = patchvacancy.City;
            if (patchvacancy.IsActive.HasValue)
                vacancy.IsActive = patchvacancy.IsActive;
            _context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id:int}")]
        public IActionResult Deletel(int id)
        {
            var vacancy = _context.Vacancies.Find(id);
            if (vacancy == null)
                return NotFound();
            _context.Vacancies.Remove(vacancy);
            _context.SaveChanges();
            return Ok();
        }
    }
}
