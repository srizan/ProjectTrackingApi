using Microsoft.EntityFrameworkCore;
using ProjectTrackingApi.Models;

namespace ProjectTrackingApi.Data.Repositories
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProjectsRepository> _logger;

        public ProjectsRepository(AppDbContext context, ILogger<ProjectsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ProjectDto> CreateProject(ProjectDto projectDto)
        {
            if (projectDto.StartDate > projectDto.EndDate)
            {
                throw new Exception("Startdate cannot be after Enddate.");
            }

            Project project = new Project
            {
                Name = projectDto.Name,
                Description = projectDto.Description,
                Status = projectDto.Status,
                Owner = projectDto.Owner,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate
            };
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            projectDto.Id = project.Id; // Set the ID of the created project
            return projectDto;
        }

        public async Task<bool> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                _logger.LogWarning("Project with ID {id} not found for deletion.", id);
                return false;
            }
            _logger.LogInformation("Deleting project with ID - {id}.", id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProjectDto> GetProject(int id)
        {
           var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return null;
            }
            var projectDto = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Status = project.Status,
                Owner = project.Owner,
                StartDate = project.StartDate,
                EndDate = project.EndDate
            };
            return projectDto;
        }

        public async Task<IEnumerable<ProjectDto>> GetProjects()
        {
            var projects = await _context.Projects.ToListAsync();
            var projectDtos = projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Status = p.Status,
                Owner = p.Owner,
                StartDate = p.StartDate,
                EndDate = p.EndDate
            }).ToList();
            return projectDtos;
        }

        public async Task<ProjectDto> UpdateProject(ProjectDto projectDto)
        {
            var existingProject = await _context.Projects.FindAsync(projectDto.Id);
            if (existingProject == null)
            {
                _logger.LogWarning("Project with ID {id} not found for update.", projectDto.Id);
                return null;
            }
            if (projectDto.StartDate > projectDto.EndDate)
            {
                throw new Exception("Startdate cannot be after Enddate.");
            }

            _logger.LogInformation("Updating project with ID {id}.", projectDto.Id);
            existingProject.Name = projectDto.Name;
            existingProject.Description = projectDto.Description;
            existingProject.Status = projectDto.Status;
            existingProject.Owner = projectDto.Owner;
            existingProject.StartDate = projectDto.StartDate;
            existingProject.EndDate = projectDto.EndDate;

            _logger.LogInformation("Saving changes for project with ID {id}.", projectDto.Id);
            _context.Entry(existingProject).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return projectDto;
        }
    }

}