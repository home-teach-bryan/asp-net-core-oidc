using System.ComponentModel.DataAnnotations;

namespace AspNetCoreJwt.Models;

public class GenerateTokenRequest
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Password { get; set; }
}