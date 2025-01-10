using ECommerceCom.Data.Enums;

namespace ECommerceCom.WepApi.Jwt
{
    public class JwtDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpireMinutes { get; set; }



    }
}
