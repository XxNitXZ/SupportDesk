namespace SupportDesk.Domain;

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }

    public Customer(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.");

        name = name.Trim();
        email = email.Trim();

        if (!email.Contains('@'))
            throw new ArgumentException("Email is not valid.");

        Id = Guid.NewGuid();
        Name = name;
        Email = email;
    }

    public void UpdateContactInfo(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.");

        name = name.Trim();
        email = email.Trim();

        if (!email.Contains('@'))
            throw new ArgumentException("Email is not valid.");

        Name = name;
        Email = email;
    }
}