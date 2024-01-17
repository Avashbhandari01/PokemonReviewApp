using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces;

public interface ICountryService
{
    Task<bool> CreateCountry(Country country);

    Task<List<CountryDto>> GetAllCountries();

    Task<CountryDto> GetCountryId(int countryId);

    Task UpdateCountry(CountryDto country);

    void DeleteCountry(int countryId);
}
