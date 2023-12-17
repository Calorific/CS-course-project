using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CS_course_project.Commands;
using CS_course_project.Model.Services.AuthServices;
using CS_course_project.model.Storage;
using CS_course_project.Navigation;

namespace CS_course_project.ViewModel; 

public class LoginViewModel : NotifyErrorsViewModel {
    private bool _firstRender = true;
    
    public ICommand SubmitCommand => Command.Create(LogIn);

    private async Task LogInAdmin() {
        var error = await AuthService.LogIn(Password, true);
        
        if (error == "WRONG_PASSWORD")
            AddError(nameof(Password), "Неверный пароль");
        else if (error != null) 
            AddError(nameof(Password), "Произошла ошибка. Попробуйте позже");
        else
            Navigator.Navigate.Execute("AdminPanel", null);
    }
    private async Task LogInUser() {
        var error = await AuthService.LogIn(Group, false);
        
        if (error == "INVALID_GROUP")
            AddError(nameof(Group), "Расписание еще не назначен");
        else if (error != null) 
            AddError(nameof(Group), "Произошла ошибка. Попробуйте позже");
        else
            Navigator.Navigate.Execute("Timetable", null);
    }
    private async void LogIn(object? sender, EventArgs e) {
        if (IsAdmin)
            await LogInAdmin();
        else 
            await LogInUser();
    }

    private List<string> _groups = new();

    public List<string> Groups {
        get => _groups;
        private set {
            if (_groups == value) return;
            _groups = value;
            Notify();
        }
    }

    public Visibility GroupVisibility { get; set; } = Visibility.Visible;
    public Visibility PasswordVisibility { get; set; } = Visibility.Collapsed;

    private bool _isAdmin;
    public bool IsAdmin {
        get => _isAdmin;
        set {
            _isAdmin = value;
            PasswordVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            GroupVisibility = !value ? Visibility.Visible : Visibility.Collapsed;
            Group = ""; Password = ""; CanSubmit = false;
            NotifyAll(nameof(IsAdmin), nameof(GroupVisibility), nameof(PasswordVisibility));
        }
    }
    
    private string _password = "";
    public string Password {
        get => _password;
        set {
            _password = value;
            ClearErrors(nameof(Password));
            if (_firstRender)
                _firstRender = false;
            else if (IsAdmin && _password.Length == 0) 
                AddError(nameof(Password), "Нужно ввести пароль");
            else 
                CanSubmit = true;
            
            Notify();
        }
    }

    private string _group = "";
    public string Group {
        get => _group;
        set {
            _group = value;
            ClearErrors(nameof(Group));
            if (_firstRender)
                _firstRender = false;
            else if (!IsAdmin && !Groups.Contains(_group)) 
                AddError(nameof(Group), "Нужно выбрать существующую группу");
            else 
                CanSubmit = true;
            Notify();
        }
    }

    private bool _canSubmit;
    public bool CanSubmit {
        get => _canSubmit;
        set {
            _canSubmit = value;
            Notify();
        }
    }

    private new void AddError(string propertyName, string error) {
        base.AddError(propertyName, error);
        CanSubmit = false;
    }

    private async void LoadData() {
        Groups = new List<string>((await DataManager.LoadTimetables()).Keys);
        Group = Groups[0];
    }
    
    private async void CheckSession() {
        var session = await DataManager.LoadSession();
        if (session == null) {
            LoadData();
            return;
        }
        
        var error = await AuthService.LogIn(session.Data, session.IsAdmin);
        
        if (error == null) {
            Navigator.Navigate.Execute(session.IsAdmin ? "AdminPanel" : "Timetable", null);
            return;
        }
        
        LoadData();
    }
    
    public LoginViewModel() {
        CheckSession();
    }
}