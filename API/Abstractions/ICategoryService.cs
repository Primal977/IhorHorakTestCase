using DTOs;

namespace API.Abstractions;

public interface ICategoryService
{
    public Task<CategoryDTO> GetByIdAsync(int id);
    public Task<List<CategoryDTO>> GetAllAsync();

    public Task<int> CreateAsync(CategoryCreateDTO categorCreateDTO);
    public Task<int> UpdateAsync(CategoryDTO categoryDTO);
    public Task DeleteAsync(int categoryId);
}
