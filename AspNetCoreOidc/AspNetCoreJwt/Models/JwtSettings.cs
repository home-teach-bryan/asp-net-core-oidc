namespace AspNetCoreJwt.Models;

public class JwtSettings
{
    public string Issuer { get; set; }
    
    public string SignKey { get; set; }
}