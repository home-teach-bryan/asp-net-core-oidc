namespace AspNetCoreOidc.Models;

public class KeyCloakConfig
{
    public string Authority { get; set; }
    public string Audience { get; set; }
    
    public string MetaAddress { get; set; }
}