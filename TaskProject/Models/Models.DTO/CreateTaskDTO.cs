namespace TaskProject.Models.Models.DTO;

public class CreateTaskDTO
{
    
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public int CategoryId { get; set; }
    
}