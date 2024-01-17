using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces;

public interface ICategoryService
{
    Task<bool> CreateCategory(Category category);

    Task<List<CategoryDto>> GetAllCategories();

    Task<CategoryDto> GetCategoryId(int categoryId);

    Task UpdateCategory(CategoryDto category);

    void DeleteCategory(int categoryId); 
}
