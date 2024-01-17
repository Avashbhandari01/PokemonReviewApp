using Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using Services.Interfaces;
using System.Net;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonOwnerController : Controller
{
    private readonly IPokemonOwnerService _pokemonOwnerRepository;

    public PokemonOwnerController(IPokemonOwnerService pokemonOwnerRepository)
    {
        _pokemonOwnerRepository = pokemonOwnerRepository;
    }

    [HttpGet("{ownerId}/pokemon")]
    public async Task<ResponseDto<List<PokemonOwnerDto>>> GetPokemonByOwner(int ownerId)
    {
        try
        {
            var owner = await _pokemonOwnerRepository.GetPokemonByOwner(ownerId);

            if (owner == null)
            {
                return new ResponseDto<List<PokemonOwnerDto>>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Owner not found!"
                };
            }

            return new ResponseDto<List<PokemonOwnerDto>>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Pokemon retrived successfully!",
                ResponseData = owner
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<List<PokemonOwnerDto>>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }
}
