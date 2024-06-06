
using System.ComponentModel.DataAnnotations;
using Appointment.SDK.Entities;

namespace FufosEntities.Entities;

public class Category : BaseEntity<short>
{
    [Required]
    [StringLength(60)]
    public string Name {get; set;} = null!;

    public virtual ICollection<Product>? Products {get; set;}

    public override string ToString() => $"{Name}";
}