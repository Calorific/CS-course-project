using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using CS_course_project.Commands;
using CS_course_project.model.Storage;
using CS_course_project.Model.Timetables;

namespace CS_course_project.ViewModel.UserControls;

public class ListItem : BaseViewModel {
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

    public ListItem(string data, bool isSelected) {
        Data = data;
        IsSelected = isSelected;
    }
}

public partial class SettingsFormViewModel : NotifyErrorsViewModel {
    private ISettings _settings;
    
    public ICommand UpdatePasswordCommand => Command.Create(UpdatePassword);
    private async void UpdatePassword(object? sender, EventArgs e) {
        if (_newPassword.Length == 0) {
            AddError(nameof(NewPassword), "Необходимо указать значение");
            return;
        }
        ClearErrors(nameof(NewPassword));
        var settings = new Settings(int.Parse(LessonDuration), int.Parse(BreakDuration),
            int.Parse(LongBreakDuration), ParseTime(StartTime), _newPassword,
            int.Parse(LessonsNumber), _settings.LongBreakLessons);
        _settings = settings;
        await DataManager.UpdateSettings(settings);
        await DataManager.UpdateSession(new Session(true, BCrypt.Net.BCrypt.HashPassword(NewPassword)));
        NewPassword = string.Empty;
    }
    
    public ICommand SubmitCommand => Command.Create(ChangeSettings);
    private async void ChangeSettings(object? sender, EventArgs e) {
        if (HasErrors) return;

        var longBreaks = (from item in LessonsArray where item.IsSelected select int.Parse(item.Data) - 1).ToList();
        
        try {
            var settings = new Settings(int.Parse(LessonDuration), int.Parse(BreakDuration), int.Parse(LongBreakDuration),
                ParseTime(StartTime), _settings.AdminPassword, int.Parse(LessonsNumber), longBreaks);
            await DataManager.UpdateSettings(settings);
        }
        catch (Exception error) {
            Console.WriteLine(error.Message);
            AddError(nameof(StartTime), "Некорректное значение");
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
            if (_lessonDuration.Length == 0)
                AddError(nameof(LessonDuration), "Необходимо указать значение");
            else 
                ClearErrors(nameof(LessonDuration));
            Notify();
        }
    }
    
    private string _breakDuration = string.Empty;
    public string BreakDuration {
        get => _breakDuration;
        set {
            _breakDuration = value;
            if (_breakDuration.Length == 0)
                AddError(nameof(BreakDuration), "Необходимо указать значение");
            else 
                ClearErrors(nameof(BreakDuration));
            Notify();
        }
    }

    private string _longBreakDuration = string.Empty;
    public string LongBreakDuration {
        get => _longBreakDuration;
        set {
            _longBreakDuration = value;
            if (_longBreakDuration.Length == 0)
                AddError(nameof(LongBreakDuration), "Необходимо указать значение");
            else 
                ClearErrors(nameof(LongBreakDuration));
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
            if (_lessonsNumber.Length == 0)
                AddError(nameof(LessonsNumber), "Необходимо указать значение");
            if (int.Parse(_lessonsNumber) > 20)
                AddError(nameof(LessonsNumber), "Значение не должно превышать 20");
            else 
                ClearErrors(nameof(LessonsNumber));
            
            Notify();
        }
    }

    
    private List<ListItem> _lessonsArray = new();
    public List<ListItem> LessonsArray {
        get => _lessonsArray;
        private set {
            _lessonsArray = value;
            Notify();
        }
    }

    private static int ParseTime(string time) {
        var parts = time.Split(':');
        var res = 0;
        
        if (int.TryParse(parts[0], out var hours))
            res += hours * 60;
        if (parts.Length > 1 && int.TryParse(parts[1], out var minutes))
            res += minutes;
        return res;
    }

    private static string FormatTime(int time) {
        var minutes = time % 60;
        return (time / 60).ToString() + ':' + (minutes < 10 ? "0" : "") + minutes;
    }
    
    private async void Init() {
        var settings = await DataManager.LoadSettings();
        _settings = settings;
        LessonDuration = settings.LessonDuration.ToString();
        BreakDuration = settings.BreakDuration.ToString();
        LongBreakDuration = settings.LongBreakDuration.ToString();
        StartTime = FormatTime(settings.StartTime);
        LessonsNumber = settings.LessonsNumber.ToString();
        LessonsArray = Enumerable.Range(0, _settings.LessonsNumber)
            .Select(idx => new ListItem((idx + 1).ToString(), settings.LongBreakLessons.Contains(idx))).ToList();
    }

    public SettingsFormViewModel() {
        _settings = new Settings();
        Init();
    }
}