using DataAccess.Repositories.GenericRepository;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryService : ICountryService
    {
        private readonly IGenericRepository<Country> _genericRepository;

        public CountryService(IGenericRepository<Country> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<bool> CreateCountry(Country country)
        {
            return await _genericRepository.AddAsync(country);
        }

        public async Task<List<CountryDto>> GetAllCountries()
        {
            var countries = await _genericRepository.GetAllAsync();

            return countries.Select(c => new CountryDto()
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }

        public async Task<CountryDto> GetCountryId(int countryId)
        {
            var country = await _genericRepository.GetByIDAsync<Country>(countryId);

            if (country == null)
            {
                return null;
            }

            return new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };
        }

        public async Task UpdateCountry(CountryDto country)
        {
            var newCountry = new Country()
            {
                Id = country.Id,
                Name = country.Name
            };

            await _genericRepository.UpdateAsync(newCountry);
        }

        public void DeleteCountry(int countryId)
        {
            _genericRepository.Delete<Country>(countryId);
        }
    }
}
