using System.Text.Json;

namespace GmailEmailFilterManager.Services;

public class EmailService
{
    private readonly string _filePath;
    private List<string> _emails = new();

    public EmailService(IWebHostEnvironment env)
    {
        // Speichert emails.json im App_Data Ordner (wird automatisch erstellt)
        var folder = Path.Combine(env.ContentRootPath, "App_Data");
        Directory.CreateDirectory(folder);
        _filePath = Path.Combine(folder, "emails.json");
        Load();
    }

    public IReadOnlyList<string> Emails => _emails.AsReadOnly();

    /// <summary>E-Mail hinzufügen. Gibt false zurück wenn Duplikat.</summary>
    public bool Add(string email)
    {
        var val = email.Trim().ToLowerInvariant();
        if (_emails.Contains(val)) return false;
        _emails.Add(val);
        Save();
        return true;
    }

    public void Remove(string email)
    {
        _emails.Remove(email);
        Save();
    }

    public void Clear()
    {
        _emails.Clear();
        Save();
    }

    public string GenerateOrString() => string.Join(" OR ", _emails);

    // ── Persistenz ────────────────────────────────────────
    private void Load()
    {
        if (!File.Exists(_filePath)) return;
        try
        {
            var json = File.ReadAllText(_filePath);
            _emails = JsonSerializer.Deserialize<List<string>>(json) ?? new();
        }
        catch
        {
            _emails = new();
        }
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(_emails, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}