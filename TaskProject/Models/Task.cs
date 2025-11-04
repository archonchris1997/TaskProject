namespace TaskProject.Models;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    
    
    //Foreign Key;
    /*
            Cada *Tarefa* tem uma *Categoria*
            Uma *Categoria* tem *N Tarefas*
     */
    
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    
    // 🔐 per-user ownership
    public string UserId { get; set; } = string.Empty;
    
}

public enum TaskPriority
{
    Low = 1,
    Medium = 2,
    High = 3
}