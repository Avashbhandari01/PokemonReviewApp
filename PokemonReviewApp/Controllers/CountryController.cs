using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Net;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController : Controller
{
    private readonly ICountryService _countryRepository;

    public CountryController(ICountryService countryRepository)
    {
        _countryRepository = countryRepository;
    }

    [HttpGet]
    public async Task<ResponseDto<List<CountryDto>>> GetCountries()
    {
        try
        {
            var result = new List<CountryDto>();

            var countries = await _countryRepository.GetAllCountries();

            foreach (var country in countries)
            {
                result.Add(new CountryDto()
                {
                    Id = country.Id,
                    Name = country.Name
                });
            }

            return new ResponseDto<List<CountryDto>>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Countries retrived successfully!",
                ResponseData = countries
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<List<CountryDto>>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpGet("{countryId}")]
    public async Task<ResponseDto<CountryDto>> GetCountry(int countryId)
    {
        try
        {
            var country = await _countryRepository.GetCountryId(countryId);

            if (country == null)
            {
                return new ResponseDto<CountryDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Country not found!"
                };
            }

            return new ResponseDto<CountryDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Country retrived successfully!",
                ResponseData = country
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<CountryDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpPost]
    public async Task<ResponseDto<CountryDto>> CreateCountry([FromBody] CountryDto countryCreate)
    {
        try
        {
            if (countryCreate == null)
            {
                return new ResponseDto<CountryDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Country object is null!"
                };
            }

            var countries = await _countryRepository.GetAllCountries();

            var country = countries
                .Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (country != null)
            {
                return new ResponseDto<CountryDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Country already exists!"
                };
            }

            var countryToAdd = new Country()
            {
                Name = countryCreate.Name
            };

            if (!await _countryRepository.CreateCountry(countryToAdd))
            {
                return new ResponseDto<CountryDto>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusMessage = "Something went wrong while creating country!"
                };
            }

            return new ResponseDto<CountryDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Country created successfully!",
                ResponseData = countryCreate
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<CountryDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpPut("update-country")]
    public async Task<ResponseDto<CountryDto>> UpdateCountry(CountryDto updatedCountry)
    {
        try
        {
            if (updatedCountry == null)
            {
                return new ResponseDto<CountryDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Country object is null!"
                };
            }

            var countryToUpate = await _countryRepository.GetCountryId(updatedCountry.Id);

            if (countryToUpate == null)
            {
                return new ResponseDto<CountryDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Country not found!"
                };
            }

            countryToUpate.Name = updatedCountry.Name;

            await _countryRepository.UpdateCountry(countryToUpate);

            return new ResponseDto<CountryDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Country updated successfully!",
                ResponseData = updatedCountry
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<CountryDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpDelete("{countryId}")]
    public ResponseDto<CountryDto> DeleteCountry(int countryId)
    {
        try
        {
            var country = _countryRepository.GetCountryId(countryId);

            if (country == null)
            {
                return new ResponseDto<CountryDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Country not found!"
                };
            }

            _countryRepository.DeleteCountry(countryId);

            return new ResponseDto<CountryDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Country deleted successfully!",
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<CountryDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }
}
