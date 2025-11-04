using Microsoft.AspNetCore.Mvc;
using TaskProject.Models.Models.DTO;
using TaskProject.Services;

namespace TaskProject.Controllers;


[ApiController]
[Route("api/[controller]")]

public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoriesController(ICategoryService service)
    {
        _service = service;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        var categories = await _service.GetAllAsync();
        return Ok(categories);
    }

    // GET: api/categories/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto>> Get(int id)
    {
        var category = await _service.GetByIdAsync(id);
        if (category == null)
            return NotFound();

        return Ok(category);
    }

    // POST: api/categories
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CategoryDto dto)
    {
        if (dto == null)
            return BadRequest("Category data is required.");

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    // PUT: api/categories/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryDto>> Update(int id, [FromBody] CategoryDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        if (updated == null)
            return NotFound();

        return Ok(updated);
    }

    // DELETE: api/categories/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}