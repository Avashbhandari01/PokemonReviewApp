using DataAccess.Repositories.GenericRepository;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerService : IReviewerService
    {
        private readonly IGenericRepository<Reviewer> _genericRepository;

        public ReviewerService(IGenericRepository<Reviewer> genericRepository)
        {
           _genericRepository = genericRepository;
        }

        public async Task<bool> CreateReviewer(Reviewer reviewer)
        {
            return await _genericRepository.AddAsync(reviewer);
        }

        public async Task<List<ReviewerDto>> GetAllReviewers()
        {
           var reviewers = await _genericRepository.GetAllAsync();

            return reviewers.Select(r => new ReviewerDto()
        {
                Id = r.Id,
                FirstName = r.FirstName,
                LastName = r.LastName
            }).ToList();
        }

        public async Task<ReviewerDto> GetReviewerId(int reviewerId)
        {
            var reviewer = await _genericRepository.GetByIDAsync<Reviewer>(reviewerId);

            return new ReviewerDto()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName,
            };
        }

        public async Task UpdateReviewer (ReviewerDto reviewer)
        {
            var newReviewer = new Reviewer()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName,
            };

            await _genericRepository.UpdateAsync(newReviewer);
        }

        public void DeleteReviewer(int reviewerId)
        {
            _genericRepository.Delete<Reviewer>(reviewerId);
        }

        public async Task<ReviewDto> GetReviewsByReviewer(int reviewerId)
        {
            var reviews = await _genericRepository.GetByIDAsync<Review>(reviewerId);

            var reviewsToShow = new ReviewDto()
            {
                Id = reviews.Id,
                Title = reviews.Title,
                Text = reviews.Text,
                Rating = reviews.Rating,
            };

            return reviewsToShow;
        }
    }
}
