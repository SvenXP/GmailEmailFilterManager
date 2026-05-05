namespace GmailEmailFilterManager.Services;

public class AuthService
{
    private readonly string _username;
    private readonly string _password;
    private readonly string _tokenValue;

    private bool _isAuthenticated = false;
    public bool IsAuthenticated => _isAuthenticated;

    // Einfacher statischer Token (kein JWT nötig für lokale App)
    public string TokenKey => "gmailfilter_auth";

    public AuthService(IConfiguration config)
    {
        _username = config["AdminUser:Username"] ?? "admin";
        _password = config["AdminUser:Password"] ?? "changeme123";
        // Token = Hash aus Username+Password (nicht reversibel)
        _tokenValue = Convert.ToBase64String(
            System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(_username + ":" + _password)));
    }

    public bool Login(string username, string password)
    {
        if (username.Trim() == _username && password == _password)
        {
            _isAuthenticated = true;
            return true;
        }
        return false;
    }

    // Prüft ob gespeicherter Token noch gültig ist
    public bool ValidateToken(string? token)
    {
        if (string.IsNullOrEmpty(token)) return false;
        if (token == _tokenValue)
        {
            _isAuthenticated = true;
            return true;
        }
        return false;
    }

    public string GetToken() => _tokenValue;

    public void Logout()
    {
        _isAuthenticated = false;
    }
}