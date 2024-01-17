using Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Net;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : Controller
{
    private readonly IReviewService _reviewRepository;
    private readonly IPokemonService _pokemonRepository;

    public ReviewController(IReviewService reviewRepository, IPokemonService pokemonRepository)
    {
        _reviewRepository = reviewRepository;
        _pokemonRepository = pokemonRepository;
    }

    [HttpGet]
    public async Task<ResponseDto<List<ReviewDto>>> GetReviews()
    {
        try
        {
            var reviews = await _reviewRepository.GetAllReviews();

            return new ResponseDto<List<ReviewDto>>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Reviews retrived successfully!",
                ResponseData = reviews
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<List<ReviewDto>>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpGet("{reviewId}")]
    public async Task<ResponseDto<ReviewDto>> GetReview(int reviewId)
    {
        try
        {
            var review = await _reviewRepository.GetReviewId(reviewId);

            if (review == null)
            {
                return new ResponseDto<ReviewDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Review not found!"
                };
            }

            return new ResponseDto<ReviewDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Review retrived successfully!",
                ResponseData = review
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<ReviewDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpGet("pokemon/{pokeId}")]
    public async Task<ResponseDto<List<ReviewDto>>> GetReviewsOfAPokemon(int pokeId)
    {
        try
        {
            var reviews = await _reviewRepository.GetReviewsOfAPokemon(pokeId);

            if (reviews == null)
            {
                return new ResponseDto<List<ReviewDto>>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Reviews not found!"
                };
            }

            return new ResponseDto<List<ReviewDto>>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Reviews retrived successfully!",
                ResponseData = reviews
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<List<ReviewDto>>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpPost]
    public async Task<ResponseDto<ReviewDto>> CreateReview([FromBody] ReviewDto reviewCreate)
    {
        try
        {
            if (reviewCreate == null)
            {
                return new ResponseDto<ReviewDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Review object is null!"
                };
            }

            var reviews = await _reviewRepository.GetAllReviews();

            var review = reviews.Where(c => c.Title.Trim().ToUpper() == reviewCreate.Title.TrimEnd().ToUpper()).FirstOrDefault();

            if (review != null)
            {
                return new ResponseDto<ReviewDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Review already exists!"
                };
            }

            var reviewToAdd = new Review()
            {
                Title = reviewCreate.Title,
                Text = reviewCreate.Text,
                Rating = reviewCreate.Rating,
                ReviewerId = reviewCreate.ReviewerId,
                PokemonId = reviewCreate.PokemonId
            };

            if (!await _reviewRepository.CreateReview(reviewToAdd))
            {
                return new ResponseDto<ReviewDto>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusMessage = "Something went wrong creating review!"
                };
            }

            return new ResponseDto<ReviewDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Review created successfully!",
                ResponseData = reviewCreate
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<ReviewDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpPut("update-review")]
    public async Task<ResponseDto<ReviewDto>> UpdateReview(ReviewDto updatedReview)
    {
        try
        {
            if (updatedReview == null)
            {
                return new ResponseDto<ReviewDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Review object is null!"
                };
            }

            var reviewToUpdate = await _reviewRepository.GetReviewId(updatedReview.Id);

            if (reviewToUpdate == null)
            {
                return new ResponseDto<ReviewDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Review not found!"
                };
            }

            reviewToUpdate.Title = updatedReview.Title;

            reviewToUpdate.Text = updatedReview.Text;

            reviewToUpdate.Rating = updatedReview.Rating;

            reviewToUpdate.ReviewerId = updatedReview.ReviewerId;

            reviewToUpdate.PokemonId = updatedReview.PokemonId;

            await _reviewRepository.UpdateReview(reviewToUpdate);

            return new ResponseDto<ReviewDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Review updated successfully!",
                ResponseData = reviewToUpdate
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<ReviewDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }

    }

    [HttpDelete("{reviewId}")]
    public ResponseDto<ReviewDto> DeleteReview(int reviewId)
    {
        try
        {
            var reviewToDelete = _reviewRepository.GetReviewId(reviewId);
            
            if (reviewToDelete == null)
            {
                return new ResponseDto<ReviewDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Review not found!"
                };
            }

            _reviewRepository.DeleteReview(reviewId);

            return new ResponseDto<ReviewDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Review deleted successfully!",
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<ReviewDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }

    [HttpGet("{pokeId}/rating")]
    public async Task<ResponseDto<ReviewRatingDto>> GetPokemonRating(int pokeId)
    {
        try
        {
            var pokemon = await _pokemonRepository.GetPokemonId(pokeId);

            if (pokemon == null)
            {
                return new ResponseDto<ReviewRatingDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Pokemon not found!"
                };
            }

            var rating = await _reviewRepository.GetPokemonRating(pokeId);

            if (rating == null)
            {
                return new ResponseDto<ReviewRatingDto>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    StatusMessage = "Pokemon rating not found!"
                };
            }

            return new ResponseDto<ReviewRatingDto>()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Pokemon rating retrived successfully!",
                ResponseData = rating
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<ReviewRatingDto>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusMessage = ex.Message
            };
        }
    }
}
