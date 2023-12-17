using System;
using System.Windows.Input;
using CS_course_project.Commands;
using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Navigation;

namespace CS_course_project.ViewModel; 

public class AdminPanelViewModel : NotifyErrorsViewModel {
    public static ICommand SettingsRedirectCommand => Command.Create(SettingsRedirect);
    private static void SettingsRedirect(object? sender, EventArgs e) {
        Navigator.Navigate.Execute("Settings", null);
    }
    
    public static ICommand LogOutCommand => Command.Create(LogOut);
    private static async void LogOut(object? sender, EventArgs e) {
        await AuthService.LogOut();
        Navigator.Navigate.Execute("Login", null);
    }
}