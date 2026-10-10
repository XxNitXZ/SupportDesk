using System.Text.Json;

namespace SupportDesk.Infrastructure.Data;

public class JsonFileHandler
{
    public async Task<List<T>> ReadAsync<T>(string filePath)
    {
        System.Diagnostics.Debug.WriteLine($"JSON READ: {filePath}");
        System.Diagnostics.Debug.WriteLine($"JSON EXISTS: {File.Exists(filePath)}");

        if (!File.Exists(filePath))
        {
            return new List<T>();
        }

        try
        {
            string json = await File.ReadAllTextAsync(filePath);

            System.Diagnostics.Debug.WriteLine($"JSON CONTENT: {json}");

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new InvalidOperationException(
                    $"Could not load '{Path.GetFileName(filePath)}'. The data file is empty or invalid.");
            }

            return JsonSerializer.Deserialize<List<T>>(json)
                   ?? new List<T>();
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(
                $"Could not load '{Path.GetFileName(filePath)}'. The data file contains invalid JSON.",
                ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new InvalidOperationException(
                $"Could not load '{Path.GetFileName(filePath)}'. Access to the file was denied.",
                ex);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException(
                $"Could not load '{Path.GetFileName(filePath)}'. The file could not be read.",
                ex);
        }
    }

    public async Task WriteAsync<T>(string filePath, List<T> data)
    {
        try
        {
            string? directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonSerializer.Serialize(data);

            System.Diagnostics.Debug.WriteLine($"SAVING JSON TO: {filePath}");

            await File.WriteAllTextAsync(filePath, json);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new InvalidOperationException(
                $"Could not save '{Path.GetFileName(filePath)}'. Access to the file was denied.",
                ex);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException(
                $"Could not save '{Path.GetFileName(filePath)}'. The file could not be written.",
                ex);
        }
    }
}