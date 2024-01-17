using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models;

public class PokemonOwner
{
    public int PokemonId { get; set; }

    [ForeignKey("PokemonId")]
    public virtual Pokemon Pokemon { get; set; }

    public int OwnerId { get; set; }

    [ForeignKey("OwnerId")]
    public virtual Owner Owner { get; set; }
}
