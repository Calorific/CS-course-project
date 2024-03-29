﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Services.TimeServices;
using CS_course_project.Model.Storage;
using CS_course_project.Model.Timetables;
using CS_course_project.ViewModel.Commands;
using CS_course_project.ViewModel.Common;

namespace CS_course_project.ViewModel.Components.Settings;

public class Item : BaseViewModel {
    private readonly string _data = string.Empty;
    public string Data {
        get => _data;
        private init {
            _data = value;
            Notify();
        }
    }

    private bool _isSelected;
    public bool IsSelected {
        get => _isSelected;
        set {
            _isSelected = value;
            Notify();
        }
    }

    public Item(string data, bool isSelected) {
        Data = data;
        IsSelected = isSelected;
    }
}

public partial class SettingsFormViewModel : NotifyErrorsViewModel {
    private readonly IDataManager? _dataManager;
    private readonly ITimeConverter? _timeConverter;
    
    private ISettings _settings = new Model.Timetables.Settings();
    
    public ICommand UpdatePasswordCommand => Command.Create(UpdatePassword);
    private async void UpdatePassword(object? sender, EventArgs e) {
        if (_dataManager == null || _timeConverter == null) return;
        if (_newPassword.Length == 0) {
            AddError(nameof(NewPassword), "Необходимо указать значение");
            return;
        }
        ClearErrors(nameof(NewPassword));
        
        try {
            var settings = new Model.Timetables.Settings(int.Parse(LessonDuration), int.Parse(BreakDuration),
                        int.Parse(LongBreakDuration), _timeConverter.ParseTime(StartTime), BCrypt.Net.BCrypt.HashPassword(NewPassword),
                        int.Parse(LessonsNumber), _settings.LongBreakLessons);
            _settings = settings;
            await _dataManager.UpdateSettings(settings);
            await _dataManager.UpdateSession(new Session(true, NewPassword));
            NewPassword = string.Empty;
        }
        catch (ArgumentException exception) {
            Console.WriteLine(exception);
            AddError(nameof(NewPassword), exception.Message);
        }
    }
    
    public ICommand SubmitCommand => Command.Create(ChangeSettings);

    private async void ChangeSettings(object? sender, EventArgs e) {
        if (HasErrors || _dataManager == null || _timeConverter == null) return;

        try {
            var settings = new Model.Timetables.Settings(int.Parse(LessonDuration), int.Parse(BreakDuration), int.Parse(LongBreakDuration),
                _timeConverter.ParseTime(StartTime), _settings.HashedAdminPassword, int.Parse(LessonsNumber), 
                LongBreaks.Where(l => l < int.Parse(LessonsNumber) - 1).ToList());
            await _dataManager.UpdateSettings(settings);
            LessonsArray = Enumerable.Range(0, settings.LessonsNumber - 1)
                .Select(idx => new Item((idx + 1).ToString(), settings.LongBreakLessons.Contains(idx))).ToList();
        }
        catch (ArgumentException exception) {
            Console.WriteLine(exception);
            AddError(nameof(StartTime), exception.Message);
        }
    }

    private string _newPassword = string.Empty;
    public string NewPassword {
        get => _newPassword;
        set {
            _newPassword = value;
            ClearErrors(nameof(NewPassword));
            Notify();
        }
    }
    
    private string _lessonDuration = string.Empty;
    public string LessonDuration {
        get => _lessonDuration;
        set {
            _lessonDuration = value;
            ClearErrors(nameof(LessonDuration));
            
            if (_lessonDuration == string.Empty)
                AddError(nameof(LessonDuration), "Необходимо указать значение");
            else if (int.Parse(_lessonDuration) == 0)
                AddError(nameof(LessonDuration), "Значение не может быть 0");
            else if (IsExceedingTime()) 
                AddError(nameof(LessonDuration), "Значение слишком большое");
            
            Notify();
        }
    }
    
