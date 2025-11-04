namespace TaskProject.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = "#0066CC"; // Hex color for UI
    
    // Navigation property
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
    
}