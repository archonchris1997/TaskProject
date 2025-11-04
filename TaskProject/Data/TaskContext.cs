using Microsoft.EntityFrameworkCore;
using TaskProject.Models;
              
using TaskEntity = TaskProject.Models.Task;


namespace TaskProject.Data;

public class TaskContext(DbContextOptions<TaskContext> options) : DbContext(options)
{
    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired();
            e.Property(x => x.Description).IsRequired();
            e.HasOne(t => t.Category)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.Property(x => x.Description).HasMaxLength(500);
            e.Property(x => x.Color).HasMaxLength(7).IsRequired();

        });
        
         // Criar primeiros elementos

         modelBuilder.Entity<Category>().HasData(
             new Category { Id = 1, Name = "Work", Description = "Work-related tasks", Color = "#FF6B6B" },
             new Category { Id = 2, Name = "Personal", Description = "Personal tasks", Color = "#4ECDC4" },
             new Category { Id = 3, Name = "Shopping", Description = "Shopping list items", Color = "#45B7D1" }
         );

    }
}