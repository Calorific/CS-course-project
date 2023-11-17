﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using CS_course_project.Commands;
using CS_course_project.Model.Services;
using CS_course_project.model.Storage;
using CS_course_project.Navigation;

namespace CS_course_project.ViewModel; 

public class LoginViewModel : NotifyErrorsViewModel {
    public List<string> Groups { get; } = new() { "AVT-213", "ФИТ-213" };

    private bool _firstRender = true;
    
    public ICommand SubmitCommand => Command.Create(LogIn);
    private async void LogIn(object? sender, EventArgs e) {
        var data = IsAdmin ? BCrypt.Net.BCrypt.HashPassword(Password) : Group;
        var error = await AuthService.LogIn(data, IsAdmin);
        
        if (error == "WRONG_PASSWORD" && IsAdmin)
            AddError(nameof(Password), "Неверный пароль");
        else if (error != null) {
            AddError(nameof(Group), "Произошла ошибка. Попробуйте позже");
        }
        else if (IsAdmin) {
            Navigator.Navigate.Execute("AdminPanel", null);
        }
    }

    public Visibility GroupVisibility { get; set; } = Visibility.Collapsed;
    public Visibility PasswordVisibility { get; set; } = Visibility.Visible;

    private bool _isAdmin = true;
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

    private static async void CheckSession() {
        var session = await DataManager.LoadSession();
        if (session.Data == null) return;
        
        var error = await AuthService.LogIn(session.Data, session.IsAdmin);
        
        if (error != null) await DataManager.RemoveSession();
        else Navigator.Navigate.Execute("AdminPanel", null);
    }
    
    public LoginViewModel() {
        CheckSession();
    }
}