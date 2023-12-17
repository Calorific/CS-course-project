using System;
using System.Windows.Input;
using CS_course_project.Commands;
using CS_course_project.Model.Services.AuthServices;
using CS_course_project.model.Storage;
using CS_course_project.Navigation;

namespace CS_course_project.ViewModel; 

public class TimetablePageViewModel : BaseViewModel {
    public static ICommand LogOutCommand => Command.Create(LogOut);
    private static async void LogOut(object? sender, EventArgs e) {
        await AuthService.LogOut();
        Navigator.Navigate.Execute("Login", null);
    }

    private string _title = string.Empty;

    public string Title {
        get => _title;
        private set {
            _title = value;
            Notify();
        }
    }

    private async void Init() {
        var session = await DataManager.LoadSession();
        Title = "Расписание " + session?.Data;
    }
    
    public TimetablePageViewModel() {
        Init();
    }
}