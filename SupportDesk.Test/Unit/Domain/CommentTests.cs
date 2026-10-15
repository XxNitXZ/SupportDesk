using SupportDesk.Domain.Tickets;

namespace SupportDesk.Tests.Domain;

public class CommentTests
{
    [Fact]
    public void Constructor_Should_CreateCommentWhenTextIsValid()
    {
        // Arrange
        var text = "This is a test comment";

        // Act
        var comment = new Comment(text);

        // Assert
        Assert.Equal(text, comment.Text);
        Assert.NotEqual(default, comment.CreatedAt);
    }

    [Fact]
    public void Constructor_Should_TrimWhitespaceFromText()
    {
        // Arrange
        var text = "  This is a test comment  ";

        // Act
        var comment = new Comment(text);

        // Assert
        Assert.Equal("This is a test comment", comment.Text);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_Should_ThrowExceptionWhenTextIsNullOrEmpty(string? text)
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new Comment(text!));

        // Assert
        Assert.Equal("Comment cannot be empty", exception.Message);
    }

    [Fact]
    public void JsonConstructor_Should_PreserveCreatedAt()
    {
        // Arrange
        var text = "Test comment";
        var createdAt = new DateTime(2025, 5, 10, 12, 30, 0);

        // Act
        var comment = new Comment(text, createdAt);

        // Assert
        Assert.Equal(text, comment.Text);
        Assert.Equal(createdAt, comment.CreatedAt);
    }

    [Fact]
    public void JsonConstructor_Should_TrimWhitespaceFromText()
    {
        // Arrange
        var text = "  Test comment  ";
        var createdAt = new DateTime(2025, 5, 10, 12, 30, 0);

        // Act
        var comment = new Comment(text, createdAt);

        // Assert
        Assert.Equal("Test comment", comment.Text);
        Assert.Equal(createdAt, comment.CreatedAt);
    }

    [Fact]
    public void JsonConstructor_Should_ThrowExceptionWhenTextIsEmpty()
    {
        // Arrange
        var createdAt = DateTime.Now;

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new Comment("", createdAt));

        // Assert
        Assert.Equal("Comment cannot be empty", exception.Message);
    }
}