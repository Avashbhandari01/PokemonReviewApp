using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Net;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryRepository;

    public CategoryController(ICategoryService categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<ResponseDto<List<CategoryDto>>> GetCategories()
    {
        try
        {
            var result = new List<CategoryDto>();

            var categories = await _categoryRepository.GetAllCategories();

            foreach (var category in categories)
            {
                result.Add(new CategoryDto()
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }

            return new ResponseDto<List<CategoryDto>>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Categories retrived successfully!",
                ResponseData = result
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<List<CategoryDto>>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpGet("{categoryId}")]
    public async Task<ResponseDto<CategoryDto>> GetCateogry(int categoryId)
    {
        try
        {
            var category = await _categoryRepository.GetCategoryId(categoryId);

            if (category == null)
            {
                return new ResponseDto<CategoryDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Category not found!"
                };
            }

            return new ResponseDto<CategoryDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Category retrived successfully!",
                ResponseData = category
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<CategoryDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpPut("update-category")]
    public async Task<ResponseDto<CategoryDto>> UpdateCategory(CategoryDto updatedCategory)
    {
        try
        {
            if (updatedCategory == null)
            {
                return new ResponseDto<CategoryDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Category object is null!"
                };
            }

            var categoryToUpdate = await _categoryRepository.GetCategoryId(updatedCategory.Id);

            if (categoryToUpdate == null)
            {
                return new ResponseDto<CategoryDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Category not found!"
                };
            }

            categoryToUpdate.Name = updatedCategory.Name;

            await _categoryRepository.UpdateCategory(categoryToUpdate);

            return new ResponseDto<CategoryDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Category updated successfully!",
                ResponseData = categoryToUpdate
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<CategoryDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpPost]
    public async Task<ResponseDto<CategoryDto>> CreateCategory([FromBody] CategoryDto categoryCreate)
    {
        try
        {
            if (categoryCreate == null)
            {
                return new ResponseDto<CategoryDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Category object is null!"
                };
            }

            var categories = await _categoryRepository.GetAllCategories();

            var category = categories
                .Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (category != null)
            {
                return new ResponseDto<CategoryDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Category already exists!"
                };
            }

            var categoryToAdd = new Category()
            {
                Name = categoryCreate.Name
            };

            if (!await _categoryRepository.CreateCategory(categoryToAdd))
            {
                return new ResponseDto<CategoryDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Something went wrong while creating category!"
                };
            }

            return new ResponseDto<CategoryDto>()
            {
                StatusCode = HttpStatusCode.Created,
                StatusMessage = "Category created successfully!",
                ResponseData = categoryCreate
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<CategoryDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpDelete("{categoryId}")]
    public ResponseDto<CategoryDto> DeleteCategory(int categoryId)
    {
        try
        {
            var categoryToDelete = _categoryRepository.GetCategoryId(categoryId);

            if (categoryToDelete == null)
            {
                return new ResponseDto<CategoryDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Category not found!"
                };
            }

            _categoryRepository.DeleteCategory(categoryId);

            return new ResponseDto<CategoryDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Category deleted successfully!",
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<CategoryDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }
}
