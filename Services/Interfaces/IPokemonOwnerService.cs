using Common.DTOs;

namespace Services.Interfaces;

public interface IPokemonOwnerService
{
    Task<List<PokemonOwnerDto>> GetPokemonByOwner(int ownerId);
}
