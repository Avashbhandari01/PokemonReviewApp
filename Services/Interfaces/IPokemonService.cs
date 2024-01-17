using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Interfaces;

public interface IPokemonService
{
    Task<bool> CreatePokemon(PokemonDto pokemon);

    Task<List<PokemonDto>> GetAllPokemons();

    Task<PokemonDto> GetPokemonId(int id);

    Task UpdatePokemon(PokemonDto pokemon);

    void DeletePokemon(int pokeId);
}
