
namespace FufosApis.Services
{
    public interface ITokenConfiguration
    {
        string Salt { get; set; }
        string Secret { get; set; }
        int MinutesExp { get; set; }
    }

    public class TokenConfiguration : ITokenConfiguration
    {
        public string Salt {get; set;} = string.Empty;
        public string Secret {get; set;} = string.Empty;
        public int MinutesExp {get; set;}
    }
}