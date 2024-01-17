using Common.DTOs;
using DataAccess.Repositories.GenericRepository;
using PokemonReviewApp.Models;
using Services.Interfaces;

namespace Services.Repository;

public class PokemonCategoryService : IPokemonCategoryService
{
    private readonly IGenericRepository<PokemonCategory> _genericRepository;

    public PokemonCategoryService(IGenericRepository<PokemonCategory> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task<List<PokemonCategoryDto>> GetPokemonByCategory(int categoryId)
    {
        var categories = await _genericRepository.GetAllAsync();

        if (categories == null)
        {
            return new List<PokemonCategoryDto>();
        }
        
        var pokemon = new List<PokemonCategoryDto>();

        foreach (var category in categories)
        {
            if (category.CategoryId == categoryId)
            {
                var pokemonToShow = await _genericRepository.GetByIDAsync<Pokemon>(category.PokemonId);

                if (pokemonToShow != null)
                {
                    pokemon.Add(new PokemonCategoryDto()
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
