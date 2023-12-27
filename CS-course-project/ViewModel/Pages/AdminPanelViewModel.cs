using System;
using System.Windows.Input;
using CS_course_project.Model.Services.AuthServices;
using CS_course_project.ViewModel.Commands;
using CS_course_project.ViewModel.Common;

namespace CS_course_project.ViewModel.Pages;

public class AdminPanelViewModel : NotifyErrorsViewModel {
    private readonly INavigator? _navigator;
    private readonly IAuthService? _authService;
    public ICommand SettingsRedirectCommand => Command.Create(SettingsRedirect);

    private void SettingsRedirect(object? sender, EventArgs e) {
        _navigator?.Navigate(Routes.Settings);
    }

    public ICommand LogOutCommand => Command.Create(LogOut);

    private async void LogOut(object? sender, EventArgs e) {
        if (_authService == null) return;
        await _authService.LogOut();
        _navigator?.Navigate(Routes.Login);
    }

    public AdminPanelViewModel(INavigator navigator, IAuthService authService) {
        _navigator = navigator;
        _authService = authService;
    }

    public AdminPanelViewModel() { }
}