using System;
using System.Windows.Input;
using CS_course_project.Commands;
using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Navigation;

namespace CS_course_project.ViewModel;

public class AdminPanelViewModel : NotifyErrorsViewModel {
    private readonly INavigator? _navigator;
    private readonly IAuthService? _authService;
    public ICommand SettingsRedirectCommand => Command.Create(SettingsRedirect);

    private void SettingsRedirect(object? sender, EventArgs e) {
        _navigator?.Navigate.Execute(Pages.Settings, null);
    }

    public ICommand LogOutCommand => Command.Create(LogOut);

    private async void LogOut(object? sender, EventArgs e) {
        if (_authService == null) return;
        await _authService.LogOut();
        _navigator?.Navigate.Execute(Pages.Login, null);
    }

    public AdminPanelViewModel(INavigator navigator, IAuthService authService) {
        _navigator = navigator;
        _authService = authService;
    }

    public AdminPanelViewModel() { }
}