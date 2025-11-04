namespace TaskProject.Repositories;
using TaskProject.Models;

public interface ITaskRepository
{
    Task<IEnumerable<Task>> GetAllAsync();
    
    Task<Task?> GetByIdAsync(int id);
    Task<IEnumerable<Task>> GetByCategoryAsync(int categoryId);
    Task <Task> CreateAsync(Task task);
    Task <Task> UpdateAsync(Task task);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistAsync(int id);
    
}