using FluentAssertions;
using Domain;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Tests.Unit.Domain;

public class UserAccountTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var user = new UserAccount();

        // Assert
        user.Id.Should().Be(0);
        user.Username.Should().BeEmpty();
        user.Email.Should().BeEmpty();
        user.Password.Should().BeEmpty();
        user.Role.Should().BeEmpty();
    }

    [Theory]
    [InlineData("john_doe", "john@example.com", "secure123", "Admin")]
    [InlineData("jane_doe", "jane@domain.com", "p@ssw0rd", "User")]
    public void PropertyAssignment_ShouldWorkCorrectly(
        string username, string email, string password, string role)
    {
        // Arrange
        var user = new UserAccount();

        // Act
        user.Username = username;
        user.Email = email;
        user.Password = password;
        user.Role = role;

        // Assert
        user.Username.Should().Be(username);
        user.Email.Should().Be(email);
        user.Password.Should().Be(password);
        user.Role.Should().Be(role);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("a")]
    [InlineData("username_with_underscores")]
    [InlineData("user@name")]
    public void Username_ShouldAcceptAnyValue(string username)
    {
        // Arrange
        var user = new UserAccount();

        // Act
        user.Username = username;

        // Assert
        user.Username.Should().Be(username);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("not-an-email")]
    [InlineData("user@domain.com")]
    [InlineData("user.name+tag@sub.domain.com")]
    public void Email_ShouldAcceptAnyValue(string email)
    {
        // Arrange
        var user = new UserAccount();

        // Act
        user.Email = email;

        // Assert
        user.Email.Should().Be(email);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("short")]
    [InlineData("longenough")]
    [InlineData("P@ssw0rd!")]
    public void Password_ShouldAcceptAnyValue(string password)
    {
        // Arrange
        var user = new UserAccount();

        // Act
        user.Password = password;

        // Assert
        user.Password.Should().Be(password);
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("User")]
    [InlineData("Manager")]
    [InlineData("InvalidRole")]
    [InlineData("")]
    public void Role_ShouldAcceptAnyValue(string role)
    {
        // Arrange
        var user = new UserAccount();

        // Act
        user.Role = role;

        // Assert
        user.Role.Should().Be(role);
    }

    [Fact]
    public void SetHashedPassword_ShouldNotStorePlainText()
    {
        // Arrange
        var user = new UserAccount();
        var plainPassword = "MySecurePassword123";

        // Act
        user.SetHashedPassword(plainPassword);

        // Assert
        user.Password.Should().NotBeNullOrEmpty();
        user.Password.Should().NotBe(plainPassword);
        user.Password.Should().StartWith("$2a$"); // Format BCrypt typique
    }

    [Fact]
    public void SetHashedPassword_WithEmptyPassword_ShouldThrowException()
    {
        // Arrange
        var user = new UserAccount();

        // Act & Assert
        user.Invoking(u => u.SetHashedPassword(""))
            .Should().Throw<ValidationException>()
            .WithMessage("Password cannot be empty");
    }

    [Fact]
    public void VerifyPassword_ShouldCheckAgainstHash()
    {
        // Arrange
        var user = new UserAccount();
        var correctPassword = "GoodPassword";
        var wrongPassword = "WrongPassword";
        user.SetHashedPassword(correctPassword);

        // Act & Assert
        user.VerifyPassword(correctPassword).Should().BeTrue();
        user.VerifyPassword(wrongPassword).Should().BeFalse();
    }

    [Fact]
    public void VerifyPassword_WithEmptyStoredPassword_ShouldReturnFalse()
    {
        // Arrange
        var user = new UserAccount();

        // Act & Assert
        user.VerifyPassword("anypassword").Should().BeFalse();
    }
}