/*using TP6.Data;
using TP6.DTO;
using Microsoft.EntityFrameworkCore;
using TP6.Models;
using TP6.Services.ServiceContracts;
namespace TP6.Services;
public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync() =>
        await _context.Categories.Select(c => new CategoryDTO
        {
            Name = c.Name,
            Description = c.Description
        }).ToListAsync();

    public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        return category == null ? null : new CategoryDTO
        {
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task<CategoryDTO> AddCategoryAsync(CategoryDTO dto)
    {
        var category = new Category { Name = dto.Name, Description = dto.Description };
        //the id is generated automatically?
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return dto;
    }

    public async Task<CategoryDTO> UpdateCategoryAsync(int id, CategoryDTO dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return null;

        category.Name = dto.Name;
        category.Description = dto.Description;
        await _context.SaveChangesAsync();

        return dto;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return true;
    }
}
*/
using TP6.Data;
using TP6.DTO;
using Microsoft.EntityFrameworkCore;
using TP6.Models;
using TP6.Services.ServiceContracts;
using AutoMapper;

namespace TP6.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CategoryService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
    {
        var categories = await _context.Categories.ToListAsync();
        return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
    }

    public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        return category == null ? null : _mapper.Map<CategoryDTO>(category);
    }

    public async Task<CategoryDTO> AddCategoryAsync(CategoryDTO dto)
    {
        var category = _mapper.Map<Category>(dto);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return _mapper.Map<CategoryDTO>(category);
    }

    public async Task<CategoryDTO> UpdateCategoryAsync(int id, CategoryDTO dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return null;

        _mapper.Map(dto, category); // Update category object with values from DTO
        await _context.SaveChangesAsync();

        return _mapper.Map<CategoryDTO>(category);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return true;
    }
}
