using System.Text.Json;

namespace SupportDesk.Infrastructure.Data;

public class JsonFileHandler
{
    public async Task<List<T>> ReadAsync<T>(string filePath)
    {
        System.Diagnostics.Debug.WriteLine($"JSON READ: {filePath}");
        System.Diagnostics.Debug.WriteLine($"JSON EXISTS: {File.Exists(filePath)}");

        if (!File.Exists(filePath))
            return new List<T>();

        string json = await File.ReadAllTextAsync(filePath);

        System.Diagnostics.Debug.WriteLine($"JSON CONTENT: {json}");

        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }

    public async Task WriteAsync<T>(string filePath, List<T> data)
    {
        string json = JsonSerializer.Serialize(data);
        await File.WriteAllTextAsync(filePath, json);
    }
}