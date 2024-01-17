using Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using Services.Interfaces;
using System.Net;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonCategoryController : Controller
{
    private readonly IPokemonCategoryService _pokemonCategoryRepository;

    public PokemonCategoryController(IPokemonCategoryService pokemonCategoryRepository)
    {
        _pokemonCategoryRepository = pokemonCategoryRepository;
    }

    [HttpGet("{categoryId}")]
    public async Task<ResponseDto<List<PokemonCategoryDto>>> GetPokemonByCategory(int categoryId)
    {
        try
        {
            var pokemon = await _pokemonCategoryRepository.GetPokemonByCategory(categoryId);

            if (pokemon == null)
            {
                return new ResponseDto<List<PokemonCategoryDto>>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Pokemon not found!"
                };
            }

            return new ResponseDto<List<PokemonCategoryDto>>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Pokemon retrived successfully!",
                ResponseData = pokemon
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<List<PokemonCategoryDto>>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }
}
