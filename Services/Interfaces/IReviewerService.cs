using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces;

public interface IReviewerService
{
    Task<bool> CreateReviewer(Reviewer reviewer);

    Task<List<ReviewerDto>> GetAllReviewers();

    Task<ReviewerDto> GetReviewerId(int reviewerId);

    Task UpdateReviewer(ReviewerDto reviewer);

    void DeleteReviewer(int reviewerId);

    Task<ReviewDto> GetReviewsByReviewer(int reviewerId);
}
