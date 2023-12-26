using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CS_course_project.Commands;
using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Storage;
using CS_course_project.ViewModel.Common;

namespace CS_course_project.ViewModel.Pages; 

public class LoginViewModel : NotifyErrorsViewModel {
    private readonly INavigator? _navigator;
    private readonly IDataManager? _dataManager;
    private readonly IAuthService? _authService;
    private bool _firstRender = true;
    
    public ICommand SubmitCommand => Command.Create(LogIn);

    private async Task LogInAdmin() {
        if (_authService == null || _navigator == null) return;
        var error = await _authService.LogIn(Password, true);
        
        if (error == "WRONG_PASSWORD")
            AddError(nameof(Password), "Неверный пароль");
        else if (error != null) 
            AddError(nameof(Password), "Произошла ошибка. Попробуйте позже");
        else
            _navigator.Navigate(Routes.AdminPanel);
    }
    private async Task LogInUser() {
        if (_authService == null || _navigator == null) return;
        var error = await _authService.LogIn(Group, false);
        
        if (error == "INVALID_GROUP")
            AddError(nameof(Group), "Расписание еще не назначен");
        else if (error != null) 
            AddError(nameof(Group), "Произошла ошибка. Попробуйте позже");
        else
            _navigator.Navigate(Routes.Timetable);
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
            if (Groups.Count == 0 && value != true) return;
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
        if (_dataManager == null) return;
        Groups = new List<string>((await _dataManager.GetTimetables()).Keys);
        if (Group.Length == 0) IsAdmin = true;
        else Group = Groups[0];
    }
    
    private async void CheckSession() {
        if (_dataManager == null || _authService == null || _navigator == null) return;
        var session = await _dataManager.GetSession();
        if (session == null) {
            LoadData();
            return;
        }
        
        var error = await _authService.LogIn(session.Data, session.IsAdmin);
        
        if (error == null) {
            _navigator.Navigate(session.IsAdmin ? Routes.AdminPanel : Routes.Timetable);
            return;
        }
        
        LoadData();
    }
    
    public LoginViewModel(INavigator navigator, IDataManager dataManager, IAuthService authService) {
        _navigator = navigator;
        _dataManager = dataManager;
        _authService = authService;
        CheckSession();
    }
    
    public LoginViewModel() {}
}