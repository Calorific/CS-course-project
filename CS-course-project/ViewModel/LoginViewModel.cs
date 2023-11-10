using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CS_course_project.Commands;
using CS_course_project.Model.Services;
using CS_course_project.Navigation;

namespace CS_course_project.ViewModel; 

public class LoginViewModel : INotifyPropertyChanged, INotifyDataErrorInfo {
    public Dictionary<string, List<string>> Errors { get; } = new();
    public List<string> Groups { get; } = new() { "AVT-213", "ФИТ-213" };

    private bool _firstRender = true;

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    
    public ICommand SubmitCommand => Command.Create(LogIn);
    private void LogIn(object? sender, EventArgs e) {
        var data = IsAdmin ? Password : Group;
        var error = AuthService.LogIn(data, IsAdmin);
        
        if (error == "WRONG_PASSWORD" && IsAdmin)
            AddError(nameof(Password), "Неверный пароль");
        else if (error != null) {
            AddError(nameof(Group), "Произошла ошибка. Попробуйте позже");
        }
        else if (IsAdmin) {
            Navigator.Navigate.Execute("AdminPanel", null);
        }
        
    }
    
    private void OnPropertyChanged([CallerMemberName] string prop = "") {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
    
    private void OnPropertiesChanged(params string[] props) {
        foreach (var prop in props) 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    private void OnErrorsChanged(string propertyName) {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    public Visibility GroupVisibility { get; set; }
    public Visibility PasswordVisibility { get; set; }

    private bool _isAdmin = true;
    public bool IsAdmin {
        get => _isAdmin;
        set {
            _isAdmin = value;
            PasswordVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            GroupVisibility = !value ? Visibility.Visible : Visibility.Collapsed;
            Group = ""; Password = ""; CanSubmit = false;
            OnPropertiesChanged(nameof(IsAdmin), nameof(GroupVisibility), nameof(PasswordVisibility));
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
            
            OnPropertyChanged();
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
            OnPropertyChanged();
        }
    }
    
    public IEnumerable GetErrors(string? propertyName) {
        return Errors!.GetValueOrDefault(propertyName, new List<string>());
    }

    public bool HasErrors => Errors.Count > 0;

    private bool _canSubmit;
    public bool CanSubmit {
        get => _canSubmit;
        set {
            _canSubmit = value;
            OnPropertyChanged();
        }
    }

    private void AddError(string propertyName, string error) {
        if (!Errors.ContainsKey(propertyName)) 
            Errors.Add(propertyName, new List<string>());
        Errors[propertyName].Add(error);
        CanSubmit = false;
        OnErrorsChanged(propertyName);
    }

    private void ClearErrors(string propertyName) {
        if (Errors.Remove(propertyName)) 
            OnErrorsChanged(propertyName);
    }

    public LoginViewModel() {
        PasswordVisibility = Visibility.Collapsed;
        GroupVisibility = Visibility.Visible;
    }
}