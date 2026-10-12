using System;
using System.Text.Json;
using SupportDesk.Domain.Tickets;
using Xunit;

public class InvalidJsonTests
{
    [Fact]
    public void InvalidJson_ShouldThrowJsonException()
    {
        // Arrange
        var invalidJson = "this is not valid JSON";

        // Act & Assert
        Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<SupportTicket[]>(invalidJson));
    }
    [Fact]
    public void JsonObjectInsteadOfArray_ShouldThrowJsonException()
    {
        // Arrange
        var json = "{\"Title\":\"Test ticket\"}";

        // Act & Assert
        Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<SupportTicket[]>(json));
    }
    [Fact]
    public void EmptyJson_ShouldThrowJsonException()
    {
        // Arrange
        var json = "";

        // Act & Assert
        Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<SupportTicket[]>(json));
    }

}