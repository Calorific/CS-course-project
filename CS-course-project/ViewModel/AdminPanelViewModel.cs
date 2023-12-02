using System;
using System.Collections.Generic;
using System.Windows.Input;
using CS_course_project.Commands;
using CS_course_project.Model.Services;
using CS_course_project.model.Storage;
using CS_course_project.Model.Timetables;
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
    
    private List<string>? _groups;
    private List<ITeacher>? _teachers;
    private List<string>? _classrooms;
    private List<string>? _subjects;

    private async void CheckRedirect() {
        _groups = await DataManager.LoadGroups();
        _teachers = await DataManager.LoadTeachers();
        _classrooms = await DataManager.LoadClassrooms();
        _subjects = await DataManager.LoadSubjects();
        if (_groups?.Count == 0 || _teachers?.Count == 0 || _classrooms?.Count == 0 || _subjects?.Count == 0) {
            Navigator.Navigate.Execute("Settings", null);
        }
    }
    
    public AdminPanelViewModel() {
        CheckRedirect();
    }
}