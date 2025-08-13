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
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("  ", false)]
    [InlineData("a", true)] // Minimum length
    [InlineData("username_with_underscores", true)]
    [InlineData("user@name", false)] // Caractères spéciaux non autorisés
    public void Username_Validation(string username, bool isValid)
    {
        var user = new UserAccount();

        if (isValid)
        {
            user.Username = username;
            user.Username.Should().Be(username);
        }
        else
        {
            user.Invoking(u => u.Username = username)
                .Should().Throw<ValidationException>()
                .WithMessage("Invalid username");
        }
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("not-an-email", false)]
    [InlineData("user@domain.com", true)]
    [InlineData("user.name+tag@sub.domain.com", true)]
    public void Email_Validation(string email, bool isValid)
    {
        var user = new UserAccount();

        if (isValid)
        {
            user.Email = email;
            user.Email.Should().Be(email);
        }
        else
        {
            user.Invoking(u => u.Email = email)
                .Should().Throw<ValidationException>()
                .WithMessage("Invalid email format");
        }
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("short", false)]
    [InlineData("longenough", true)]
    [InlineData("P@ssw0rd!", true)]
    public void Password_Validation(string password, bool isValid)
    {
        var user = new UserAccount();

        if (isValid)
        {
            user.Password = password;
            user.Password.Should().Be(password);
        }
        else
        {
            user.Invoking(u => u.Password = password)
                .Should().Throw<ValidationException>()
                .WithMessage("Password does not meet requirements");
        }
    }

    [Theory]
    [InlineData("Admin", true)]
    [InlineData("User", true)]
    [InlineData("Manager", true)]
    [InlineData("InvalidRole", false)]
    [InlineData("", false)]
    public void Role_Validation(string role, bool isValid)
    {
        var user = new UserAccount();

        if (isValid)
        {
            user.Role = role;
            user.Role.Should().Be(role);
        }
        else
        {
            user.Invoking(u => u.Role = role)
                .Should().Throw<ValidationException>()
                .WithMessage("Invalid role specified");
        }
    }

    [Fact]
    public void SetHashedPassword_ShouldNotStorePlainText()
    {
        // Arrange
        var user = new UserAccount();
        var plainPassword = "MySecurePassword123";

        // Act
        user.SetHashedPassword(plainPassword); // Méthode à implémenter

        // Assert
        user.Password.Should().NotBeNullOrEmpty();
        user.Password.Should().NotBe(plainPassword);
        user.Password.Should().StartWith("$2a$"); // Format BCrypt typique
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
}