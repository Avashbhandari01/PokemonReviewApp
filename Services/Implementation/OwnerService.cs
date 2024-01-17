using DataAccess.Repositories.GenericRepository;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class OwnerService : IOwnerService
{
    private readonly IGenericRepository<Owner> _genericRepository;

    public OwnerService(IGenericRepository<Owner> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task<bool> CreateOwner(OwnerDto owner)
    {
        var country = await _genericRepository.GetByIDAsync<Country>(owner.CountryId);

        if (country == null)
        {
            return false;
        }

        var newOwner = new Owner()
        {
            FirstName = owner.FirstName,
            LastName = owner.LastName,
            Gym = owner.Gym,
            CountryId = owner.CountryId
        };

        return await _genericRepository.AddAsync(newOwner);
    }

    public async Task<List<OwnerDto>> GetAllOwners()
    {
        var owners = await _genericRepository.GetAllAsync();

        return owners.Select(o => new OwnerDto()
        {
            Id = o.Id,
            FirstName = o.FirstName,
            LastName = o.LastName,
            Gym = o.Gym,
            CountryId = o.CountryId
        }).ToList();
    }

    public async Task<OwnerDto> GetOwnerId(int ownerId)
    {
        var owner = await _genericRepository.GetByIDAsync<Owner>(ownerId);

        if (owner == null)
        {
            return null;
        }

        return new OwnerDto()
        {
            Id = owner.Id,
            FirstName = owner.FirstName,
            LastName = owner.LastName,
            Gym = owner.Gym,
            CountryId = owner.CountryId
        };
    }

    public async Task UpdateOwner(OwnerDto owner)
    {
        var newOwner = new Owner()
        {
            Id = owner.Id,
            FirstName = owner.FirstName,
            LastName = owner.LastName,
            Gym = owner.Gym,
            CountryId = owner.CountryId
        };

        await _genericRepository.UpdateAsync(newOwner);
    }

    public void DeleteOwner(int ownerId)
    {
        _genericRepository.Delete<Owner>(ownerId);
    }

    public async Task<List<CountryDto>> GetCountryByOwner(int ownerId)
    {
        var owners = await _genericRepository.GetAllAsync();

        var country = new List<CountryDto>();

        foreach (var owner in owners)
        {
            if (owner.Id == ownerId)
            {
                var countryToShow = await _genericRepository.GetByIDAsync<Country>(owner.CountryId);

                if (countryToShow != null)
                {
                    country.Add(new CountryDto()
                    {
                        Name = countryToShow.Name
                    });
                }
            } 
        }

        if (country.Count == 0)
        {
            return null;
        }

        return country;
    }
}
