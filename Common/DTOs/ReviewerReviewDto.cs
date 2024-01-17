using PokemonReviewApp.Models;

namespace Common.DTOs;

public class ReviewerReviewDto
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public ICollection<Review> Reviews { get; set; }
}
