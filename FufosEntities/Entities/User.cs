
using System.ComponentModel.DataAnnotations;
using Appointment.SDK.Entities;
using Microsoft.EntityFrameworkCore;

namespace FufosEntities.Entities;

[Index(nameof(Email), Name = "Ix_User__Email", IsUnique = true)]
public class User : BaseUser<int>
{
    [Required]
    [StringLength(128)]
    public string FullName { get; set; } = null!;
    [Required]
    [StringLength(128, MinimumLength = 6, ErrorMessage = "The password must have between 6 and 14 characters.")]
    public string Password {get; set;} = null!;
    public bool IsAdmin {get; set;}
    public bool IsEmployee {get; set;}

    public override string ToString() => $"{FullName}";
}