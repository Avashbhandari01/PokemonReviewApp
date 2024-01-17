using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Net;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewerController : Controller
{
    private readonly IReviewerService _reviewerRepository;

    public ReviewerController(IReviewerService reviewerRepository)
    {
        _reviewerRepository = reviewerRepository;
    }

    [HttpGet]
    public async Task<ResponseDto<List<ReviewerDto>>> GetReviewers()
    {
        try
        {
            var reviewers = await _reviewerRepository.GetAllReviewers();  

            return new ResponseDto<List<ReviewerDto>>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Reviewers retrived successfully!",
                ResponseData = reviewers
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<List<ReviewerDto>>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message,
            };
        }
    }

    [HttpGet("{reviewerId}")]
    public async Task<ResponseDto<ReviewerDto>> GetReviewer(int reviewerId)
    {
        try
        {
            var reviewer = await _reviewerRepository.GetReviewerId(reviewerId);

            if (reviewer == null)
            {
                return new ResponseDto<ReviewerDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Reviewer not found!",
                };
            }

            return new ResponseDto<ReviewerDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Reviewer retrived successfully!",
                ResponseData = reviewer
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<ReviewerDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message,
            };
        }
    }

    [HttpGet("{reviewerId}/reviews")]
    public async Task<ResponseDto<ReviewDto>> GetReviewsByReviewer(int reviewerId)
    {
        try
        {
            var reviewer = await _reviewerRepository.GetReviewsByReviewer(reviewerId);

            if (reviewer == null)
            {
                return new ResponseDto<ReviewDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Reviewer not found!",
                };
            }

            return new ResponseDto<ReviewDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Reviews retrived successfully!",
                ResponseData = reviewer
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<ReviewDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message,
            };
        }
    }

    [HttpPost]
    public async Task<ResponseDto<ReviewerDto>> CreateReviewer([FromBody] ReviewerDto reviewerCreate)
    {
        try
        {
            if (reviewerCreate == null)
            {
                return new ResponseDto<ReviewerDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Reviewer object is null!",
                };
            }

            var reviewers = await _reviewerRepository.GetAllReviewers();

            var reviewer = reviewers.Where(c => c.FirstName.Trim().ToUpper() == reviewerCreate.FirstName.TrimEnd().ToUpper()).FirstOrDefault();

            if (reviewer != null)
            {
                return new ResponseDto<ReviewerDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Reviewer already exists!",
                };
            }

            var reviewerToAdd = new Reviewer()
            {
                FirstName = reviewerCreate.FirstName,
                LastName = reviewerCreate.LastName,
            };

            if (!await _reviewerRepository.CreateReviewer(reviewerToAdd))
            {
                return new ResponseDto<ReviewerDto>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusMessage = "Something went wrong creating reviewer!",
                };
            }

            return new ResponseDto<ReviewerDto>()
            {
                StatusCode = HttpStatusCode.Created,
                StatusMessage = "Reviewer created successfully!",
                ResponseData = reviewerCreate
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<ReviewerDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message,
            };
        }
    }

    [HttpPut("{reviewerId}")]
    public async Task<ResponseDto<ReviewerDto>> UpdateReviewer(ReviewerDto updatedReviewer)
    {
        try
        {
            if (updatedReviewer == null)
            {
                return new ResponseDto<ReviewerDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Reviewer object is null!",
                };
            }

            var reviewerToUpdate = await _reviewerRepository.GetReviewerId(updatedReviewer.Id);

            if (reviewerToUpdate == null)
            {
                return new ResponseDto<ReviewerDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Reviewer not found!",
                };
            }

            reviewerToUpdate.FirstName = updatedReviewer.FirstName;

            reviewerToUpdate.LastName = updatedReviewer.LastName;

            await _reviewerRepository.UpdateReviewer(reviewerToUpdate);

            return new ResponseDto<ReviewerDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Reviewer updated successfully!",
                ResponseData = reviewerToUpdate
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<ReviewerDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message,
            };
        }
    }

    [HttpDelete("{reviewerId}")]
    public ResponseDto<ReviewerDto> DeleteReviewer(int reviewerId)
    {
        try
        {
            var reviewerToDelete = _reviewerRepository.GetReviewerId(reviewerId);

            if (reviewerToDelete == null)
            {
                return new ResponseDto<ReviewerDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Reviewer not found!",
                };
            }

            _reviewerRepository.DeleteReviewer(reviewerId);

            return new ResponseDto<ReviewerDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Reviewer deleted successfully!",
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<ReviewerDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message,
            };
        }
    }
}
