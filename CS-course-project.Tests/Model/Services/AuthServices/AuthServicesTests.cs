using CS_course_project.Model.Services.AuthServices;

namespace CS_course_project.Tests.Model.Services.AuthServices; 

public class AuthServicesTests {
    private readonly AuthService _authService = new(new MockAuthDataManager());
    
    [Fact]
    public async void ShouldReturnNullWhenPasswordIsCorrect() {
        // Arrange
        const bool isAdmin = true;
        const string password = "123";
        object? expected = null;
        
        
        // Act
        var res = await _authService.LogIn(password, isAdmin);
        
        
        // Assert
        Assert.Equal(expected, res);
    }
    
    [Fact]
    public async void ShouldReturnStringWhenPasswordIsNotCorrect() {
        // Arrange
        const bool isAdmin = true;
        const string password = "1234";
        const string expected = "WRONG_PASSWORD";
        
        
        // Act
        var res = await _authService.LogIn(password, isAdmin);
        
        
        // Assert
        Assert.Equal(expected, res);
    }
    
    [Fact]
    public async void ShouldReturnNullWhenGroupExists() {
        // Arrange
        const bool isAdmin = false;
        const string group = "group";
        object? expected = null;
        
        
        // Act
        var res = await _authService.LogIn(group, isAdmin);
        
        
        // Assert
        Assert.Equal(expected, res);
    }
    
    [Fact]
    public async void ShouldReturnStringWhenGroupNotExists() {
        // Arrange
        const bool isAdmin = false;
        const string group = "wrongGroup";
        const string expected = "INVALID_GROUP";
        
        
        // Act
        var res = await _authService.LogIn(group, isAdmin);
        
        
        // Assert
        Assert.Equal(expected, res);
    }
}