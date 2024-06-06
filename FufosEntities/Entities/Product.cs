
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Appointment.SDK.Entities;

namespace FufosEntities.Entities;

public class Product : BaseEntity<short>
{
    [Required]
    [StringLength(60)]
    public string Name {get; set;} = null!;
    [Required]
    [StringLength(180)]
    public string Description {get; set;} = null!;

    [Required]
    [ForeignKey("Category")]
    public short RowidCategory {get; set;}
    public virtual Category? Category {get; set;}

    public override string ToString() => $"{Name}";
}