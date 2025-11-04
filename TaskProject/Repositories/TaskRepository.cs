
using Microsoft.EntityFrameworkCore;
using TaskProject.Data;
using Task = TaskProject.Models.Task;

namespace TaskProject.Repositories;


public class TaskRepository(TaskContext db) : ITaskRepository
{
    /*
        Mostra as Tarefas e inclui as *respectivas Categorias*  
       
     */
    
    public async Task<IEnumerable<Task>> GetAllAsync()
    {
        return await db.Tasks
            .Include(t => t.Category)
            .OrderByDescending(t => t.Id)
            .ToListAsync();
    }
    

    
    public async Task<Task?> GetByIdAsync(int id)
    {
        return await db.Tasks
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Task>> GetByCategoryAsync(int categoryId)
    {
        return await db.Tasks
            .Include(t => t.Category)
            .Where(t => t.CategoryId == categoryId)
            .ToListAsync();
    }


    /*
    public async Task<IEnumerable<Task>> GetByCategoryAsync(int categoryId)
    {
        return await db.Tasks
            .Include(t => t.Category)
            .Where(t => t.CategoryId == categoryId)
            .ToListAsync();
    } */

    
    //---------------------------------------------------------//---------------------------------------------------------
    
    public async Task<Task> CreateAsync(Task task)
    {
        db.Tasks.Add(task);
        await db.SaveChangesAsync();
        return task;
        
    }

    public async Task<Task> UpdateAsync(Task task)
    {
        db.Tasks.Update(task);
        await db.SaveChangesAsync();
        return task;
    }

    
    // In TaskRepository.cs
    // 🧩 DeleteAsync: returns true if deleted, false if not found
    public async Task<bool> DeleteAsync(int id)
    {
        var task = await db.Tasks.FindAsync(id);

        if (task == null)
            return false;

        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return true;
    }

    public Task<bool> ExistAsync(int id)
    {
        return db.Tasks.AnyAsync(t => t.Id == id);
        
    }
}