using Common.DTOs;

namespace Services.Interfaces;

public interface IPokemonCategoryService
{
    Task<List<PokemonCategoryDto>> GetPokemonByCategory(int categoryId);
}
