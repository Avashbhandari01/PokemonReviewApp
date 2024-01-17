using Common.DTOs;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces;

public interface IReviewService
{
    Task<bool> CreateReview(Review review);

    Task<List<ReviewDto>> GetAllReviews();

    Task<ReviewDto> GetReviewId(int reviewId);

    Task UpdateReview(ReviewDto review);

    void DeleteReview(int reviewId);

    Task DeleteReviewFromPokemonId(int pokeId);

    Task<List<ReviewDto>> GetReviewsOfAPokemon(int pokeId);

    Task<ReviewRatingDto> GetPokemonRating(int pokeId);
}
