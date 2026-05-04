using System.Text.Json;

namespace GmailEmailFilterManager.Services;

public class EmailList
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public List<string> Emails { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class EmailListService
{
    private readonly string _filePath;
    private List<EmailList> _lists = new();

    private static readonly JsonSerializerOptions _json = new() { WriteIndented = true };

    public EmailListService(IWebHostEnvironment env)
    {
        var folder = Path.Combine(env.ContentRootPath, "App_Data");
        Directory.CreateDirectory(folder);
        _filePath = Path.Combine(folder, "lists.json");
        Load();
    }

    public IReadOnlyList<EmailList> Lists => _lists.AsReadOnly();

    // ── Listen-Operationen ─────────────────────────────────
    public EmailList AddList(string name)
    {
        var list = new EmailList { Name = name.Trim() };
        _lists.Add(list);
        Save();
        return list;
    }

    public void RenameList(string id, string newName)
    {
        var list = _lists.FirstOrDefault(l => l.Id == id);
        if (list is null) return;
        list.Name = newName.Trim();
        Save();
    }

    public void DeleteList(string id)
    {
        _lists.RemoveAll(l => l.Id == id);
        Save();
    }

    // ── E-Mail-Operationen ─────────────────────────────────
    public bool AddEmail(string listId, string email)
    {
        var list = _lists.FirstOrDefault(l => l.Id == listId);
        if (list is null) return false;
        var val = email.Trim().ToLowerInvariant();
        if (list.Emails.Contains(val)) return false;
        list.Emails.Add(val);
        Save();
        return true;
    }

    public void RemoveEmail(string listId, string email)
    {
        var list = _lists.FirstOrDefault(l => l.Id == listId);
        list?.Emails.Remove(email);
        Save();
    }

    public void ClearEmails(string listId)
    {
        var list = _lists.FirstOrDefault(l => l.Id == listId);
        list?.Emails.Clear();
        Save();
    }

    public string GenerateOrString(string listId)
    {
        var list = _lists.FirstOrDefault(l => l.Id == listId);
        return list is null ? string.Empty : string.Join(" OR ", list.Emails);
    }

    // ── Persistenz ─────────────────────────────────────────
    private void Load()
    {
        if (!File.Exists(_filePath)) return;
        try
        {
            var json = File.ReadAllText(_filePath);
            _lists = JsonSerializer.Deserialize<List<EmailList>>(json) ?? new();
        }
        catch { _lists = new(); }
    }

    private void Save()
    {
        File.WriteAllText(_filePath, JsonSerializer.Serialize(_lists, _json));
    }
}