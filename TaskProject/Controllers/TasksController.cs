using Microsoft.AspNetCore.Mvc;
using TaskProject.Models.Models.DTO;
using TaskProject.Services;

namespace TaskProject.Controllers;

// “This controller is part of a Web API — not a web page.”
[ApiController]
/*
 This attribute tells ASP.NET Core what URL should reach your controller.
   
   "api/" → a fixed prefix for your API endpoints
   "[controller]" → automatically replaced by your controller name (minus “Controller”)
*/
[Route("api/[controller]")]
public class TasksController(ITaskService service): ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TasksDTO>>> GetAll()
    {
        return Ok(await service.GetAllTasksAsync());

    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TasksDTO>> Get(int id)
    {
        var task = await service.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        
        return Ok(task);
    }
    
    //GetByCategory
    [HttpGet("category/{categoryId:int}")]
    public async  Task<ActionResult<IEnumerable<TasksDTO>>>GetByCategory(int categoryId)
    {
        var tasks = await service.GetTasksByCategoryAsync(categoryId);
        
        if (tasks == null || !tasks.Any())
        {
            return NotFound("No tasks found for this category.");
        }

        return Ok(tasks);
    }

    // observa como tem aqui o  *CreateTaskDTO* 
    [HttpPost]
    public async Task<ActionResult<TasksDTO>> Create(CreateTaskDTO task)
    {
        var varCreated= await service.CreateTaskAsync(task);

        if (varCreated == null)
        {
            return NotFound();
        }
        
        return CreatedAtAction(nameof(Get), new { id = varCreated.Id }, varCreated);
        
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TasksDTO>> Update(int id, UpdateTaskDto task)
    {
         var varUpdated = await service.UpdateTaskAsync(id, task);
         
         if (varUpdated == null)
         {
             return NotFound();
         }
         
         return Ok(varUpdated);
        
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deletedElement = await service.DeleteTaskAsync(id);

        if (!deletedElement)
        {
            return NotFound();
        }
        
        return NoContent();
    }
    
    
}