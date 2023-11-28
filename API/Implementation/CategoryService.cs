using API.Abstractions;
using DAL.Abstractions;
using DAL.Entities;
using DTOs;
using Microsoft.EntityFrameworkCore;

namespace API.Implementation;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category, int> _categoryRepository;
    private readonly IBookVolumeService _bookVolumeService;

    public CategoryService(IRepository<Category, int> categoryRepository, IBookVolumeService bookVolumeService)
    {
        _categoryRepository = categoryRepository;
        _bookVolumeService = bookVolumeService;
    }

    public async Task<CategoryDTO> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.Find((s => s.Id == id), includeProperties: "BookVolumes").SingleOrDefaultAsync();

        var categoryDTO = new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name
        };
        var bookVolumeIDs = category.BookVolumes.Select(x => x.GoogleVolumeID).ToList();
        categoryDTO.GoogleBookVolumes = await _bookVolumeService.GetByIDsAsync(bookVolumeIDs);

        return categoryDTO;
    }

    public async Task<List<CategoryDTO>> GetAllAsync()
    {
        return await _categoryRepository.Entities.Select(c => new CategoryDTO
        {
            Id = c.Id,
            Name = c.Name
        }).ToListAsync();
    }

    public async Task<int> CreateAsync(CategoryCreateDTO categoryCreateDTO)
    {
        var categoryWithTheSameName = _categoryRepository.Find((r => r.Name == categoryCreateDTO.Name)).FirstOrDefault();

        if (categoryWithTheSameName is not null)
        {
            throw new Exception($"Category with Name [{categoryCreateDTO.Name}] already exists!");
        }

        var newCategory = new Category
        {
            Name = categoryCreateDTO.Name
        };

        if (categoryCreateDTO.GoogleVolumesIDs?.Any() != null)
        {
            var volumes = categoryCreateDTO.GoogleVolumesIDs.Select(id => new BookVolume
            {
                GoogleVolumeID = id,
            });

            newCategory.BookVolumes.AddRange(volumes);
        }

        await _categoryRepository.AddAsync(newCategory);

        await _categoryRepository.SaveChangesAsync();

        return newCategory.Id;
    }

    public async Task<int> UpdateAsync(CategoryDTO categoryDTO)
    {
        var oldCategory = await _categoryRepository.Find((s => s.Id == categoryDTO.Id), includeProperties: "BookVolumes").SingleOrDefaultAsync();

        var existingCategoryWithSameName = _categoryRepository.Find((r => r.Id != categoryDTO.Id && r.Name == categoryDTO.Name)).FirstOrDefault();
        if (existingCategoryWithSameName is not null)
        {
            throw new Exception($"Category with Name [{categoryDTO.Name}] already exists!");
        }

        oldCategory.Name = categoryDTO.Name;
        if (categoryDTO.GoogleBookVolumes?.Any() == true)
        {
            oldCategory.BookVolumes = categoryDTO.GoogleBookVolumes.Select(gbv => new BookVolume
            {
                GoogleVolumeID = gbv.Id
            }).ToList();
        }
        else
        {
            oldCategory.BookVolumes.Clear();
        }


        _categoryRepository.Update(oldCategory);

        await _categoryRepository.SaveChangesAsync();

        return oldCategory.Id;
    }

    public async Task DeleteAsync(int categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);

        if (category is null)
        {
            throw new Exception($"Category with ID [{categoryId}] was not found!");
        }

        await _categoryRepository.RemoveByIdAsync(categoryId);

        await _categoryRepository.SaveChangesAsync();
    }
}
