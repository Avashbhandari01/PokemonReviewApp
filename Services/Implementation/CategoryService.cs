using DataAccess.Repositories.GenericRepository;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _genericRepository;

        public CategoryService(IGenericRepository<Category> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<bool> CreateCategory(Category category)
        {
            return await _genericRepository.AddAsync(category);
        }

        public async Task<List<CategoryDto>> GetAllCategories()
        {
            var categories = await _genericRepository.GetAllAsync();

            return categories.Select(c => new CategoryDto()
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }

        public async Task<CategoryDto> GetCategoryId(int categoryId)
        {
            var category = await _genericRepository.GetByIDAsync<Category>(categoryId);

            if (category == null)
            {
                return null;
            }

            return new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task UpdateCategory(CategoryDto category)
        {
            var newCategory = new Category()
            {
                Id = category.Id,
                Name = category.Name
            };

            await _genericRepository.UpdateAsync(newCategory);
        }

        public void DeleteCategory(int categoryId)
        {
             _genericRepository.Delete<Category>(categoryId);
        }
    }
}
