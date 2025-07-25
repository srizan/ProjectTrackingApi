using ProjectTrackingApi.Models;

namespace ProjectTrackingApi.Data.Repositories
{
    public interface IProjectsRepository
    {
        Task<ProjectDto> CreateProject(ProjectDto projectDto);
        Task<bool> DeleteProject(int id);
        Task<ProjectDto> GetProject(int id);
        Task<IEnumerable<ProjectDto>> GetProjects();
        Task<ProjectDto> UpdateProject(ProjectDto projectDto);
    }
}