using System.Text.Json.Serialization;

namespace SupportDesk.Domain.Tickets;

public class Comment
{
    public string Text { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Comment(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Comment cannot be empty");

        Text = text.Trim();
        CreatedAt = DateTime.Now;
    }

    [JsonConstructor]
    public Comment(string text, DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Comment cannot be empty");

        Text = text.Trim();
        CreatedAt = createdAt;
    }
}