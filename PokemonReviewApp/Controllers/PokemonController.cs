using Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Net;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonController : Controller
{
    private readonly IPokemonService _pokemonRepository;
    private readonly IReviewService _reviewRepository;

    public PokemonController(IPokemonService pokemonRepository, IReviewService reviewRepository)
    {
        _pokemonRepository = pokemonRepository;
        _reviewRepository = reviewRepository;
    }

    [HttpGet]
    public async Task<ResponseDto<List<PokemonDto>>> GetPokemons()
    {
        try
        {
            var pokemons = await _pokemonRepository.GetAllPokemons();

            return new ResponseDto<List<PokemonDto>>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Pokemons retrived successfully!",
                ResponseData = pokemons
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<List<PokemonDto>>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpGet("{pokeId}")]
    public async Task<ResponseDto<PokemonDto>> GetPokemon(int pokeId)
    {
        try
        {
            var pokemon = await _pokemonRepository.GetPokemonId(pokeId);

            if (pokemon == null)
            {
                return new ResponseDto<PokemonDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Pokemon not found!"
                };
            }

            return new ResponseDto<PokemonDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Pokemon retrived successfully!",
                ResponseData = pokemon
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<PokemonDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpPost]
    public async Task<ResponseDto<PokemonDto>> CreatePokemon([FromBody] PokemonDto pokemonCreate)
    {
        try
        {
            if (pokemonCreate == null)
            {
                return new ResponseDto<PokemonDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Pokemon not found!"
                };
            }

            var pokemons = await _pokemonRepository.GetAllPokemons();

            var pokemon = pokemons.Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (pokemon != null)
            {
                return new ResponseDto<PokemonDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Pokemon already exists!"
                };
            }

            var pokemonToAdd = new PokemonDto()
            {
                Name = pokemonCreate.Name,
                BirthDate = pokemonCreate.BirthDate,
                CategoryId = pokemonCreate.CategoryId,
                OwnerId = pokemonCreate.OwnerId
            };

            if (!await _pokemonRepository.CreatePokemon(pokemonToAdd))
            {
                return new ResponseDto<PokemonDto>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusMessage = "Something went wrong while creating pokemon!"
                };
            }

            return new ResponseDto<PokemonDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Pokemon created successfully!",
                ResponseData = pokemonCreate
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<PokemonDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpPut("{pokeId}")]
    public async Task<ResponseDto<PokemonDto>> UpdatePokemon(PokemonDto updatedPokemon)
    {
        try
        {
            if (updatedPokemon == null)
            {
                return new ResponseDto<PokemonDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Pokemon object is null!"
                };
            }

            var pokemonToUpdate = await _pokemonRepository.GetPokemonId(updatedPokemon.Id);

            if (pokemonToUpdate == null)
            {
                return new ResponseDto<PokemonDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Pokemon not found!"
                };
            }

            pokemonToUpdate.Name = updatedPokemon.Name;

            pokemonToUpdate.BirthDate = updatedPokemon.BirthDate;

            pokemonToUpdate.CategoryId = updatedPokemon.CategoryId;

            pokemonToUpdate.OwnerId = updatedPokemon.OwnerId;

            await _pokemonRepository.UpdatePokemon(updatedPokemon);

            return new ResponseDto<PokemonDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Pokemon updated successfully!",
                ResponseData = updatedPokemon
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<PokemonDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpDelete("{pokeId}")]
    public ResponseDto<PokemonDto> DeletePokemon(int pokeId)
    {
        try
        {
            var pokemon = _pokemonRepository.GetPokemonId(pokeId);

            if (pokemon == null)
            {
                return new ResponseDto<PokemonDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Pokemon not found!"
                };
            }

            var reviewsToDelete = _reviewRepository.DeleteReviewFromPokemonId(pokeId);

            _pokemonRepository.DeletePokemon(pokeId);

            return new ResponseDto<PokemonDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Pokemon deleted successfully!"
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<PokemonDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }
}
