using TaskProject.Models;
using TaskProject.Models.Models.DTO;
using TaskProject.Repositories;

namespace TaskProject.Services;

public class CategoryService(ICategoryRepository repo) : ICategoryService
{
    
    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var allMembers = await repo.GetAllAsync();

        return allMembers.Select(ToDto);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var element = await repo.GetByIdAsync(id);
        
        if (element is not null)
            return ToDto(element);
        
        return null;
        
    }

    public async Task<CategoryDto> CreateAsync(CategoryDto dto)
    {
        var created = await repo.CreateAsync(new Category { Name = dto.Name, Description = dto.Description, Color = dto.Color });
        
        return ToDto(created);
    }

    public async Task<CategoryDto?> UpdateAsync(int id, CategoryDto dto)
    {
       var element = await repo.GetByIdAsync(id);
       
       if (element is null)
       {
           return null;
       }
       
       element.Name = dto.Name;
       element.Description = dto.Description;
       element.Color = dto.Color;
       
       var categoryUpdate = await repo.UpdateAsync(element);
       
       return ToDto(categoryUpdate);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var trueornot= await repo.DeleteAsync(id);

        if (!trueornot)
        {
            return false;
        }
        
        return true;    

    }

    private static CategoryDto ToDto(Category c) => new()
    {
        Id = c.Id, Name = c.Name, Description = c.Description, Color = c.Color
    };
    
}