using DataAccess.Repositories.GenericRepository;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class PokemonService : IPokemonService
{
    private readonly IGenericRepository<Pokemon> _genericRepository;

    public PokemonService(IGenericRepository<Pokemon> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task<bool> CreatePokemon(PokemonDto pokemon)
    {
        var pokemonEntity = new Pokemon()
        {
            Name = pokemon.Name,
            BirthDate = pokemon.BirthDate,
            PokemonOwners = new List<PokemonOwner>()
            {
                new PokemonOwner()
                {
                    PokemonId = pokemon.Id,
                    OwnerId = pokemon.OwnerId
                }
            },
            PokemonCategories = new List<PokemonCategory>()
            {
                new PokemonCategory()
                {
                    PokemonId = pokemon.Id,
                    CategoryId = pokemon.CategoryId
                }
            }
        };

        return await _genericRepository.AddAsync(pokemonEntity);
    }

    public async Task<List<PokemonDto>> GetAllPokemons()
    {
        try
        {
            var pokemon = await _genericRepository.GetAllAsync();

            return pokemon.Select(p => new PokemonDto()
            {
                Id = p.Id,
                Name = p.Name,
                BirthDate = p.BirthDate,
            }).ToList();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<PokemonDto> GetPokemonId(int id)
    {
        var pokemon = await _genericRepository.GetByIDAsync<Pokemon>(id);

        return new PokemonDto()
        {
            Id = pokemon.Id,
            Name = pokemon.Name,
            BirthDate = pokemon.BirthDate,
        };
    }

    public async Task UpdatePokemon(PokemonDto pokemon)
    {
        var newPokemon = new Pokemon()
        {
            Id = pokemon.Id,
            Name = pokemon.Name,
            BirthDate = pokemon.BirthDate,
            PokemonOwners = new List<PokemonOwner>()
            {
                new PokemonOwner()
                {
                    OwnerId = pokemon.OwnerId
                }
            },
            PokemonCategories = new List<PokemonCategory>()
            {
                new PokemonCategory()
                {
                    CategoryId = pokemon.CategoryId
                }
            }
        };

        await _genericRepository.UpdateAsync(newPokemon);
    }

    public void DeletePokemon(int pokeId)
    {
        _genericRepository.Delete<Owner>(pokeId);
    }
}

