namespace SupportDesk.Domain.Tickets;

internal class Comment
{
    public string Text { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Comment(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Kommentar får inte vara tom.");

        Text = text;
        CreatedAt = DateTime.Now;
    }
}
