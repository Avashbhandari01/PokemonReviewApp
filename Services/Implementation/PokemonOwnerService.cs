using Common.DTOs;
using DataAccess.Repositories.GenericRepository;
using PokemonReviewApp.Models;
using Services.Interfaces;

namespace Services.Repository;

public class PokemonOwnerService : IPokemonOwnerService
{
    private readonly IGenericRepository<PokemonOwner> _genericRepository;
    public PokemonOwnerService(IGenericRepository<PokemonOwner> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task<List<PokemonOwnerDto>> GetPokemonByOwner(int ownerId)
    {
        var owners = await _genericRepository.GetAllAsync();

        if (owners == null)
        {
            return new List<PokemonOwnerDto>();
        }

        var pokemon = new List<PokemonOwnerDto>();

        foreach (var owner in owners)
        {
            if (owner.OwnerId == ownerId)
            {
                var pokemonToShow = await _genericRepository.GetByIDAsync<Pokemon>(owner.PokemonId);

                if (pokemonToShow != null)
                {
                    pokemon.Add(new PokemonOwnerDto()
                    {
                        Name = pokemonToShow.Name,
                        BirthDate = pokemonToShow.BirthDate,
                    });
                }
            }
        }

        if (pokemon.Count == 0)
        {
            return null;
        }

        return pokemon;
    }
}
