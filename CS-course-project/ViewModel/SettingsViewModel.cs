using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using CS_course_project.Commands;
using CS_course_project.model.Storage;
using CS_course_project.Model.Timetables;

namespace CS_course_project.ViewModel;

public class Test {
    public string Name { get; set; }

    public Test(string name) {
        Name = name;
    } 
    
}

public partial class SettingsViewModel : NotifyErrorsViewModel {
    public ICommand SubmitCommand => Command.Create(ChangeSettings);
    private async void ChangeSettings(object? sender, EventArgs e) {
        if (HasErrors) return;
        try {
            var settings = new Settings(int.Parse(LessonDuration), int.Parse(BreakDuration), int.Parse(LongBreakDuration), ParseTime(StartTime));
            await DataManager.UpdateSettings(settings);
        }
        catch (Exception error) {
            Console.WriteLine(error.Message);
            AddError(nameof(StartTime), "Некорректное значение");
        }
    }

    public ICommand RemoveCommand => Command.Create(RemoveItem);

    private void RemoveItem(object? sender, EventArgs e) {
        if (sender is string item) {
            Teachers.Remove(item);
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

    private ObservableCollection<string> _teachers;
    public ObservableCollection<string> Teachers {
        get => _teachers; 
        private set {
            if (_teachers == value) return;
            _teachers = value;
            Notify();
        }
    }

    private static int ParseTime(string time) {
        var parts = time.Split(':');
        var res = 0;
        
        if (int.TryParse(parts[0], out var hours))
            res += hours * 60;
        if (int.TryParse(parts[1], out var minutes))
            res += minutes;
        return res;
    }

    private static string FormatTime(int time) {
        var minutes = time % 60;
        return (time / 60).ToString() + ':' + (minutes < 10 ? "0" : "") + minutes;
    }
    
    private async void Init() {
        var settings = await DataManager.LoadSettings();
        var teachers = await DataManager.LoadTeachers();
        LessonDuration = settings.LessonDuration.ToString();
        BreakDuration = settings.BreakDuration.ToString();
        LongBreakDuration = settings.LongBreakDuration.ToString();
        StartTime = FormatTime(settings.StartTime);
        Teachers = new ObservableCollection<string>(teachers);
        // Teachers = teachers;
    }

    public SettingsViewModel() {
        _teachers = new ObservableCollection<string>();
        Init();
    }
}