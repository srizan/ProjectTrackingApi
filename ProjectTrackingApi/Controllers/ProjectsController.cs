using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTrackingApi.Data;
using ProjectTrackingApi.Models;

namespace ProjectTrackingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            var projects = await _context.Projects.ToListAsync();
            return projects;
        }

        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            if (project == null)
            {
                return BadRequest("Project cannot be null.");
            }
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProjects), new { id = project.Id }, project);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProject(int Id)
        {
            var project = await _context.Projects.FindAsync(Id);
            if (project == null)
            {
                return NotFound();
            }
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateProject(int Id, Project project)
        {
            if (Id != project.Id)
            {
                return BadRequest("Project ID mismatch.");
            }
            var existingProject = await _context.Projects.FindAsync(Id);
            if (existingProject == null)
            {
                return NotFound();
            }
            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.status = project.status;
            existingProject.owner = project.owner;
            existingProject.StartDate = project.StartDate;
            existingProject.EndDate = project.EndDate;
            _context.Entry(existingProject).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
