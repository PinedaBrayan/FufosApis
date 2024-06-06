
namespace FufosEntities.DTOS
{
    public class UserDTO
    {
        public string? FullName {get; set;}
        public string Email {get; set;} = null!;
        public string Password {get; set;} = null!;
    }
}