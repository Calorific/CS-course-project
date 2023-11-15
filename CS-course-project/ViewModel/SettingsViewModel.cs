using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using CS_course_project.Commands;
using CS_course_project.model.Storage;
using CS_course_project.Model.Timetables;

namespace CS_course_project.ViewModel;

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

    public ICommand RemoveGroupCommand => Command.Create(RemoveGroup);
    private void RemoveGroup(object? sender, EventArgs e) {
        if (sender is string item) {
            Groups.Remove(item);
        }
    }
    
    public ICommand RemoveTeacherCommand => Command.Create(RemoveTeacher);
    private void RemoveTeacher(object? sender, EventArgs e) {
        if (sender is string item) {
            Teachers.Remove(item);
        }
    }
    
    public ICommand RemoveClassroomCommand => Command.Create(RemoveClassroom);
    private void RemoveClassroom(object? sender, EventArgs e) {
        if (sender is string item) {
            Classrooms.Remove(item);
        }
    }
    
    public ICommand RemoveSubjectCommand => Command.Create(RemoveSubject);
    private void RemoveSubject(object? sender, EventArgs e) {
        if (sender is string item) {
            Subjects.Remove(item);
        }
    }
    
    public ICommand Test => Command.Create(T);
    private void T(object? sender, EventArgs e) {
        foreach (var t in Groups) 
            Console.WriteLine(t);
        Console.WriteLine("");
        
        foreach (var t in Teachers) 
            Console.WriteLine(t);
        Console.WriteLine("");
        
        foreach (var t in Classrooms) 
            Console.WriteLine(t);
        Console.WriteLine("");
        
        foreach (var t in Subjects) 
            Console.WriteLine(t);
        Console.WriteLine("");
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

    private ObservableCollection<string> _groups;
    public ObservableCollection<string> Groups {
        get => _groups; 
        set {
            if (_groups == value) return;
            _groups = value;
            Notify();
        }
    }
    
    private ObservableCollection<string> _teachers;
    public ObservableCollection<string> Teachers {
        get => _teachers; 
        set {
            if (_teachers == value) return;
            _teachers = value;
            Notify();
        }
    }
    
    private ObservableCollection<string> _classrooms;
    public ObservableCollection<string> Classrooms {
        get => _classrooms; 
        set {
            if (_classrooms == value) return;
            _classrooms = value;
            Notify();
        }
    }
    
    private ObservableCollection<string> _subjects;
    public ObservableCollection<string> Subjects {
        get => _subjects; 
        set {
            if (_subjects == value) return;
            _subjects = value;
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
        LessonDuration = settings.LessonDuration.ToString();
        BreakDuration = settings.BreakDuration.ToString();
        LongBreakDuration = settings.LongBreakDuration.ToString();
        StartTime = FormatTime(settings.StartTime);
        
        var groups = await DataManager.LoadGroups();
        Groups = new ObservableCollection<string>(groups);
        
        var teachers = await DataManager.LoadTeachers();
        Teachers = new ObservableCollection<string>(teachers);

        var classrooms = await DataManager.LoadClassrooms();
        Classrooms = new ObservableCollection<string>(classrooms);
        
        var subjects = await DataManager.LoadSubjects();
        Subjects = new ObservableCollection<string>(subjects);
    }
    
    public SettingsViewModel() {
        _groups = new ObservableCollection<string>();
        _teachers = new ObservableCollection<string>();
        _classrooms = new ObservableCollection<string>();
        _subjects = new ObservableCollection<string>();
        Init();
    }
}