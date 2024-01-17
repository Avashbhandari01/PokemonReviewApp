using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Interfaces;

public interface IOwnerService
{
    Task<bool> CreateOwner(OwnerDto owner);

    Task<List<OwnerDto>> GetAllOwners();

    Task<OwnerDto> GetOwnerId(int ownerId);

    Task UpdateOwner(OwnerDto owner);

    void DeleteOwner(int ownerId);

    Task<List<CountryDto>> GetCountryByOwner(int ownerId);
}
