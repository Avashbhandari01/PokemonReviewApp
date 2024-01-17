using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using System.Net;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OwnerController : Controller
{
    private readonly IOwnerService _ownerRepository;

    public OwnerController(IOwnerService ownerRepository)
    {
        _ownerRepository = ownerRepository;
    }

    [HttpGet]
    public async Task<ResponseDto<List<OwnerDto>>> GetOwners()
    {
        try
        {
            var result = new List<OwnerDto>();

            var owners = await _ownerRepository.GetAllOwners();

            foreach (var owner in owners)
            {
                result.Add(new OwnerDto()
                {
                    Id = owner.Id,
                    FirstName = owner.FirstName,
                    LastName = owner.LastName,
                    Gym = owner.Gym,
                });
            }

            return new ResponseDto<List<OwnerDto>>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Owners retrived successfully!",
                ResponseData = owners
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<List<OwnerDto>>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpGet("{ownerId}")]
    public async Task<ResponseDto<OwnerDto>> GetOwner(int ownerId)
    {
        try
        {
            var owner = await _ownerRepository.GetOwnerId(ownerId);

            if (owner == null)
            {
                return new ResponseDto<OwnerDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Owner not found!"
                };
            }

            return new ResponseDto<OwnerDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Owner retrived successfully!",
                ResponseData = owner
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<OwnerDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpPost]
    public async Task<ResponseDto<OwnerDto>> CreateOwner([FromBody] OwnerDto ownerCreate)
    {
        try
        {
            if (ownerCreate == null)
            {
                return new ResponseDto<OwnerDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Owner object is null!"
                };
            }

            var owners = await _ownerRepository.GetAllOwners();

            var owner = owners.Where(c => c.FirstName.Trim().ToUpper() == ownerCreate.FirstName.TrimEnd().ToUpper()).FirstOrDefault();

            if (owner != null)
            {
                return new ResponseDto<OwnerDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Owner already exists!"
                };
            }

            var ownerToAdd = new OwnerDto()
            {
                FirstName = ownerCreate.FirstName,
                LastName = ownerCreate.LastName,
                Gym = ownerCreate.Gym,
                CountryId = ownerCreate.CountryId
            };

            if (!await _ownerRepository.CreateOwner(ownerToAdd))
            {
                return new ResponseDto<OwnerDto>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusMessage = "Something went wrong while creating owner!"
                };
            }

            return new ResponseDto<OwnerDto>
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Owner created successfully!",
                ResponseData = ownerCreate
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<OwnerDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpPut("update-owner")]
    public async Task<ResponseDto<OwnerDto>> UpdateOwner(OwnerDto updatedOwner)
    {
        try
        {
            if (updatedOwner == null)
            {
                return new ResponseDto<OwnerDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Owner object is null!"
                };
            }

            var ownerToUpate = await _ownerRepository.GetOwnerId(updatedOwner.Id);

            if (ownerToUpate == null)
            {
                return new ResponseDto<OwnerDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Owner not found!"
                };
            }

            ownerToUpate.FirstName = updatedOwner.FirstName;

            ownerToUpate.LastName = updatedOwner.LastName;

            ownerToUpate.Gym = updatedOwner.Gym;

            ownerToUpate.CountryId = updatedOwner.CountryId;

            await _ownerRepository.UpdateOwner(ownerToUpate);

            return new ResponseDto<OwnerDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Owner updated successfully!",
                ResponseData = updatedOwner
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<OwnerDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpDelete("{ownerId}")]
    public ResponseDto<OwnerDto> DeleteOwner(int ownerId)
    {
        try
        {
            var owner = _ownerRepository.GetOwnerId(ownerId);

            if (owner == null)
            {
                return new ResponseDto<OwnerDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Owner not found!"
                };
            }

            _ownerRepository.DeleteOwner(ownerId);

            return new ResponseDto<OwnerDto>
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Owner deleted successfully!"
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<OwnerDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpGet("/CountryByOwners/{ownerId}")]
    public async Task<ResponseDto<List<CountryDto>>> GetCountryByOwner(int ownerId)
    {
        try
        {
            var country = await _ownerRepository.GetCountryByOwner(ownerId);

            if (country == null)
            {
                return new ResponseDto<List<CountryDto>>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Country not found!"
                };
            }

            return new ResponseDto<List<CountryDto>>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Country retrived successfully!",
                ResponseData = country
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
}
