using TaskProject.Models.Models.DTO;
namespace TaskProject.Services;

public interface ITaskService
{
    //Pelos vistos tenho de usar nos Serviços os *DTO's*
    
    Task<IEnumerable<TasksDTO>> GetAllTasksAsync();
    Task<TasksDTO?> GetTaskByIdAsync(int id);
    Task<IEnumerable<TasksDTO>> GetTasksByCategoryAsync(int categoryId);
    Task<TasksDTO> CreateTaskAsync(CreateTaskDTO dto);
    Task<TasksDTO?> UpdateTaskAsync(int id, UpdateTaskDto dto);
    Task<bool> DeleteTaskAsync(int id);
    
    
}