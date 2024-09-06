using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobHunterAPI.Data;
using JobHunterAPI.Models;
using JobHunterAPI.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace JobHunterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }




        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobDto>>> GetAlljobs([FromHeader(Name = "x-api-key")] string ApiKey)
        {
          

            // Build the query with the search term applied directly in the database query
            var jobs = _context.jobs
                .Include(j => j.JobCategories)
                .ThenInclude(jc => jc.Category);  

           
           

            // Execute the query and project to DTOs
            var jobDtos = await jobs
                .Select(job => new JobDto
                {
                    Id = job.Id,
                    CompanyName = job.CompanyName,
                    CompanyLogo = job.CompanyLogo,
                    JobTitle = job.JobTitle,
                    JobType = job.JobType,
                    JobDescription = job.JobDescription,
                    JobStatus = job.JobStatus,
                    RequiredExperience = job.RequiredExperience,
                    Salary = job.Salary,
                    JobLocation = job.JobLocation,
                    ExpiresOn = job.ExpiresOn,
                    CreatedOn = job.CreatedOn,
                    categories = job.JobCategories.Select(k => k.Category).ToList()
                })
                .ToListAsync();

            return Ok(jobDtos);
        }

        // GET: api/Jobs?search=software
        [HttpGet("S")]
        public async Task<ActionResult<IEnumerable<JobDto>>> Getjobs(string? search)
        {
            if (_context.jobs == null)
            {
                return Problem("No Jobs available");
            }

            // Build the query with the search term applied directly in the database query
            var query = _context.jobs
                .Include(j => j.JobCategories)
                .ThenInclude(jc => jc.Category)
                .AsQueryable();  // Convert to IQueryable to allow further filtering

            // Apply the search filter if provided
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper(); 
                query = query.Where(j =>
                j.JobTitle.ToUpper().Contains(search) ||
            j.CompanyName.ToUpper().Contains(search) ||
            j.JobDescription.ToUpper().Contains(search) ||
            j.JobLocation.ToUpper().Contains(search));
            }

            // Execute the query and project to DTOs
            var jobDtos = await query
                .Select(job => new JobDto
                {
                    Id = job.Id,
                    CompanyName = job.CompanyName,
                    CompanyLogo = job.CompanyLogo,
                    JobTitle = job.JobTitle,
                    JobType = job.JobType,
                    JobDescription = job.JobDescription,
                    JobStatus = job.JobStatus,
                    RequiredExperience = job.RequiredExperience,
                    Salary = job.Salary,
                    JobLocation = job.JobLocation,
                    ExpiresOn = job.ExpiresOn,
                    CreatedOn = job.CreatedOn,
                    categories = job.JobCategories.Select(k => k.Category).ToList()
                })
                .ToListAsync();

            return Ok(jobDtos);
        }


        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetJob(int id)
        {
            var job = await _context.jobs
                .Include(j => j.JobCategories)
                .ThenInclude(jc => jc.Category)
                .SingleOrDefaultAsync(j => j.Id == id);

            if (job == null)
            {
                return NotFound();
            }

            var jobdto = new JobDto
            {
                Id = job.Id,
                CompanyName = job.CompanyName,
                CompanyLogo = job.CompanyLogo,
                JobTitle = job.JobTitle,
                JobType = job.JobType,
                JobDescription = job.JobDescription,
                JobStatus = job.JobStatus,
                RequiredExperience = job.RequiredExperience,
                Salary = job.Salary,
                JobLocation = job.JobLocation,
                ExpiresOn = job.ExpiresOn,
                CreatedOn = job.CreatedOn,
                //CategoryIds = job.JobCategories.Select(jc=>jc.CategoryId).ToList(),
                categories = job.JobCategories.Select(k => k.Category).ToList()
            };
            return Ok(jobdto);
        }

        // PUT: api/Jobs/5
       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJob(int id, Job job)
        {
            if (id != job.Id)
            {
                return BadRequest();
            }

            _context.Entry(job).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Jobs
        [HttpPost]
        public async Task<ActionResult<JobDto>> PostJob([FromBody] JobDto jobdto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var job = new Job
            {
                CompanyName = jobdto.CompanyName,
                CompanyLogo = jobdto.CompanyLogo,
                JobTitle = jobdto.JobTitle,
                JobType = jobdto.JobType,
                JobDescription = jobdto.JobDescription,
                JobStatus = jobdto.JobStatus,
                RequiredExperience = jobdto.RequiredExperience,
                Salary = jobdto.Salary,
                JobLocation = jobdto.JobLocation,
                ExpiresOn = jobdto.ExpiresOn,
                CreatedOn = jobdto.CreatedOn,
                JobCategories = jobdto.CategoryIds.Select(Id => new JobCategory { CategoryId = Id }).ToList(),
              //  Categories =jobdto.JobCategories.Select(k => k.Category).ToList()
            };

            _context.jobs.Add(job);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJob", new { id = job.Id }, jobdto);
        }

        // DELETE: api/Jobs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.jobs.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool JobExists(int id)
        {
            return _context.jobs.Any(e => e.Id == id);
        }
    }
}
