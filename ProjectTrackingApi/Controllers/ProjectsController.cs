using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTrackingApi.Data;
using ProjectTrackingApi.Data.Repositories;
using ProjectTrackingApi.Models;

namespace ProjectTrackingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : Controller
    {
         private readonly ILogger<ProjectsController> _logger;
        private readonly IProjectsRepository _projectsRepository;

        public ProjectsController(ILogger<ProjectsController> logger, IProjectsRepository projectsRepository)
        {
            _logger = logger;
            _projectsRepository = projectsRepository;
        }

        /// <summary>
        /// Returns all projects.
        /// </summary>
        /// <returns>A list of projects</returns>
        /// <response code="200">Returns the list of projects</response>
        /// <response code="500">An error occurred while fetching projects</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
        {
            try
            {
                _logger.LogInformation("Fetching all projects.");
                var projects = await _projectsRepository.GetProjects();
                return Ok(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching projects.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Returns a project by ID.
        /// </summary>
        /// <param name="id">The ID of the project</param>
        /// <returns>The requested project by ID</returns>
        /// <response code="200">Returns the project</response>
        /// <response code="404">Project not found</response>
        /// <response code="500">An error occurred while fetching the project</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(int id)
        {
            try
            {
                _logger.LogInformation("Fetching project by the ID {id}.", id);

                var ProjectDetails = await _projectsRepository.GetProject(id);
                if (ProjectDetails == null)
                {
                    _logger.LogWarning("Project with ID {id} not found.", id);
                    return NotFound();
                }

                return Ok(ProjectDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching project with ID {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the project.");
            }

        }


        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="projectDto">The project to create</param>
        /// <returns>The created project</returns>
        /// <response code="201">Project created successfully</response>
        /// <response code="400">Invalid project data</response>
        /// <response code="500">An error occurred while creating the project</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProjectDto>> CreateProject(ProjectDto projectDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for project creation.");
                return BadRequest(ModelState);
            }

            if (projectDto == null)
            {
                _logger.LogWarning("Project data cannot be null for creating new Project.");
                return BadRequest("Project data cannot be null for creating new Project.");
            }         

            try
            {
                _logger.LogInformation("Creating new project: {Name}", projectDto.Name);
                projectDto = await _projectsRepository.CreateProject(projectDto);

                _logger.LogInformation("Project created successfully with ID {Id}.", projectDto.Id);
                return CreatedAtAction(nameof(GetProjects), new { id = projectDto.Id }, projectDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating project. for project: {Name}", projectDto.Name);
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating the project. {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="id">The ID of the project to update</param>
        /// <param name="projectDto">The updated project data</param>
        /// <returns>No content</returns>
        /// <response code="204">Project updated successfully</response>
        /// <response code="400">Invalid project data or ID mismatch</response>
        /// <response code="404">Project not found</response>
        /// <response code="500">An error occurred while updating the project</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, ProjectDto projectDto)
        {
            if (id != projectDto.Id)
            {
                _logger.LogWarning("Project ID mismatch: expected {ExpectedId}, received {ReceivedId}.", id, projectDto.Id);
                return BadRequest("Project ID mismatch.");
            }
            if (projectDto == null)
            {
                _logger.LogWarning("Project cannot be null to update.");
                return BadRequest("Project cannot be null.");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for project update.");
                return BadRequest(ModelState);
            }           

            try
            {
                var updatedProject = await _projectsRepository.UpdateProject(projectDto);

                if(updatedProject == null)
                {
                    _logger.LogWarning("Project with ID {id} not found for update.", id);
                    return NotFound();
                }
                _logger.LogInformation("Project with ID {id} updated successfully.", id);
                return NoContent();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project with ID {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating the project. {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a project by ID.
        /// </summary>
        /// <param name="id">The ID of the project to delete</param>
        /// <returns>No content</returns>
        /// <response code="204">Project deleted successfully</response>
        /// <response code="404">Project not found</response>
        /// <response code="500">An error occurred while deleting the project</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                var success = await _projectsRepository.DeleteProject(id);

                if (!success)
                {
                    _logger.LogWarning("Project with ID {id} not found for deletion.", id);
                    return NotFound();
                }

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error deleting project with ID {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the project.");
            }
        }


    }
}
