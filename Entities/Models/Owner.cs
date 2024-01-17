using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models;

public class Owner
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Gym { get; set; }

    public int CountryId { get; set; }

    [ForeignKey("CountryId")]
    public virtual Country Country { get; set; }

    public ICollection<PokemonOwner> PokemonOwners { get; set; }
}
