using TP6.DTO;
namespace TP6.Services.ServiceContracts;
public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
    Task<CategoryDTO> GetCategoryByIdAsync(int id);
    Task<CategoryDTO> AddCategoryAsync(CategoryDTO dto);
    Task<CategoryDTO> UpdateCategoryAsync(int id, CategoryDTO dto);
    Task<bool> DeleteCategoryAsync(int id);
}
