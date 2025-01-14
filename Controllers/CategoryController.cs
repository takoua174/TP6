using Microsoft.AspNetCore.Mvc;
//using TP6.Models;
using TP6.DTO;
using Microsoft.AspNetCore.Authorization;
using TP6.Services;
using TP6.Services.ServiceContracts;
namespace TP6.Controllers;
[Route("api/[controller]")]
[ApiController]
public class Categorycontroller : ControllerBase
{
    private readonly ICategoryService _service;

    public Categorycontroller(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories() =>
            Ok(await _service.GetAllCategoriesAsync());


    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id) =>
            Ok(await _service.GetCategoryByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO dto) =>
        Ok(await _service.AddCategoryAsync(dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDTO dto) =>
        Ok(await _service.UpdateCategoryAsync(id, dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id) =>
        Ok(await _service.DeleteCategoryAsync(id));
}
//understand this
//    [Authorize(Roles = "admin,customer")]
