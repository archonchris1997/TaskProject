using Microsoft.EntityFrameworkCore;
using TaskProject.Data;
using TaskProject.Models;

namespace TaskProject.Repositories;
using Task = TaskProject.Models.Task;

public class CategoryRepository(TaskContext db) : ICategoryRepository
{
    
    public async Task<IEnumerable<Category>> GetAllAsync() => await db.Categories.ToListAsync();

    
    public async Task<Category?> GetByIdAsync(int id) => await db.Categories.FindAsync(id);
    

    public async Task<Category> CreateAsync(Category category)
    {
        
        db.Categories.Add(category);
        await db.SaveChangesAsync();
        return category;
        
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        db.Categories.Update(category);
        await db.SaveChangesAsync();
        return category;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await db.Categories.FindAsync(id);
        
        if (entity is null) return false;
        
        db.Categories.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        
        return await db.Categories.AnyAsync(x => x.Id == id);
        
    }
}