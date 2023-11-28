using API.Abstractions;
using DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        return Ok(category);
    }

    [HttpPost()]
    public async Task<IActionResult> Create(CategoryCreateDTO category)
    {
        var createdCategoryId = await _categoryService.CreateAsync(category);
        return Ok(createdCategoryId);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _categoryService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut()]
    public async Task<IActionResult> Update(CategoryDTO updatedCategory)
    {
        return Ok(await _categoryService.UpdateAsync(updatedCategory));
    }
}
