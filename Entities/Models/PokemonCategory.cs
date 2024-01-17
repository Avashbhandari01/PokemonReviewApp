using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models;

public class PokemonCategory
{
    public int PokemonId { get; set; }

    [ForeignKey("PokemonId")]
    public virtual Pokemon Pokemon { get; set; }

    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; }
}
