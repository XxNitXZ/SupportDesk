using System;
﻿using System.Text.Json.Serialization;

namespace SupportDesk.Domain;

public class Customer
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Email { get; private set; }

    public Customer(string name, string email)
        : this(name, email, Guid.NewGuid())
    {
    }

    [JsonConstructor]
    public Customer(string name, string email, Guid id)
    {
        Validate(name, email);

        Id = id;
        Name = name.Trim();
        Email = email.Trim();
    }

    public void UpdateContactInfo(string name, string email)
    {
        Validate(name, email);

        Name = name.Trim();
        Email = email.Trim();
    }

    private static void Validate(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.");

        if (!email.Trim().Contains('@'))
            throw new ArgumentException("Email is not valid.");
    }
}