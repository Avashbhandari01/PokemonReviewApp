using Common.DTOs;
using DataAccess.Repositories.GenericRepository;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class ReviewService : IReviewService
{
    private readonly IGenericRepository<Review> _genericRepository;
    private readonly DataContext _context;

    public ReviewService(DataContext context, IGenericRepository<Review> genericRepository)
    {
        _context = context;
        _genericRepository = genericRepository;
    }

    public async Task<bool> CreateReview(Review review)
    {
        var pokemon = await _genericRepository.GetByIDAsync<Pokemon>(review.PokemonId);

        if (pokemon == null)
        {
            return false;
        }

        var reviewer = await _genericRepository.GetByIDAsync<Reviewer>(review.ReviewerId);

        if (reviewer == null)
        {
            return false;
        }

        var newReview = new Review()
        {
            Title = review.Title,
            Text = review.Text,
            Rating = review.Rating,
            PokemonId = review.PokemonId,
            ReviewerId = review.ReviewerId,
        };

        return await _genericRepository.AddAsync(newReview);
    }

    public async Task<List<ReviewDto>> GetAllReviews()
    {
        var reviews = await _genericRepository.GetAllAsync();

        return reviews.Select(r => new ReviewDto()
        {
            Id = r.Id,
            Title = r.Title,
            Text = r.Text,
            Rating = r.Rating,
        }).ToList();
    }

    public async Task<ReviewDto> GetReviewId(int reviewId)
    {
        var review = await _genericRepository.GetByIDAsync<Review>(reviewId);

        return new ReviewDto()
        {
            Id = review.Id,
            Title = review.Title,
            Text = review.Text,
            Rating = review.Rating,
        };
    }

    public async Task UpdateReview(ReviewDto review)
    {
        var newReview = new Review()
        {
            Id = review.Id,
            Title = review.Title,
            Text = review.Text,
            Rating = review.Rating,
            ReviewerId = review.ReviewerId,
            PokemonId = review.PokemonId,
        };

        await _genericRepository.UpdateAsync(newReview);
    }

    public void DeleteReview(int reviewId)
    {
        _genericRepository.Delete<Review>(reviewId);
    }

    public async Task DeleteReviewFromPokemonId(int pokeId)
    {
        var reviews = await _genericRepository.GetAllAsync();

        var review = reviews.Where(r => r.Pokemon.Id == pokeId).ToList();

        foreach (var item in review)
        {
            _genericRepository.Delete<Review>(item.Id);
        }
    }

    public async Task<List<ReviewDto>> GetReviewsOfAPokemon(int pokeId)
    {
        var reviews = await _genericRepository.GetAllAsync();

        var result = new List<ReviewDto>();

        foreach (var review in reviews)
        {
            if (review.PokemonId == pokeId)
            {
                result.Add(new ReviewDto()
                {
                    Id = review.Id,
                    Title = review.Title,
                    Text = review.Text,
                    Rating = review.Rating,
                });
            } 
        }

        if (result.Count() == 0)
        {
            return null;
        }

        return result;
    }

    public async Task<ReviewRatingDto> GetPokemonRating(int pokeId)
    {
        var reviews = await _genericRepository.GetAllAsync();

        var review = reviews.Where(r => r.PokemonId == pokeId).ToList();

        var ratingToShow = new List<ReviewRatingDto>();

        foreach (var item in review)
        {
            ratingToShow.Add(new ReviewRatingDto()
            {
                Rating = item.Rating,
            });
        }

        if (review.Count() <= 0)
            return null;

        var result = new ReviewRatingDto()
        {
            Rating = GetAverageRating(ratingToShow),
        };

        return result;
    }

    private decimal GetAverageRating(List<ReviewRatingDto> ratingToShow)
    {
        if (ratingToShow.Count() <= 0)
            return 0;

        decimal averageRating = (decimal)ratingToShow.Sum(r => r.Rating) / ratingToShow.Count;
        return averageRating;
    }
}
    