    private string _breakDuration = string.Empty;
    public string BreakDuration {
        get => _breakDuration;
        set {
            _breakDuration = value;
            ClearErrors(nameof(BreakDuration));
            
            if (_breakDuration == string.Empty)
                AddError(nameof(BreakDuration), "Необходимо указать значение");
            else if (IsExceedingTime())
                AddError(nameof(BreakDuration), "Значение слишком большое");
            
            Notify();
        }
    }

    private string _longBreakDuration = string.Empty;
    public string LongBreakDuration {
        get => _longBreakDuration;
        set {
            _longBreakDuration = value;
            ClearErrors(nameof(LongBreakDuration));
            
            if (_longBreakDuration == string.Empty)
                AddError(nameof(LongBreakDuration), "Необходимо указать значение");
            else if (IsExceedingTime())
                AddError(nameof(LongBreakDuration), "Значение слишком большое");
            
            Notify();
        }
    }
    
    
    [GeneratedRegex(@"\d\d\d")]
    private static partial Regex StartTimeRegex();
    private string _startTime = string.Empty;
    public string StartTime {
        get => _startTime;
        set {
            if (StartTimeRegex().Match(value).Value == string.Empty) 
                _startTime = value;
            if (_startTime.Length == 0)
                AddError(nameof(StartTime), "Необходимо указать значение");
            else 
                ClearErrors(nameof(StartTime));
            
            Notify();
        }
    }
    
    private string _lessonsNumber = string.Empty;
    public string LessonsNumber {
        get => _lessonsNumber;
        set {
            _lessonsNumber = value;
            ClearErrors(nameof(LessonsNumber));
            
            if (_lessonsNumber == string.Empty)
                AddError(nameof(LessonsNumber), "Необходимо указать значение");
            else if (int.Parse(_lessonsNumber) == 0)
                AddError(nameof(LessonsNumber), "Значение не может быть 0");
            else if (IsExceedingTime())
                AddError(nameof(LessonsNumber), "Значение слишком большое");
            
            Notify();
        }
    }
    
    private List<Item> _lessonsArray = new();
    private List<int> LongBreaks => (from item in LessonsArray where item.IsSelected select int.Parse(item.Data) - 1).ToList();
    public List<Item> LessonsArray {
        get => _lessonsArray;
        private set {
            _lessonsArray = value;
            Notify();
        }
    }

    private bool IsExceedingTime() {
        if (_timeConverter == null) return false;
        if (_lessonsNumber == string.Empty || _lessonDuration == string.Empty
              || _longBreakDuration == string.Empty || _breakDuration == string.Empty || _lessonsNumber == string.Empty) {
            return false;
        }
        var total = _timeConverter.ParseTime(_startTime)
                    + int.Parse(_lessonsNumber) * int.Parse(_lessonDuration)
                    + int.Parse(_longBreakDuration) * LongBreaks.Count
                    + int.Parse(_breakDuration) * (int.Parse(_lessonsNumber) - LongBreaks.Count - 1);
        
        return total > 24 * 60;
    }
    
    private async void Init() {
        if (_dataManager == null || _timeConverter == null) return;
        var settings = await _dataManager.GetSettings();
        _settings = settings;
        LessonDuration = settings.LessonDuration.ToString();
        BreakDuration = settings.BreakDuration.ToString();
        LongBreakDuration = settings.LongBreakDuration.ToString();
        StartTime = _timeConverter.FormatTime(settings.StartTime);
        LessonsNumber = settings.LessonsNumber.ToString();
        LessonsArray = Enumerable.Range(0, _settings.LessonsNumber - 1)
            .Select(idx => new Item((idx + 1).ToString(), settings.LongBreakLessons.Contains(idx))).ToList();
    }

    public SettingsFormViewModel(IDataManager dataManager, ITimeConverter timeConverter) {
        _dataManager = dataManager;
        _timeConverter = timeConverter;
        Init();
    }
    
    public SettingsFormViewModel() {}
}