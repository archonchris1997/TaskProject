using TaskProject.Models.Models.DTO;

namespace TaskProject.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<CategoryDto> CreateAsync(CategoryDto dto);
    Task<CategoryDto?> UpdateAsync(int id, CategoryDto dto);
    Task<bool> DeleteAsync(int id);
    
}