
using System.Security.Cryptography;
using System.Text;

namespace FufosApis.Utilities
{
    public static class Utils{

        public static string HashTo256(string Value, string Salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] BytesMessage = Encoding.UTF8.GetBytes(Value + Salt);

                byte[] hashBytes = sha256.ComputeHash(BytesMessage);

                var sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public static bool Compare(string Salt, string Password, string EncryptedPassword)
        {
            var PassEncrypted = HashTo256(Password, Salt);
            return EncryptedPassword.Equals(PassEncrypted);
        }

        public static Type? SearchType(string Name)
        {
            var Result = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => x.GetType(Name) is not null)
                .Select(x => x.GetType(Name))
                .FirstOrDefault();

            return Result;
        }

    }
}