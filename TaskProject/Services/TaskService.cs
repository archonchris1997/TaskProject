using System.Security.Claims;
using TaskProject.Models;
using TaskProject.Models.Models.DTO;
using TaskProject.Repositories;
using Task = TaskProject.Models.Task;

namespace TaskProject.Services;

public class TaskService (ITaskRepository repo, IHttpContextAccessor http): ITaskService
{
    private string? CurrentUserId
    {
        get
        {
            var context = http.HttpContext;
            var user = context?.User;
            
            // Retorna o *Nome* do *User*
            return user?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
 
    
    public async Task<IEnumerable<TasksDTO>> GetAllTasksAsync()
    {
        var tasks = await repo.GetAllAsync();
        //Converte a Lista de Tasks em Lista de DTO's
        return tasks.Select(ToDto);
    }

    public async Task<TasksDTO?> GetTaskByIdAsync(int id)
    {
        var task = await repo.GetByIdAsync(id);
        
        if (task is not null)
            return ToDto(task);
        
        return null;
    }

    public async Task<IEnumerable<TasksDTO>> GetTasksByCategoryAsync(int categoryId)
    {
        var tasks = await repo.GetByCategoryAsync(categoryId);
        return tasks.Select(ToDto);
        
    }

    public async Task<TasksDTO> CreateTaskAsync(CreateTaskDTO dto)
    {
        var entity = new Task
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            Priority = dto.Priority,
            CategoryId = dto.CategoryId
        };

        await repo.CreateAsync(entity);
        
        // Vamos verificar se foi criada;
        var taskCriada = await repo.GetByIdAsync(entity.Id);
        
        /*
         
         Se, por alguma razão, o repositório não conseguiu ir buscar a tarefa criada,
           (por exemplo, se algo correu mal na base de dados),
           então simplesmente usas a entity original (que já tem os dados que inseriste).
         
         */
        if (taskCriada is null)
        {
            taskCriada = entity;
        }
        return ToDto(taskCriada);
    }

    public async Task<TasksDTO?> UpdateTaskAsync(int id, UpdateTaskDto dto)
    {
        var task = await repo.GetByIdAsync(id);
        if (task is null)
        {
            return null;
        }
        
        // dto é a nova Task *Updated*
        // verifica se a *Task Updated* tem *todos os campos*
        if (dto.Title is not null) task.Title = dto.Title;
        if (dto.Description is not null) task.Description = dto.Description;
        if (dto.DueDate.HasValue) task.DueDate = dto.DueDate;
        if (dto.IsCompleted.HasValue) task.IsCompleted = dto.IsCompleted.Value;
        if (dto.Priority.HasValue) task.Priority = dto.Priority.Value;
        if (dto.CategoryId.HasValue) task.CategoryId = dto.CategoryId.Value;
        
        //---------------------------------------------------
        
        // Espera pelo Repositório para fazer um Update
        await repo.UpdateAsync(task);
        
        // Vamos obter *novamente* a Task através do *id* para *verificar* se foi *updated*
        var taskUpdated = await repo.GetByIdAsync(id);
        
        //Ser por alguma razão ela não foi criada então fica a Task original que já tinhas originalmente
        if (taskUpdated is null)
        {
            taskUpdated = task;
        }
        
        return ToDto(taskUpdated);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var entity = await repo.GetByIdAsync(id);
        
        if (entity is null)
        {
            return false;
        }

        await repo.DeleteAsync(id);
        
        
        return true;
    }
    
    private static TasksDTO ToDto(Task t)
    {
        return new TasksDTO
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            CreatedAt = t.CreatedAt,
            DueDate = t.DueDate,
            IsCompleted = t.IsCompleted,
            Priority = t.Priority,
            CategoryId = t.CategoryId,
            CategoryName = t.Category?.Name ?? ""
        };
    }
}