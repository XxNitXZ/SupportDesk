using System.Text.Json.Serialization;

namespace SupportDesk.Domain;

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }

    public Customer(string name, string email)
    {
        name = name.Trim();
        email = email.Trim();

        Validate(name, email);

        Id = Guid.NewGuid();
        Name = name;
        Email = email;
    }

    [JsonConstructor]
    public Customer(Guid id, string name, string email)
    {
        name = name.Trim();
        email = email.Trim();

        Validate(name, email);

        Id = id;
        Name = name;
        Email = email;
    }

    public void UpdateContactInfo(string name, string email)
    {
        name = name.Trim();
        email = email.Trim();

        Validate(name, email);

        Name = name;
        Email = email;
    }

    private static void Validate(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");

        if (name.Contains("   "))
            throw new ArgumentException("Name cannot contain multiple spaces.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.");

        if (email.Contains(' '))
            throw new ArgumentException("Email cannot contain spaces.");

        if (!email.Contains('@'))
            throw new ArgumentException("Email is not valid.");
    }
}