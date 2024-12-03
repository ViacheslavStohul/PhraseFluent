namespace DistLearning.Service.Options;

public class TokenConfiguration
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiryMinutes { get; set; }
    
    public int RefreshTokenExpirationDays { get; set; }
    
    public string CertificatePassword { get; set; } = string.Empty;
}