using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Storage;
using CS_course_project.ViewModel.Pages;

namespace CS_course_project.Tests.ViewModel.Pages.LoginViewModelPage;

public class LoginViewModelTests {
    private readonly INavigator _navigator = new MockNavigator();
    private readonly IDataManager _dataManager = new MockDataManager();
    private readonly IAuthService _authService = new MockAuthService();

    [Fact]
    public void ShouldHaveErrorsWhenPasswordIsEmpty() {
        // Arrange
        var loginViewModel = new LoginViewModel(_navigator, _dataManager, _authService) {
            Password = ""
        };
        const bool expected = true;
        
        
        // Act
        loginViewModel.IsAdmin = true;
        loginViewModel.Password = string.Empty;
        loginViewModel.SubmitCommand.Execute(null);
        
        // Assert
        Assert.Equal(expected, loginViewModel.HasErrors);
    }
    
    [Fact]
    public void ShoulAlwaysBeAdminWhenGroupsAreEmpty() {
        // Arrange
        var loginViewModel = new LoginViewModel(_navigator, _dataManager, _authService);
        const bool expected = true;
        
        
        // Act
        loginViewModel.IsAdmin = true;
        loginViewModel.IsAdmin = false;
        
        // Assert
        Assert.Equal(expected, loginViewModel.IsAdmin);
    }
}